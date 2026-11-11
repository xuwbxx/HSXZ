using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using FDScanSystem.Helper;
using HosterServices;

namespace FDScanSystem.Server
{
    public class AppContext
    {

        static TextWriter LOG_WRITER = null;

        readonly static bool _IsDebug;
        readonly static bool _IsLog;
        static AppContext()
        {
            _IsDebug = (ConfigurationManager.AppSettings["IsDebug"] ?? "").Equals("true", StringComparison.CurrentCultureIgnoreCase);
            _IsLog = (ConfigurationManager.AppSettings["IsLog"] ?? "false").Equals("true", StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool IsDebug
        {
            get
            {
                return _IsDebug;
            }
        }

        public static void Initialize(TextWriter writer)
        {

            LOG_WRITER = writer;
            //DefaultPortProvider.Register(new VersionDataReceiver());
            //DefaultPortProvider.Register(new OKDataReceiver());
            //DefaultPortProvider.Register(new DetailDataReceiver());
            //DefaultPortProvider.Register(new WaitDataReceiver());
            //DefaultPortProvider.SetDefault(new DefaultDataReceiver());
        }

        public static void Log(string msg)
        {
            if (IsDebug)
            {
                if (_IsLog)
                    LogWriter.Info(msg);
                else
                    LOG_WRITER.WriteLine(string.Format("{0}--[INFO]: {1}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), msg));
            }

        }

        public static void Success(string msg)
        {
            if (IsDebug)
            {
                if (_IsLog)
                    LogWriter.Info(msg);
                else
                    LOG_WRITER.WriteLine(string.Format("{0}--[SUCCESS]: {1}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), msg));
            }
        }

        public static void Error(string msg)
        {
            // LOG_WRITER.WriteLine(string.Format("{0}--[ERROR]: {1}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), msg));
            LogWriter.Error(msg.ToString());
        }

        public static void Error(Exception ex)
        {
            //  LOG_WRITER.WriteLine(string.Format("{0}--[ERROR]: {1}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), ex.ToString()));
            LogWriter.Error(ex.ToString());
        }


        public static bool IsStartedProcess()
        {
            Process[] startedProcess = System.Diagnostics.Process.GetProcessesByName("FDScanSystem.Server");
            return (startedProcess != null && startedProcess.Length > 0 && startedProcess[0] != null && startedProcess.Count(u => u.Id != Process.GetCurrentProcess().Id) > 0);
        }
    }
}
