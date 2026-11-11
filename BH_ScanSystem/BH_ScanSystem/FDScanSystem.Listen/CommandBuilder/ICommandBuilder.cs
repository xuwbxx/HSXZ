using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace FDScanSystem.Listen.Logic {
    public interface ICommandBuilder {
        CommandBase Build(int id, string identity, string command);
    }
}
