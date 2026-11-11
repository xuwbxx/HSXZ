using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool
{
    public class LogUtils
    {
        private static object o = new object();

        private static LogUtils _Instance = null;

        public static LogUtils Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (o)
                    {
                        _Instance = new LogUtils();
                    }
                }
                return _Instance;
            }
        }

        private string path = AppDomain.CurrentDomain.BaseDirectory + @"Logtext.txt";

        public void LogText(string str)
        {
            if (!File.Exists(path))
            {
                using (File.CreateText(path)) { }
            }

            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ": " + str);
            }

        }


        public static string Read(string path)
        {
            string txt = string.Empty;
            using (StreamReader sr = new StreamReader(path, Encoding.Default))
            {
                txt = sr.ReadToEnd();
            }
            return txt;
        }
    }
}
