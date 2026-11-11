using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FDScanSystem.Server.Job;
using FDScanSystem.Server.Analysis;

namespace FDScanSystem.Server.CommandBuilder.MCU {
    public class MCUCommandBuilder : ICommandBuilder {

        public CommandBase Build(int id, string identity, string command) {
            CommandBase cmd = null;
            if (command.IndexOf("info?", StringComparison.CurrentCultureIgnoreCase) > 0) {
                cmd = new OneWayCommand();
                cmd.CommandText = command;
                cmd.DataProtocol = AnalysisProtocolType.MCU;
                cmd.Identity = identity;
                cmd.Status = CommandStatus.InitializeConnection;
                cmd.ID = id;
                return cmd;
            }
            else if (command.IndexOf("begin", StringComparison.CurrentCultureIgnoreCase) >= 0) {
                cmd = new TwoWayCommand();
                cmd.CommandText = command;
                cmd.DataProtocol = AnalysisProtocolType.MCU;
                cmd.Identity = identity;
                cmd.Status = CommandStatus.New;
                cmd.ID = id;
            }
            else if (command.IndexOf("read", StringComparison.CurrentCultureIgnoreCase) >= 0) {
                cmd = new TwoWayCommand();
                cmd.CommandText = command;
                cmd.DataProtocol = AnalysisProtocolType.MCU;
                cmd.Identity = identity;
                cmd.Status = CommandStatus.New;
                cmd.ID = id;
            }
            else {
                cmd = new OneWayCommand();
                cmd.CommandText = command;
                cmd.DataProtocol = AnalysisProtocolType.MCU;
                cmd.Identity = identity;
                cmd.Status = CommandStatus.New;
                cmd.ID = id;
            }
            return cmd;
        }
    }
}
