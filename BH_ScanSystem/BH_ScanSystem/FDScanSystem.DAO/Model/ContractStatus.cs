using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDScanSystem.DAO.Model {
    public class ContractStatus {
        /// <summary>
        /// 已签订/待返
        /// </summary>
        public const int Signed = 1;


        /// <summary>
        /// 生产脱节
        /// </summary>
        public const int BuildError = 2;

        /// <summary>
        /// 生产滞后
        /// </summary>
        public const int BuildLate = 3;


        /// <summary>
        /// 收款脱节
        /// </summary>
        public const int PayError = 4;


    }
}
