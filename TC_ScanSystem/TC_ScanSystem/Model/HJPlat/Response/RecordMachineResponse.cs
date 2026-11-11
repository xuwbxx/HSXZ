using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Response
{
    public class RecordMachineResponse
    {
        public string Idx { set; get; }
        public string MachineCode { set; get; }
        public string MachineName { set; get; }
        public string Model { set; get; }
        public string License { set; get; }
        public string Operator { set; get; }
        public string Charger { set; get; }
        public string Phone { set; get; }
        public string CaveName { set; get; }
        public string Unit { set; get; }

        public string Vender { set; get; }

        public string EntryDate { set; get; }
        public string DepartureDate { set; get; }
    }
}
