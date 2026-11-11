using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FD_ScanSystem.WebModel;
using FD_ScanSystem.WebModel.Cache;
using FDScanSystem.DAO;
using FDScanSystem.Web.DataInfo.DataProcess;
using OfficeShop.WebModel;
using ZJFramework10.Extension;
namespace FDScanSystem.Web.DataInfo
{
    public partial class DynamicQueryDetail : AdminPageBase
    {

        protected int DeviceId
        {
            get
            {
                return ZJFramework10.Utility.QueryStringManager.GetValue<int>("device", 0);
            }
        }

        protected int DataValueId
        {
            get
            {
                return ZJFramework10.Utility.QueryStringManager.GetValue<int>("data", 0);
            }
        }


        public string CacheKey = "Dynamic";
        public string valu = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            var adminContext = PassPort.CurrentUser;
            if (adminContext != null)
            {
                //if (adminContext.UserName.ToLowerInvariant().Equals("admin"))
                //{
                //    url.Visible = true;
                //}
                //else
                //{
                //    url.Visible = false;
                //}
            }

            int t = 0;
            var db = DataContextFactory.Create();

            var deviceInfo = db.FD_Device.FirstOrDefault(u => u.ID == DeviceId);

            List<QueryDetailTreeEntity> result = db.FD_ScanPoint.Where(u => u.DeviceId == DeviceId).Select(y => new QueryDetailTreeEntity
            {
                NodeId = y.ID,
                ParentId = 0,
                AlertMax = y.AlertMax ?? 0,
                Channel1 = y.Channel1,
                NodeName = y.PointName,
                Remark = y.Remark,
                AlertMin = y.AlertMin ?? 0
            }).ToList();

            if (deviceInfo != null)
                result.Insert(0, new QueryDetailTreeEntity
                {
                    AlertMax = 0.0,
                    AlertMin = 0.0,
                    Channel1 = "无",
                    NodeId = 0,
                    NodeName = deviceInfo.DeviceName,
                    ParentId = -1,
                    Remark = "采集设备"
                });


            this.aspxTree.DataSource = result;
            this.aspxTree.DataBind();

            if (!IsPostBack)
            {
                //var deviceInfo = db.FD_Device.FirstOrDefault(u => u.ID == DeviceId);

                //List<QueryDetailTreeEntity> result = db.FD_ScanPoint.Where(u => u.DeviceId == DeviceId).Select(y => new QueryDetailTreeEntity
                // {
                //     NodeId = y.ID,
                //     ParentId = 0,
                //     AlertMax = y.AlertMax,
                //     Channel1 = y.Channel1,
                //     NodeName = y.PointName,
                //     Remark = y.Remark,
                //     AlertMin = y.AlertMin
                // }).ToList();

                //if (deviceInfo != null)
                //    result.Insert(0, new QueryDetailTreeEntity
                //    {
                //        AlertMax = 0.0,
                //        AlertMin = 0.0,
                //        Channel1 = "无",
                //        NodeId = 0,
                //        NodeName = deviceInfo.DeviceName,
                //        ParentId = -1,
                //        Remark = "采集设备"
                //    });


                //this.aspxTree.DataSource = result;
                //this.aspxTree.DataBind();

                //DevExpress.Web.ASPxPanel

                DataCreate();
                t = 1;
            }
            if (t == 0)
            {
                var dataValueEntity = db.FD_DataValue2.FirstOrDefault(u => u.ID == DataValueId);
                var detailEntity = CacheProxy.Get<FD_ScanPoint>(() => { return null; }, CacheKey);
                if (dataValueEntity != null)
                {
                    if (detailEntity != null)
                    {
                        this.ASPxGridView1.DataSource = DataTemplateFacade.CreateDataSource(dataValueEntity.ScanLocation, detailEntity.Channel1.AsInt());
                        //this.ASPxGridView1.DataSource = DataTemplateFacade.CreateDataSource(getFileName(dataValueEntity.ScanLocation), detailEntity.Channel1.AsInt());
                        // this.ASPxGridView2.DataSource = DataTemplateFacade.CreateDataSource3(getFileName(dataValueEntity.ScanLocation, "3"), detailEntity.Channel1.AsInt());

                        //this.aspGrid.DataBind();
                        this.ASPxGridView1.DataBind();
                        // this.ASPxGridView2.DataBind();

                        //var model = DataTemplateFacade.CreateDataSource2(getFileName(dataValueEntity.ScanLocation, "2"), detailEntity.Channel1.AsInt(), 2).FirstOrDefault();
                        //if (model != null)
                        //{
                        //    //valu = model.ScanData.ToString("F6");
                        //}
                        //double value = DataTemplateFacade.CreateDataSource(getFileName(dataValueEntity.ScanLocation), detailEntity.Channel1.AsInt());

                    }
                }
            }
        }

