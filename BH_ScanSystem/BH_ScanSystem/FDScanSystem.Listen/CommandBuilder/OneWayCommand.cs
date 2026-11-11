using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDScanSystem.Listen.Logic {
    /// <summary>
    /// 不需要等待的命令
    /// 
    /// </summary>
    public class OneWayCommand : CommandBase {
        public override int Type {
            get { return 1; }
        }
    }
}
