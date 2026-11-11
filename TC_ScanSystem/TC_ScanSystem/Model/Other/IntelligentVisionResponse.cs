using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Other
{
    public class IntelligentVisionModel
    {
        public string DBAlarmID { set; get; }

        public string AlarmID { set; get; }

        public string ScannerAlarmID { set; get; }

        public string IntellexAlarmID { set; get; }

        public string ScannerCustomerNumber { set; get; }

        public string ScannerFeedNumber { set; get; }

        public string ScannerIP { set; get; }

        public string VideoSourceSystemID { set; get; }

        public string VideoSourceChannelID { set; get; }

        public string AlarmType { set; get; }

        public string ManagerStartTime { set; get; }

        public string ManagerAlarmTime { set; get; }

        public string ManagerEndTime { set; get; }

        public string VideoSourceAlarmStartTime { set; get; }

        public string VideoSourceAlarmTime { set; get; }

        public string VideoSourceAlarmEndTime { set; get; }

        public string JPBPath { set; get; }

        public string Thumbnail { set; get; }

        public string ThumbnailRows { set; get; }

        public string ThumbnailCols { set; get; }

        public string ThumbnailSize { set; get; }

        public string ThumbnailPath { set; get; }

        public string ScannerName { set; get; }

        public string spareInt1 { set; get; }

        public string spareInt2 { set; get; }

        public string spareInt3 { set; get; }

        public string spareStr32_1 { set; get; }

        public string spareStr32_2 { set; get; }

        public string spareStr64_1 { set; get; }

        public string spareStr64_2 { set; get; }

        public string ruleName { set; get; }

        public string AlarmLevelID { set; get; }

        public string UserID { set; get; }

        public string ackTime { set; get; }

        public string RuleNameStr { set; get; }

        public string PTZNumber { set; get; }

        public string SpeciesID { set; get; }

        public string DVUID { set; get; }

        public string CalibrationID { set; get; }
    }


    public class IntelligentVisionAlarmCount
    {
        public int ID { set; get; }
        
        public int InCount { set; get; }

        public int OutCount { set; get; }
    }
}
