using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FD_ScanSystem.WebModel;
using FD_ScanSystem.WebModel.Enums;
using FDScanSystem.DAO;
using FDScanSystem.DAO.Model;
using FDScanSystem.Protocol;
using FDScanSystem.Protocol.ScanConfig;
using OfficeShop.DAL;
using OfficeShop.Model;
using OfficeShop.WebModel;
using ZJFramework10;
using ZJFramework10.Extension;


namespace FDScanSystem.Web.DataInfo
{
    public partial class PointSelect : AdminPageBase
    {

        protected string SelectedIds { get; set; }

        protected int ScanNodeId
        {
            get
            {
                return ZJFramework10.Utility.QueryStringManager.GetValue<int>("node");
            }
        }

        protected int ScanTypeId
        {
            get
            {
                return ZJFramework10.Utility.QueryStringManager.GetValue<int>("type");
            }
        }
        protected string Point
        {
            get
            {
                return ZJFramework10.Utility.QueryStringManager.GetValue<string>("point");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var db = DataContextFactory.Create();
            var point = db.FD_ScanPoint.Where(u => u.ScanTypeId == ScanTypeId && u.NodeId == ScanNodeId).OrderBy(u => u.PointName).ToList();
            mainGrid.DataSource = point;
            mainGrid.DataBind();


         //   SelectedIds = ZJFramework10.Utility.FormManager.GetValue<string>("selectedHistory", "");


            //if (!string.IsNullOrEmpty(SelectedIds))
            //{
            //    List<string> ids = SelectedIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            //    var goodsDataList = db.FD_ScanPoint.Where(u => ids.Contains(u.ID.ToString())).Select(u => new
            //    {
            //        u.ID,
            //        u.PointName,
            //        u.Remark
            //    }).ToList();

            //    rptDataSource.DataSource = goodsDataList;
            //    rptDataSource.DataBind();

            //    if (goodsDataList.Count <= 0)
            //    {
            //        rptDataSource.Visible = false;
            //    }
            //    else
            //    {
            //        rptDataSource.Visible = true;
            //    }

                //chkPoint.DataSource = point;
                //chkPoint.DataTextField = "PointName";
                //chkPoint.DataValueField = "";

            }
        

        protected void mainGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomCallbackEventArgs e)
        {

        }

        public override string PermissionCode
        {
            get { return PermissionDict.DataInfo_StaticCalChart; }
        }
    }
}