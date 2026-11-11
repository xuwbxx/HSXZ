using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDScanSystem.Client.Logic.ThreadManager {
    public interface IThead {

        void Start();

        void Stop();

        string ThreadName { get; }
    }
}
