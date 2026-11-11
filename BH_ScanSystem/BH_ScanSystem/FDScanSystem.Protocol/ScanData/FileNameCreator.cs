using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace FDScanSystem.Protocol.ScanData
{
    public class FileNameCreator
    {
        private readonly string _RootDir;

        static FileNameCreator _Instance;
        static object _Lock = new object();
        protected FileNameCreator()
        {
            _RootDir = ConfigurationManager.AppSettings["ScanDataDir"];
        }

        public static FileNameCreator GetInstance()
        {
            if (_Instance == null)
            {
                lock (_Lock)
                {
                    if (_Instance == null)
                    {
                        _Instance = new FileNameCreator();
                    }
                }
            }
            return _Instance;
        }


        public string CreateFile(int deviceId, long filetime)
        {
            string fileDir = Path.Combine(_RootDir, deviceId.ToString());
            if (!System.IO.Directory.Exists(fileDir))
            {
                Directory.CreateDirectory(fileDir);
            }
            DateTime d = DateTime.FromFileTime(filetime);

            fileDir = Path.Combine(fileDir, d.ToString("yyMMddHH"));
            if (!System.IO.Directory.Exists(fileDir))
            {
                Directory.CreateDirectory(fileDir);
            }
            string fileName = Path.Combine(fileDir, d.ToString("yyMMddhhmmssfff") + ".dat");
            if (!File.Exists(fileName))
            {
                using (FileStream s = File.Create(fileName)) { }
            }
            return fileName;
        }

    }
}
