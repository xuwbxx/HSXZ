using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Response
{
    public class RepaireTaskResponse
    {
        public int InspectionId { get; set; }
        public string SpecialCode { get; set; }
        public string title { get; set; }

        public int SpecialID { get; set; }

        public string SpecialName { get; set; }

        public string ProjectName { get; set; }

        public string OrgName { get; set; }

        public string InspectorName { get; set; }

        public string ThreatTypeName { get; set; }

        public int ThreatType { get; set; }

        public int ThreatLevel { get; set; }

        public int Status { get; set; }

        public DateTime InspectionDate { get; set; }
        public string InsDate { get { return InspectionDate.ToString("yyyy-MM-dd hh:mm:ss"); } }

        public DateTime DeadLine { get; set; }
        public string StructorName { get; set; }

        public string CaveName { get; set; }
        public int InspectType { get; set; }
    }
    public enum RepairedStatus
    {
        /// <summary>
        /// 待整改
        /// </summary>
        ToBeRepaired = 0,


        /// <summary>
        /// 待审核
        /// </summary>
        ToBeCheck = 1,


        /// <summary>
        /// 待责任人审核
        /// </summary>
        ToBeLeaderCheck = 4,


        /// <summary>
        /// 整改通过
        /// </summary>
        Repaired = 2,



        /// <summary>
        /// 需要重新整改
        /// </summary>
        Rejected = 3
    }




    public enum InspectionStatus
    {
        /// <summary>
        /// 创建
        /// </summary>
        Normal = 0,


        /// <summary>
        /// 待确认
        /// </summary>
        ToBeConfirm = 1,


        /// <summary>
        /// 整改中
        /// </summary>
        Repairing = 2,


        /// <summary>
        /// 一结束
        /// </summary>
        Finished = 3


    }




    public enum SecurityThreatStatus
    {
        /// <summary>
        /// 创建，暂存
        /// </summary>
        Created = 0,
        /// <summary>
        /// 正常检查内容，无需整改
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 待整改
        /// </summary>
        ToBeRepaired = 2,

        /// <summary>
        /// 待审核
        /// </summary>
        ToBeChecked = 3,
        /// <summary>
        /// 已审核通过
        /// </summary>
        Checked = 4
    }
}
