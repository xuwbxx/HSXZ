using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrainingsApp.Helper;
using FD_ScanSystem.WebModel.Enums;

namespace FD_ScanSystem.WebModel.View
{
    public class DeviceView
    {
        public int ID { get; set; }

        public string DeviceName { get; set; }

        public int NodeId { get; set; }

        public string NodeName { get; set; }

        public string DeviceIdentity { get; set; }

        public int DeviceProtocol { get; set; }

        public string CommunicationName { get; set; }

        public string ProtocolName
        {
            get
            {
                return EnumHelper.GetEnumValueDescription<DeviceProtocolEnum>((DeviceProtocolEnum)DeviceProtocol);
            }
        }

        public DateTime CreateDate { get; set; }

    }
}
