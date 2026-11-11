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

namespace FDScanSystem.Web.Setting
{
    public partial class FD_Point_List : AdminPageBase
    {
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
                drpNode.DataSource = db.FD_ScanNode.Where(u => u.NodeParentId == 0).ToList();
                drpNode.TextField = "NodeName";
                drpNode.ValueField = "ID";
                drpNode.DataBind();
                if (drpNode.Items.Count > 0)
                {
                    drpNode.SelectedIndex = 0;
                }

                drpScanType.DataSource = db.FD_Dict.Where(u => u.DictType == DictTypeEnum.SCANTYPE).ToList();
                drpScanType.TextField = "DictValue";
                drpScanType.ValueField = "DictID";
                drpScanType.DataBind();
                if (drpScanType.Items.Count > 0)
                {
                    drpScanType.SelectedIndex = 0;
                }

            }
            BindData();
        }

        protected void btnRefresh_CLick(object sender, EventArgs e)
        { }

        protected void btn_Choose_Click(object sender, EventArgs e)
        { }

        private void BindData()
        {
            var db = DataContextFactory.Create();



            var list = (from a in db.FD_ScanPoint
                        join b in db.FD_ScanNode
                        on a.NodeId equals b.ID
                        join c in db.FD_Dict.Where(u => u.DictType == DictTypeEnum.SCANTYPE)
                        on a.ScanTypeId equals c.DictID
                        join d in db.FD_Device
                        on a.DeviceId equals d.ID
                        join e in db.FD_CommunicationDevice
                        on d.CommunicationDeviceId equals e.ID
                        select new PointView
                        {
                            pName = a.Remark,
                            NodeId = b.ID,
                            ScanTypeId = c.DictID,
                            PointName = a.PointName,
                            ID = a.ID,
                            ScanTypeName = c.DictValue,
                            LayerName = b.NodeName,
                            DeviceName = d.DeviceName,
                            CommunicationName = e.DeviceName,
                            CreateDate = a.CreateDate

                        }).ToList();
            if (drpNode.Items.Count > 0)
            {
                list = list.Where(u => u.NodeId == drpNode.Value.AsInt()).ToList();
            }
            if (drpScanType.Items.Count > 0)
            {
                list = list.Where(u => u.ScanTypeId == drpScanType.Value.AsInt()).ToList();
            }
            aspGrid.DataSource = list.OrderBy(u => u.PointName);
            aspGrid.DataBind();
        }

        protected void aspGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            int key = e.Keys[0].AsInt();
            if (key > 0)
            {
                var db = DataContextFactory.Create();
                var cinfo = db.FD_ScanPoint.FirstOrDefault(u => u.ID == key);
                if (cinfo != null)
                {
                    db.FD_ScanPoint.DeleteOnSubmit(cinfo);
                    db.SubmitChanges();
                }
            }
        }

        protected void aspGrid_RowDeleted(object sender, DevExpress.Web.Data.ASPxDataDeletedEventArgs e)
        {
            e.ExceptionHandled = true;
            BindData();
        }

        public override string PermissionCode
        {
            get { return PermissionDict.F_ScanPoint; }
        }

       
    }
    public class PointView
    {
        public int ID { get; set; }
        public string PointName { get; set; }
        public string pName { get; set; }               //传感器编号
        public string PositionName { get; set; }
        public double Height { get; set; }              //标高
        public string DeviceName { get; set; }
        public string CommunicationName { get; set; }   //通讯设备名称
        public string LayerName { get; set; }           //监测区域名称
        public DateTime CreateDate { get; set; }
        public string ProjectName { get; set; }
        public string ScanTypeName { get; set; }
        public int ScanTypeId { set; get; }
        public int NodeId { set; get; }
        public int PositionId { get; set; }
        public int DeviceId { get; set; }               //监测设备ID
        public int CommunicaionId { get; set; }         //通讯设备ID
    }
}