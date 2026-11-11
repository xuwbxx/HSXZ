using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FDScanSystem.DAO;
using FDScanSystem.DAO.Model;
using OfficeShop.WebModel;
using OfficeShop.DAL;
using OfficeShop.WebModel.Storage;
using OfficeShop.Model;
using FD_ScanSystem.WebModel;

namespace FDScanSystem.Web
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (this.Page is AdminPageBase)
            //{
            //    if ((this.Page as AdminPageBase).PermissionCode.Equals(PermissionDict.F_Vote_Group_List, StringComparison.CurrentCultureIgnoreCase))
            //    {
            //        UC_Left2.Visible = true;
            //    }
            //    else
            //    {
            //        string url = Request.RawUrl.ToLower();
            //        //if ((url.Contains("task") || url.Contains("user") || url.Contains("role") || url.Contains("function") || url.Contains("company")) && !url.Contains("review") && !url.Contains("vote") && !url.Contains("taskgroup"))
            //        //{
            //        //    UC_Left2.Visible = true;
            //        //}
            //        //else
            //        //{
            //        //    UC_Left1.Visible = true;
            //        //}
            //    }
            //}

            if (PassPort.CurrentUser != null)
            {
                var adminContext = PassPort.CurrentUser;
                // var User = db.Vt_User.SingleOrDefault(u => u.ID == adminContext.UserID);
                if (adminContext != null)
                {
                    var db = DataContextFactory.Create();



                    //var model = db.Vt_Company.SingleOrDefault(u => u.ID == adminContext.CompanyId);
                    //if (model != null)
                    //{
                    //    adminContext.CompanyName = model.CompanyName;
                    //}

                    //ltUser.Text = string.Format("{0} ( {1}{2} )", adminContext.RealName, adminContext.CompanyName, adminContext.Professional);
                }
            }
        }
    }
}
