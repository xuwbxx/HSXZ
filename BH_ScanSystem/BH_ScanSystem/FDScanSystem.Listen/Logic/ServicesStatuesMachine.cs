using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using ZJFramework10.Extension;

namespace FDScanSystem.Listen.Logic {
    public class ServicesStatuesMachine : IDisposable {

        private const string StatusLogPath = "Status.st";
        private int _checkCount = 6000;
        private int _flagCount = 1;

        private void Writer() {
            try {
                using (FileStream stream = new FileStream(Application.StartupPath + "/" + StatusLogPath, FileMode.Create, FileAccess.Write)) {

                    using (StreamWriter writer = new StreamWriter(stream)) {
                        writer.WriteLine(_checkCount.ToString());
                        writer.WriteLine(_flagCount.ToString());
                    }
                }
            }
            catch {

            }
        }

        private void Read() {
            if (File.Exists(Application.StartupPath + "/" + StatusLogPath)) {
                try {
                    using (FileStream stream = new FileStream(Application.StartupPath + "/" + StatusLogPath, FileMode.Open, FileAccess.Read)) {
                        using (StreamReader reader = new StreamReader(stream)) {
                            if (reader.Peek() != -1)
                                _checkCount = reader.ReadLine().AsInt();
                            if (reader.Peek() != -1)
                                _flagCount = reader.ReadLine().AsInt();
                        }
                    }
                }
                catch {

                }
            }
        }

        public bool IsOutofMaxRange() {

            return (_flagCount % _checkCount) == 0;
        }


        public void Increment() {
            System.Threading.Interlocked.Increment(ref _flagCount);
        }

        public void Initialize() {
            Read();
        }

        public void Dispose() {

            Writer();
        }
    }
}
