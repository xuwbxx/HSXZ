using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Training
{
    public class SafetyKitsResponse
    {
        public SafetyKitsResponse()
        {
            IsPass = false;
            Certificate = string.Empty;
            CertificateNo = string.Empty;
            QRCode = string.Empty;
            Photo = string.Empty;
        }

        public string PersonId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string PersonName { get; set; }
        /// <summary>
        /// 是否通过
        /// </summary>
        public bool IsPass { get; set; }
        /// <summary>
        /// 得分
        /// </summary>
        public string Score { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string IdentifyID { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        public string ReadCardId { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// 籍贯
        /// </summary>
        public string Native { get; set; }
        /// <summary>
        /// 岗位
        /// </summary>
        public string Station { get; set; }
        /// <summary>
        /// 工种
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 证书
        /// </summary>
        public string Certificate { get; set; }
        /// <summary>
        /// 证书编号
        /// </summary>
        public string CertificateNo { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName { get; set; }
        /// <summary>
        /// 住址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 紧急联系人
        /// </summary>
        public string EmergencyContacts { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string EmergencyContactsTel { get; set; }
        /// <summary>
        /// 培训类型
        /// </summary>
        public string TrainType { get; set; }
        /// <summary>
        /// 培训时间
        /// </summary>
        public string TrainTime { get; set; }
        /// <summary>
        /// 培训单位
        /// </summary>
        public string TrainDepart { get; set; }
        /// <summary>
        /// 是否合格
        /// </summary>
        public string ISPass { get; set; }
        /// <summary>
        /// 培训内容
        /// </summary>
        public string TrainContent { get; set; }
        /// <summary>
        /// 照片
        /// </summary>
        public string Photo { get; set; }
        /// <summary>
        /// 二维码
        /// </summary>
        public string QRCode { get; set; }
        /// <summary>
        /// 答题结果
        /// </summary>
        public string Answer { get; set; }
        /// <summary>
        /// 题目集合
        /// </summary>
        public string[] ExamContent { get; set; }
    }
}
