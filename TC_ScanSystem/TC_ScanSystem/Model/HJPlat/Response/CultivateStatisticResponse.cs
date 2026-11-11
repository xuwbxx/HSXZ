using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.HJPlat.Response
{
    public class CultivateStatisticResponse
    {
        /// <summary>
        /// 现有总人数
        /// </summary>
        public int PersonCount { get; set; }

        /// <summary>
        /// 历史总人数
        /// </summary>
        public int HisCount { get; set; }

        /// <summary>
        /// 培训人数
        /// </summary>
        public int TrainCount { get; set; }

        /// <summary>
        /// 培训率
        /// </summary>
        public string PrcentOfTrain { get; set; }

        /// <summary>
        /// 培训人次
        /// </summary>
        public int PersonTime { get; set; }

        /// <summary>
        /// 累计学时
        /// </summary>
        public double Period { get; set; }

        /// <summary>
        /// 人均学时
        /// </summary>
        public string AverageOfPeriod { get; set; }

        /// <summary>
        /// 考试人次
        /// </summary>
        public int ExamCount { get; set; }

        /// <summary>
        /// 合格人次
        /// </summary>
        public int PassedCount { get; set; }

        /// <summary>
        /// 考试合格率
        /// </summary>
        public string PrcentOfPassed { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string DepartName { get; set; }


    }
}