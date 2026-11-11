using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeShop.WebModel;
using FD_ScanSystem.WebModel;
using FDScanSystem.DAO;
using FDScanSystem.Web.DataInfo;

namespace FDScanSystem.Web
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var db = DataContextFactory.Create();

            var deviceInfo = db.FD_Device.FirstOrDefault(u => u.ID == 4);

            List<QueryDetailTreeEntity> result = db.FD_ScanPoint.Where(u => u.DeviceId == 4).Select(y => new QueryDetailTreeEntity
            {
                NodeId = y.ID,
                ParentId = 0,
                AlertMax = y.AlertMax??0,
                Channel1 = y.Channel1,
                NodeName = y.PointName,
                Remark = y.Remark,
                AlertMin = y.AlertMin??0
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
        }
    }
}