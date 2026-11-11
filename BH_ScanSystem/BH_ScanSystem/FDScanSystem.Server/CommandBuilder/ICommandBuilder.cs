using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace FDScanSystem.Server.Job {
    public interface ICommandBuilder {
        CommandBase Build(int id, string identity, string command);
    }
}
