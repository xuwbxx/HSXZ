using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeShop.WebModel;
using FD_ScanSystem.WebModel;
using FDScanSystem.DAO;
using FD_ScanSystem.WebModel.Cache;
using FDScanSystem.Web.DataInfo.DataProcess;
using ZJFramework10.Extension;
namespace FDScanSystem.Web.DataInfo
{
    public partial class DynamicDataQueryDetail : AdminPageBase
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
                if (adminContext.UserName.ToLowerInvariant().Equals("admin"))
                {
                    url.Visible = true;
                }
                else
                {
                    url.Visible = false;
                }
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
                var  detailEntity = CacheProxy.Get<FD_ScanPoint>(() => { return null; }, CacheKey);
                if (dataValueEntity != null)
                {
                    if (detailEntity != null)
                    {
                      //  this.aspGrid.DataSource = DataTemplateFacade.CreateDataSource(dataValueEntity.ScanLocation, detailEntity.Channel1.AsInt());
                        this.ASPxGridView1.DataSource = DataTemplateFacade.CreateDataSource(getFileName(dataValueEntity.ScanLocation), detailEntity.Channel1.AsInt());
                        this.ASPxGridView2.DataSource = DataTemplateFacade.CreateDataSource3(getFileName(dataValueEntity.ScanLocation, "3"), detailEntity.Channel1.AsInt());

                        //this.aspGrid.DataBind();
                        this.ASPxGridView1.DataBind();
                        this.ASPxGridView2.DataBind();

                        var model = DataTemplateFacade.CreateDataSource2(getFileName(dataValueEntity.ScanLocation, "2"), detailEntity.Channel1.AsInt(), 2).FirstOrDefault();
                        if (model != null)
                        {
                            //valu = model.ScanData.ToString("F6");
                        }
                        //double value = DataTemplateFacade.CreateDataSource(getFileName(dataValueEntity.ScanLocation), detailEntity.Channel1.AsInt());

                    }
                }
            }
            
        }

        private string getFileName(string filename)
        {
            string[] f = filename.Split('.');
            if (f.Length > 1)
            {
                return f[0] + "-1." + f[1];
            }
            return "";
        }

        private string getFileName(string filename,string Number)
        {
            string[] f = filename.Split('.');
            if (f.Length > 1)
            {
                return f[0] + "-" + Number + "." + f[1];
            }
            return "";
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

                //this.aspGrid.DataSource = DataTemplateFacade.CreateDataSource(dataValueEntity.ScanLocation, detailEntity.Channel1.AsInt());
                this.ASPxGridView1.DataSource = DataTemplateFacade.CreateDataSource(getFileName(dataValueEntity.ScanLocation, "2"), detailEntity.Channel1.AsInt());
                this.ASPxGridView2.DataSource = DataTemplateFacade.CreateDataSource3(getFileName(dataValueEntity.ScanLocation, "3"), detailEntity.Channel1.AsInt());

                //this.aspGrid.DataBind();
                this.ASPxGridView1.DataBind();
                this.ASPxGridView2.DataBind();

                var model = DataTemplateFacade.CreateDataSource2(getFileName(dataValueEntity.ScanLocation, "2"), detailEntity.Channel1.AsInt(), 2).FirstOrDefault();
                if (model != null)
                {
                   // valu = model.ScanData.ToString("F6");
                }
            }
        }



        public override string PermissionCode
        {
            get { return PermissionDict.DataInfo_Dynamic_Query; }
        }

        protected void aspxTree_FocusedNodeChanged(object sender, EventArgs e)
        {
            var db = DataContextFactory.Create();

            var dataValueEntity = db.FD_DataValue2.FirstOrDefault(u => u.ID == DataValueId);
            var detailEntity = this.aspxTree.FocusedNode.DataItem as QueryDetailTreeEntity;
            CacheProxy.Update(detailEntity, CacheKey);
            if (dataValueEntity != null)
            {

                //this.aspGrid.DataSource = DataTemplateFacade.CreateDataSource(dataValueEntity.ScanLocation, detailEntity.Channel1.AsInt());
                this.ASPxGridView1.DataSource = DataTemplateFacade.CreateDataSource(getFileName(dataValueEntity.ScanLocation), detailEntity.Channel1.AsInt());
                this.ASPxGridView2.DataSource = DataTemplateFacade.CreateDataSource3(getFileName(dataValueEntity.ScanLocation, "3"), detailEntity.Channel1.AsInt());

                //this.aspGrid.DataBind();
                this.ASPxGridView1.DataBind();
                this.ASPxGridView2.DataBind();

                var model = DataTemplateFacade.CreateDataSource2(getFileName(dataValueEntity.ScanLocation, "2"), detailEntity.Channel1.AsInt(), 2).FirstOrDefault();
                if (model != null)
                {
                   // valu = model.ScanData.ToString("F6");
                }
            }

        }

        //protected void aspGrid_PageSizeChange(object sender, EventArgs e)
        //{
 
        //}

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

                //this.aspGrid.DataSource = DataTemplateFacade.CreateDataSource(dataValueEntity.ScanLocation, detailEntity.Channel1.AsInt());
                this.ASPxGridView1.DataSource = DataTemplateFacade.CreateDataSource(getFileName(dataValueEntity.ScanLocation), detailEntity.Channel1.AsInt());
                this.ASPxGridView2.DataSource = DataTemplateFacade.CreateDataSource3(getFileName(dataValueEntity.ScanLocation, "3"), detailEntity.Channel1.AsInt());

                //this.aspGrid.DataBind();
                this.ASPxGridView1.DataBind();
                this.ASPxGridView2.DataBind();
                var model = DataTemplateFacade.CreateDataSource2(getFileName(dataValueEntity.ScanLocation, "2"), detailEntity.Channel1.AsInt(), 2).FirstOrDefault();
                if (model != null)
                {
                   // valu = model.ScanData.ToString("F6");
                    e.Result = model.ScanData.ToString("F6");
                }
            }
        }

        protected void aspGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomCallbackEventArgs e)
        {
            var db = DataContextFactory.Create();
            int pointId = e.Parameters.AsInt();
            if (pointId == 0)
            {
               // this.aspGrid.DataSource = null;
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

                //this.aspGrid.DataSource = DataTemplateFacade.CreateDataSource(dataValueEntity.ScanLocation, detailEntity.Channel1.AsInt());
                this.ASPxGridView1.DataSource = DataTemplateFacade.CreateDataSource(getFileName(dataValueEntity.ScanLocation), detailEntity.Channel1.AsInt());
                this.ASPxGridView2.DataSource = DataTemplateFacade.CreateDataSource3(getFileName(dataValueEntity.ScanLocation, "3"), detailEntity.Channel1.AsInt());

                // this.aspGrid.DataBind();
                this.ASPxGridView1.DataBind();
                this.ASPxGridView2.DataBind();
            }
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

                //this.aspGrid.DataSource = DataTemplateFacade.CreateDataSource(dataValueEntity.ScanLocation, detailEntity.Channel1.AsInt());
                this.ASPxGridView1.DataSource = DataTemplateFacade.CreateDataSource(getFileName(dataValueEntity.ScanLocation), detailEntity.Channel1.AsInt());
                this.ASPxGridView2.DataSource = DataTemplateFacade.CreateDataSource3(getFileName(dataValueEntity.ScanLocation, "3"), detailEntity.Channel1.AsInt());

                //this.aspGrid.DataBind();
                this.ASPxGridView1.DataBind();
                this.ASPxGridView2.DataBind();
                var model = DataTemplateFacade.CreateDataSource2(getFileName(dataValueEntity.ScanLocation, "2"), detailEntity.Channel1.AsInt(), 2).FirstOrDefault();
                if (model != null)
                {
                  // valu = model.ScanData.ToString("F6");
                }
            }
        }


        protected void aspGrid2_CustomCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomCallbackEventArgs e)
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

               // this.aspGrid.DataSource = DataTemplateFacade.CreateDataSource(dataValueEntity.ScanLocation, detailEntity.Channel1.AsInt());
                this.ASPxGridView1.DataSource = DataTemplateFacade.CreateDataSource(getFileName(dataValueEntity.ScanLocation), detailEntity.Channel1.AsInt());
                this.ASPxGridView2.DataSource = DataTemplateFacade.CreateDataSource3(getFileName(dataValueEntity.ScanLocation, "3"), detailEntity.Channel1.AsInt());

               // this.aspGrid.DataBind();
                this.ASPxGridView1.DataBind();
                this.ASPxGridView2.DataBind();
                var model = DataTemplateFacade.CreateDataSource2(getFileName(dataValueEntity.ScanLocation, "2"), detailEntity.Channel1.AsInt(), 2).FirstOrDefault();
                if (model != null)
                {
                   //valu = model.ScanData.ToString("F6");
                }
            }
        }


        //protected void btnXlsxExport_Click(object sender, EventArgs e)
        //{
        //    gridExport.WriteXlsxToResponse();
        //}

        protected void btnXlsxExport1_Click(object sender, EventArgs e)
        {
            gridExport1.WriteXlsToResponse();
        }

        protected void btnXlsxExport2_Click(object sender, EventArgs e)
        {
            gridExport2.WriteXlsToResponse();
        }

       
    }


    public class QueryDetailTreeEntity
    {
        public int NodeId { get; set; }

        public int ParentId { get; set; }

        public string NodeName { get; set; }

        public string Channel1 { get; set; }

        public double AlertMin { get; set; }

        public double AlertMax { get; set; }

        public string Remark { get; set; }

    }
}