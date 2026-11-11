using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FDScanSystem.Protocol;
using FDScanSystem.Client.Logic.MsgProvider;
using TcpTransmit.Core;
using FDScanSystem.Client.Logic.MsgSender;

namespace FDScanSystem.Client.Logic
{
    public class MessageFacade
    {

        static Dictionary<string, MessageProviderBase> MsgProviders;
        static List<byte> _buffer = new List<byte>(1024 * 1024);

        static HashSet<string> _UnResponseCode = new HashSet<string>(new List<string> {
            ClientMsgCode.Response_OK,
             
            ClientMsgCode.Response_Error
        });

        static MessageFacade()
        {
            MsgProviders = new Dictionary<string, MessageProviderBase>();
            MsgProviders.Add(ClientMsgCode.ServerToClient_Caiji, new MsgProvider_10001());
            MsgProviders.Add(ClientMsgCode.ServerToClient_SetPeriod, new MsgProvider_10002());
            MsgProviders.Add(ClientMsgCode.ServerToClient_SetPeriod2, new MsgProvider_10003());
            MsgProviders.Add(ClientMsgCode.Response_OK, new MsgProvider_ResponseOK());
        }

        public static ClientToServerMsgBase Provide(BaseSocketConnection e, byte[] receiveData)
        {
            try
            {
                lock (_buffer)
                {
                    if (receiveData != null && receiveData.Length > 0 && receiveData[receiveData.Length - 1] == 0x0A)
                    {
                        _buffer.AddRange(receiveData);
                        _buffer.RemoveAt(_buffer.Count - 1);

                        string msgContent = Encoding.UTF8.GetString(_buffer.ToArray());
                        _buffer.Clear();
                        return Provide(msgContent);
                    }
                    else
                    {
                        _buffer.AddRange(receiveData);
                    }
                }
            }
            catch (Exception ex)
            {
                AppContext.Log(ex.ToString());
            }
            return null;
        }


        public static ClientToServerMsgBase Provide(string str)
        {
            if (str != null)
            {
                ClientMsgEntity msg = ClientMsgEntity.DeSerialize(str);
                if (msg != null)
                {


                    MessageProviderBase provider = null;
                    if (MsgProviders.TryGetValue(msg.Command, out provider))
                    {
                        provider.Provide(msg);
                    }
                    if (!_UnResponseCode.Contains(msg.Command))
                    {
                        return new ClientToServerMsgBase
                        {
                            Command = ClientMsgCode.Response_OK,
                            Identity = msg.Identity,
                            Paramters = new Dictionary<string, object>(0)
                        };


                    }

                }
            }
            return null;
        }
    }
}
