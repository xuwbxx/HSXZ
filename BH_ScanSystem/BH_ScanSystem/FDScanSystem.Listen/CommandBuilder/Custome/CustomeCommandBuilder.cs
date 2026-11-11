using FDScanSystem.Listen.Analysis;
using FDScanSystem.Listen.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDScanSystem.Listen.CommandBuilder.Custome
{
    public class CustomeCommandBuilder : ICommandBuilder
    {

        public CommandBase Build(int id, string identity, string command)
        {
            CommandBase cmd = new CustomeCommand(command);
            cmd.ConnectionKey = identity;
            cmd.DataProtocol = AnalysisProtocolType.Customer;
            cmd.Status = CommandStatus.New;
            cmd.CommandText = command;
            cmd.ID = id;
           // cmd.Identity = Guid.NewGuid().ToString();
            return cmd;
        }
    }
}

