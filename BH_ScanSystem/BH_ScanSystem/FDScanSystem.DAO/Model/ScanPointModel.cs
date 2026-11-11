using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDScanSystem.DAO.Model
{
    public class ScanPointModel
    {
        public int ID { get; set; }

        public int NodeId { get; set; }

        public int DeviceId { get; set; }
        public string PointName { set; get; }

        public int ScanTypeId { get; set; }

        public int PositionId { set; get; }

        public string ScanTypeName { set; get; }

        public string Channel1 { get; set; }

        public string Channel2 { get; set; }

        public string MapDataName { set; get; }

        public string MapDataName2 { set; get; }

        public double AlertMin { get; set; }

        public double AlertMin2 { get; set; }

        public double AlertMax { get; set; }

        public double AlertMax2 { get; set; }

        public string MapX { get; set; }

        public string MapY { get; set; }

        public string Unit { get; set; }

        public string CreateDate { get; set; }

        public string Other1 { get; set; }

        public string Other2 { get; set; }

        public string Other3 { get; set; }

        public string Other4 { get; set; }

        public double? para1 { set; get; }

        public double? para2 { set; get; }

        public double? para3 { set; get; }

        public double? para4 { set; get; }

        public double? para5 { set; get; }

        public double? para6 { set; get; }

        public double? MaxNum { set; get; }

        public double? MaxNum2 { set; get; }

        public double? MinNum { set; get; }

        public double? MinNum2 { set; get; }

    }

    //public class DataValueModel
    //{
    //    public int ID { set; get; }

    //    public int PointId { set; get; }

    //    public int DataId { set; get; }

    //    public double ScanValue { set; get; }

    //    public double ScanValue2 { set; get; }

    //    public double ScanValue3 { set; get; }

    //    public double ScanValue4 { set; get; }
    //}
}
