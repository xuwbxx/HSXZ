using System;
using System.Collections.Generic;
using Interactive.Utility.Reflect;
using log4net;
using TcpTransmit.Core.Configuration.Client;

namespace TcpTransmit.Core.Client {
    /// <summary>
    /// 客户端宿主管理器
    /// </summary>
    public class HostManager {
        private static readonly HostManager instance;
        private static readonly ILog log;
        private readonly Dictionary<string, Host> _dictionary;

        static HostManager(){
            log = LogManager.GetLogger("HostManager");
            instance = new HostManager();
        }

        private HostManager(){
            _dictionary = new Dictionary<string, Host>(20);
            TcpTransmitSection configuraSection = TcpTransmitSection.Instance;
            if (configuraSection.Clients.Count < 1){
                log.Error("没有配置客户端,请注意!");
                throw new Exception("配置文件有问题,Ihandle没有或者配置失败!");
            }


            foreach (ClientElement client in configuraSection.Clients){
                var handler = Helper.GetObject(client.Handler.AssemblyName,
                                               client.Handler.ClassName) as IHandler;
                if (handler == null)
                    throw new Exception("配置文件有问题,Ihandle没有或者配置失败!");

                handler.Init(client.Handler.Argument);

                var host = new Host(handler, configuraSection.SocketBufferSize,
                                    configuraSection.MessageBufferSize,configuraSection.MinPoolsAsyncEventArgsNum,configuraSection.MaxPoolsAsyncEventArgsNum);

                if (client.Connectors == null || client.Connectors.Count < 1)
                    throw new Exception("配置文件有问题,Connectors配置失败!");


                for (int i = 0; i < client.Connectors.Count; i++){
                    ConnectorElement connector = client.Connectors[i];
                    host.AddConnector(connector.Name, connector.ReconnectAttempts,
                                      connector.ReconnectAttemptInterval,
                                      connector.IpEndPoint);
                }
                _dictionary.Add(client.Name, host);
            }
        }


        /// <summary>
        /// 默认客户端宿主
        /// </summary>
        public Host this[string key]{
            get { return _dictionary[key]; }
        }



        /// <summary>
        /// 实例
        /// </summary>
        public static HostManager Instance{
            get { return instance; }
        }
    }
}