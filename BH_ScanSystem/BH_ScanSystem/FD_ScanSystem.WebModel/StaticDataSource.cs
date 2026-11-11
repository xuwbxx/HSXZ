using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrainingsApp.Helper;
using FDScanSystem.DAO.Model;
using OfficeShop.WebModel;
using FDScanSystem.DAO;

namespace FD_ScanSystem.WebModel
{
    public class StaticDataSource
    {
        public static List<EnumItem> ListDepart()
        {
            return EnumHelper.GetEnumItems(typeof(DepartConst));
        }

        public static List<EnumItem> ListTeamLeader()
        {
            var db = DataContextFactory.Create();
            int did = 0;// PassPort.CurrentUser.DepartId;
            if (did < 5)
            {
                return (from A in db.FD_User
                        //join B in db.FD_User_Extension
                        //on A.ID equals B.UserId
                        //where A.DepartId == did
                        select new EnumItem
                        {
                            Key = A.ID,
                            Value = A.RealName
                        }).ToList();
            }
            else
            {
                return (from A in db.FD_User
                        //join B in db.FD_User_Extension
                        //on A.ID equals B.UserId
                        //where A.IsProjectLeader == true
                        select new EnumItem
                        {
                            Key = A.ID,
                            Value = A.RealName
                        }).ToList();
            }
        }
    }
}
