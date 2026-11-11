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
using OfficeShop.DAL;
using OfficeShop.Model;
using OfficeShop.WebModel;
using ZJFramework10.Extension;

namespace FDScanSystem.Web.DataInfo
{
    public partial class FD_LayerImage : AdminPageBase
    {
        private string ltstr = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            UserContext user = PassPort.CurrentUser;
            var adminContext = user as AdminUserContext;
            if (adminContext != null)
            {
                var url = adminContext.FunctionRights.Where(u => !string.IsNullOrEmpty(u.FunctionUrl)).FirstOrDefault();
                if (url != null)
                {
                    ltUrl.Text = "<a href=\"" + url.FunctionUrl + "\">首页</a>";
                }
                else
                {
                    Response.Redirect("/login.aspx");
                }
            }

            if (!IsPostBack)
            {
                var db = DataContextFactory.Create();
            //    img_Layer.Visible = false;
                drpNode.DataSource = db.FD_ScanNode.Where(u => u.NodeParentId == 0).ToList();
                drpNode.TextField = "NodeName";
                drpNode.ValueField = "ID";
                drpNode.DataBind();
                // drpNode.Items.Insert(0, new DevExpress.Web.ASPxEditors.ListEditItem("请选择", "0"));

                drpNode.SelectedIndex = 0;


                drpScanType.DataSource = db.FD_Dict.Where(u => u.DictType == DictTypeEnum.SCANTYPE).ToList();
                drpScanType.TextField = "DictValue";
                drpScanType.ValueField = "DictID";
                drpScanType.DataBind();
                //drpScanType.Items.Insert(0, new DevExpress.Web.ASPxEditors.ListEditItem("请选择", "0"));

                drpScanType.SelectedIndex = 0;

                drpLayer.DataSource = db.FD_ScanLayer.Where(u => u.NodeId == drpNode.Value.AsInt()).ToList();
                drpLayer.TextField = "LayerName";
                drpLayer.ValueField = "ID";
                drpLayer.DataBind();
                drpLayer.SelectedIndex = 0;
                PositionShow();
            }

        }

        protected void drpLayer_Select(object sender, EventArgs e)
        {
            var db = DataContextFactory.Create();

            drpLayer.DataSource = db.FD_ScanLayer.Where(u => u.NodeId == drpNode.Value.AsInt()).ToList();
            drpLayer.TextField = "LayerName";
            drpLayer.ValueField = "ID";
            drpLayer.DataBind();
           // drpLayer.Items.Insert(0, new DevExpress.Web.ASPxEditors.ListEditItem("请选择", "0"));

            drpLayer.SelectedIndex = 0;
        }

        protected void btn_Choose_Click(object sender, EventArgs e)
        {
            PositionShow();
        }

        private void PositionShow()
        {
            var db = DataContextFactory.Create();
            img_Layer.ImageUrl = db.FD_ScanLayer.SingleOrDefault(d => d.ID == drpLayer.Value.AsInt()).ImageLocation;
            ltstr = "<div style=\"position:absolute; float:left; z-index:9999; margin-top:{0}px; margin-left:{1}px\">\n";
            ltstr += "<a href=\"{4}\" title=\"{2}\" ><img src=\"{3}\" Height=25px Width=25px /></a>\n</div>\n";
            int typeId = drpScanType.Value.AsInt();
            int layerId = drpLayer.Value.AsInt();
            List<FD_ScanPoint> temp_pos = db.FD_ScanPoint.Where(d => d.ScanTypeId == typeId && d.LayerId == layerId).ToList();
            if (temp_pos.Count > 0)
            {
                for (int i = 0; i < temp_pos.Count; i++)
                {
                    //img_Layer.Visible = true;
                    //string url = WebHelper.Root + "MK_DataValue_List.aspx?SNP=" + temp_pos[i].ID;
                    string url = "/DataInfo/StaticCalChart.aspx?pid=" + temp_pos[i].ID;
                    string title = drpScanType.Text + " ";
                    int ID = temp_pos[i].ID;
                    //  List<MK_Point> temp_pt = DB.MK_Point.Where(d => d.PositionId == ID).ToList();

                    List<FD_DataValue> temp_value = db.FD_DataValue.Where(d => d.PointId == ID).ToList();
                    string img = "/Upload/point.png";
                    ltDiv.Text += string.Format(ltstr, temp_pos[i].MapY, temp_pos[i].MapX, title, img, url);

                }
            }
            else
            {
                Response.Redirect("/DataInfo/StaticCalChart.aspx");
            }
        }



        public override string PermissionCode
        {
            get { return PermissionDict.DataInfo_StaticCalChart; }
        }
    }
}