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
namespace FDScanSystem.Web
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string ltstr = "";
            ltstr = "<div style=\"position:absolute; float:left; z-index:9999; margin-top:{0}px; margin-left:{1}px\">\n";
            ltstr += "<a href=\"{4}\" title=\"{2}\" ><img src=\"{3}\" Height=25px Width=25px /></a>\n</div>\n";
            var db = DataContextFactory.Create();
            var temp_pos = db.FD_ScanNode.ToList();

            if (temp_pos.Count > 0)
            {
                for (int i = 0; i < temp_pos.Count; i++)
                {
                    //img_Layer.Visible = true;
                    //string url = WebHelper.Root + "MK_DataValue_List.aspx?SNP=" + temp_pos[i].ID;
                    string url = "/DataInfo/FD_LayerImage.aspx?pid=" + temp_pos[i].ID;
                    string title = temp_pos[i].NodeName;
                    int ID = temp_pos[i].ID;
                    //  List<MK_Point> temp_pt = DB.MK_Point.Where(d => d.PositionId == ID).ToList();

                    List<FD_DataValue> temp_value = db.FD_DataValue.Where(d => d.PointId == ID).ToList();
                    string img = "/Upload/point.png";
                    ltDiv.Text += string.Format(ltstr, temp_pos[i].MapY, temp_pos[i].MapX, title, img, url);

                }
            }
            else
            {
                Response.Redirect("/Index.aspx");
            }
        }
    }
}