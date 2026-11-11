using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDScanSystem.DAO.Model
{
    public class DataValueModel
    {
        public int PointId { get; set; }

        public double ScanValue { get; set; }

        public double ScanValue2 { get; set; }

        public double ScanValue3 { get; set; }

        public double ScanValue4 { get; set; }

        public DateTime CreateDate { get; set; }

        public int Status { get; set; }
    }
}
