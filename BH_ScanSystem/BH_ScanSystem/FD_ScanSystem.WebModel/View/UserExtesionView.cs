using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrainingsApp.Helper;
using FDScanSystem.DAO.Model;

namespace FD_ScanSystem.WebModel.View {
    public class UserExtensionView {

        public int Idx { get; set; }

        public int Depart { get; set; }

        public int AuthLevel { get; set; }

        public decimal Price { get; set; }

        public string UserName { get; set; }

        public string RealName { get; set; }


        public string DepartName {
            get {
                if (Depart == 0) {
                    return "尚未未选择";
                }
                else {
                    return EnumHelper.GetEnumValueDescription<DepartConst>((DepartConst)Depart);
                }
            }
        }

        public string AuthLevelName {
            get {
                if (AuthLevel == 0) {
                    return "尚未未选择";
                }
                else {
                    return EnumHelper.GetEnumValueDescription<AuthLevels>((AuthLevels)AuthLevel);
                }
            }
        }
    }
}
