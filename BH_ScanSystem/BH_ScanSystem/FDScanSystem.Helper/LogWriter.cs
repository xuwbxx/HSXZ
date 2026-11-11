using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Config;
using log4net;

namespace FDScanSystem.Helper {
    public class LogWriter {
        private static readonly ILog _log;

        static LogWriter() {
            XmlConfigurator.Configure();
            _log = LogManager.GetLogger(typeof(LogWriter));
        }

        /// <summary>
        /// 发送游戏服务异常日志
        /// </summary>
        /// <param name="ruid">玩家id 得不到传Guid.Empty</param>
        /// <param name="serverType">服务类型（GateWay.ServerType）</param>
        /// <param name="ex">异常</param>
        public static void Error(Guid ruid, int serverType, Exception ex) {
            _log.Error(ruid + "," + serverType + "," + ex);
            //Helper.Instance.SendGameServerExceptionLog(ruid, serverType, "", "", "", ex);
        }


        /// <param name="content">内容</param>
        public static void Error(string content) {
            _log.Error(content);
            //   Helper.Instance.SendGameServerExceptionLog(ruid, serverType, "", "", content, null);
        }


        /// <summary>
        /// 发送游戏服务异常日志
        /// </summary>
        /// <param name="ruid">玩家id 得不到传Guid.Empty</param>
        /// <param name="serverType">服务类型（GateWay.ServerType）</param>
        /// <param name="showMessage">显示信息</param>
        /// <param name="ex">异常</param>
        public static void Error(Guid ruid, int serverType, string showMessage, Exception ex) {
            _log.Error(ruid + "," + serverType + "," + showMessage + "," + ex);
            //  Helper.Instance.SendGameServerExceptionLog(ruid, serverType, "", "", showMessage, ex);
        }

        /// <summary>
        /// 发送游戏服务异常日志
        /// </summary>
        /// <param name="ruid">玩家id 得不到传Guid.Empty</param>
        /// <param name="serverType">服务类型（GateWay.ServerType）</param>
        /// <param name="className">类名</param>
        /// <param name="methodName">方法名</param>
        /// <param name="showMessage">显示信息</param>
        /// <param name="ex">异常</param>
        public static void Error(Guid ruid, int serverType, string className, string methodName, string showMessage, Exception ex) {
            _log.Error(ruid + "," + serverType + "," + showMessage + "," + ex);
            //   Helper.Instance.SendGameServerExceptionLog(ruid, serverType, className, methodName, showMessage, ex);
        }


        /// <summary>
        /// 发送游戏服务调试日志
        /// </summary>
        /// <param name="ruid">玩家id 得不到传Guid.Empty</param>
        /// <param name="serverType">服务类型（GateWay.ServerType）</param>
        /// <param name="className">类名</param>
        /// <param name="methodName">方法名</param>
        /// <param name="content">内容</param>
        public static void Debug(Guid ruid, int serverType, string className, string methodName, string content) {
            _log.Debug(ruid + "," + serverType + "," + content);
            // Helper.Instance.SendGameServerDebugLog(ruid, serverType, className, methodName, content);
        }


        /// <summary>
        /// 发送游戏服务调试日志
        /// </summary>
        /// <param name="ruid">玩家id 得不到传Guid.Empty</param>
        /// <param name="serverType">服务类型（GateWay.ServerType）</param>
        /// <param name="content">内容</param>
        public static void Debug(Guid ruid, int serverType, string content) {
            _log.Debug(ruid + "," + serverType + "," + content);
            //  Helper.Instance.SendGameServerDebugLog(ruid, serverType, "", "", content);
        }

        /// <summary>

        /// </summary>

        /// <param name="content">内容</param>
        public static void Info(string content) {
            _log.Info(content);
            // Helper.Instance.SendGameServerDebugLog(ruid, serverType, "", "", content);
        }
    }
}