        protected void DataCreate()
        {
            var db = DataContextFactory.Create();

            if (this.aspxTree.FocusedNode == null)
            {
                //this.aspGrid.DataSource = null;
                this.ASPxGridView1.DataSource = null;
                // this.aspGrid.DataBind();
                return;
            }
            var detailEntity = aspxTree.FocusedNode.DataItem as QueryDetailTreeEntity;
            if (detailEntity.NodeId == 0)
            {
                // this.aspGrid.DataSource = null;
                this.ASPxGridView1.DataSource = null;
                this.ASPxGridView1.DataBind();
                //this.aspGrid.DataBind();
                return;
            }
            var dataValueEntity = db.FD_DataValue2.FirstOrDefault(u => u.ID == DataValueId);
            if (dataValueEntity != null)
            {

                this.ASPxGridView1.DataSource = DataTemplateFacade.CreateDataSource(dataValueEntity.ScanLocation, detailEntity.Channel1.AsInt());
                // this.ASPxGridView1.DataSource = DataTemplateFacade.CreateDataSource(getFileName(dataValueEntity.ScanLocation, "2"), detailEntity.Channel1.AsInt());
                // this.ASPxGridView2.DataSource = DataTemplateFacade.CreateDataSource3(getFileName(dataValueEntity.ScanLocation, "3"), detailEntity.Channel1.AsInt());

                //this.aspGrid.DataBind();
                this.ASPxGridView1.DataBind();
                // this.ASPxGridView2.DataBind();

                //var model = DataTemplateFacade.CreateDataSource2(getFileName(dataValueEntity.ScanLocation, "2"), detailEntity.Channel1.AsInt(), 2).FirstOrDefault();
                //if (model != null)
                //{
                //    // valu = model.ScanData.ToString("F6");
                //}
            }
        }



        public override string PermissionCode
        {
            get { return PermissionDict.DataInfo_Dynamic_Query; }
        }

        protected void btnXlsxExport1_Click(object sender, EventArgs e)
        {
            gridExport.WriteXlsToResponse();
        }

        protected void aspGrid1_CustomCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomCallbackEventArgs e)
        {
            var db = DataContextFactory.Create();
            int pointId = e.Parameters.AsInt();
            if (pointId == 0)
            {
                //this.aspGrid.DataSource = null;
                this.ASPxGridView1.DataSource = null;
                this.ASPxGridView1.DataBind();
                // this.aspGrid.DataBind();
                return;
            }
            var dataValueEntity = db.FD_DataValue2.FirstOrDefault(u => u.ID == DataValueId);
            var detailEntity = db.FD_ScanPoint.FirstOrDefault(u => u.ID == pointId);

            CacheProxy.Update(detailEntity, CacheKey);
            if (dataValueEntity != null)
            {

                this.ASPxGridView1.DataSource = DataTemplateFacade.CreateDataSource(dataValueEntity.ScanLocation, detailEntity.Channel1.AsInt());
                //this.ASPxGridView1.DataSource = DataTemplateFacade.CreateDataSource(getFileName(dataValueEntity.ScanLocation), detailEntity.Channel1.AsInt());
                //this.ASPxGridView2.DataSource = DataTemplateFacade.CreateDataSource3(getFileName(dataValueEntity.ScanLocation, "3"), detailEntity.Channel1.AsInt());

                //this.aspGrid.DataBind();
                this.ASPxGridView1.DataBind();
                // this.ASPxGridView2.DataBind();
                //var model = DataTemplateFacade.CreateDataSource2(getFileName(dataValueEntity.ScanLocation, "2"), detailEntity.Channel1.AsInt(), 2).FirstOrDefault();
                //if (model != null)
                //{
                //    // valu = model.ScanData.ToString("F6");
                //}
            }
        }


        protected void aspTree_CustomCallback(object sender, DevExpress.Web.ASPxTreeList.TreeListCustomDataCallbackEventArgs e)
        {
            var db = DataContextFactory.Create();
            int pointId = e.Argument.ToString().AsInt();
            if (pointId == 0)
            {
                // this.aspGrid.DataSource = null;
                this.ASPxGridView1.DataSource = null;
                this.ASPxGridView1.DataBind();
                //this.aspGrid.DataBind();
                return;
            }
            var dataValueEntity = db.FD_DataValue2.FirstOrDefault(u => u.ID == DataValueId);
            var detailEntity = db.FD_ScanPoint.FirstOrDefault(u => u.ID == pointId);

            CacheProxy.Update(detailEntity, CacheKey);
            if (dataValueEntity != null)
            {

                this.ASPxGridView1.DataSource = DataTemplateFacade.CreateDataSource(dataValueEntity.ScanLocation, detailEntity.Channel1.AsInt());
                //this.ASPxGridView1.DataSource = DataTemplateFacade.CreateDataSource(getFileName(dataValueEntity.ScanLocation), detailEntity.Channel1.AsInt());
                // this.ASPxGridView2.DataSource = DataTemplateFacade.CreateDataSource3(getFileName(dataValueEntity.ScanLocation, "3"), detailEntity.Channel1.AsInt());

                //this.aspGrid.DataBind();
                this.ASPxGridView1.DataBind();
                // this.ASPxGridView2.DataBind();
                // var model = DataTemplateFacade.CreateDataSource2(getFileName(dataValueEntity.ScanLocation, "2"), detailEntity.Channel1.AsInt(), 2).FirstOrDefault();
                //if (model != null)
                //{
                //    // valu = model.ScanData.ToString("F6");
                //    e.Result = model.ScanData.ToString("F6");
                //}
            }
        }

    }
}