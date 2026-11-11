using ExternalService.Video;
using Model.Common;
using Model.Video;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Tool;

namespace Business.Video
{
    public class VideoConstant
    {
        //////横江大道视频参数
        //public const string ProjectCode = "32011103";
        //public const string HikServerIP = "58.213.214.158";
        ////public const string HikServerIP = "10.34.1.100";
        //public const string HikServerPort = "8090";
        //public const string HikServerUser = "gwy";
        //public const string HikServerPwd = "shGWY@829";
        //public const string WebServerIP = "58.213.214.158";

        ////横江大道视频参数
        public const string HikServerUser = "gwy";
        public const string HikServerPwd = "shGWY@829";

        public static readonly string ProjectCode = ConfigurationManager.AppSettings["ProjectCode"].ToString();
        public static readonly string HikServerIP = ConfigurationManager.AppSettings["HikServerIP"].ToString();
        public static readonly string HikServerPort = ConfigurationManager.AppSettings["HikServerPort"].ToString();
        public static readonly string WebServerIP = ConfigurationManager.AppSettings["WebServerIP"].ToString();

    }

    public class HikOrg
    {
        public string i_id { get; set; }
        public string c_org_name { get; set; }
        public string i_parent_id { get; set; }
        public string c_index_code { get; set; }
        public string c_org_desc { get; set; }
        public string c_area_code { get; set; }
        public string c_cascade_code { get; set; }

    }

    public class HikPoint
    {
        public string i_id { get; set; }
        public string i_org_id { get; set; }
        public string c_device_index_code { get; set; }
        public string c_name { get; set; }
        public string i_channel_no { get; set; }
        public string i_camera_type { get; set; }
        public string i_stream_link_type { get; set; }
        public string i_stream_type { get; set; }
        public string c_matrix_code { get; set; }
        public string i_pixel { get; set; }
        public string i_ptz_type { get; set; }
        public string i_is_bind_audio { get; set; }
        public string c_stream_url { get; set; }
        public string i_is_infrared { get; set; }
        public string i_is_auto_follow { get; set; }
        public string i_is_intelligent { get; set; }
        public string i_is_high_digital { get; set; }
        public string c_install_address { get; set; }
        public string c_install_time { get; set; }
        public string i_charge_dept { get; set; }
        public string c_description { get; set; }
        public string i_is_online { get; set; }
        public string c_longitude { get; set; }
        public string c_latitude { get; set; }
        public string c_index_code { get; set; }
        public string i_key_control { get; set; }
        public string c_cascade_code { get; set; }
        public string c_decode_tag { get; set; }
        public string i_res_type { get; set; }
        public string i_device_res_type { get; set; }
        public string b_is_enable { get; set; }

    }

    public class CameraTreeView
    {

        /// <summary>
        /// ID
        /// </summary>
        public int NodeId { get; set; }

        /// <summary>
        /// 0 分类节点
        /// 1 摄像机
        /// </summary>
        public int NodeType { get; set; }


        /// <summary>
        /// 节点名称
        /// </summary>
        public string NodeValue { get; set; }

        /// <summary>
        /// 父节点ID
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// ICO
        /// </summary>
        public string NodeImg { get; set; }

        /// <summary>
        /// 摄像机编号
        /// </summary>
        public string IndexCode { get; set; }


        /// <summary>
        /// 父节点编号
        /// </summary>
        public string ParentCode { get; set; }

        /// <summary>
        /// 父级Item
        /// </summary>
        public CameraTreeView ParentItem { get; set; }

        /// <summary>
        /// 子类Items
        /// </summary>
        public List<CameraTreeView> SubItems { get; set; }
    }

    public class VideoBusiness
    {
        HikServiceHelper hik;
        public List<CameraTreeView> VideoInit()
        {
            //List<CameraTreeView> retObj = new List<CameraTreeView>();

            hik = new HikServiceHelper(VideoConstant.HikServerIP, VideoConstant.HikServerPort, VideoConstant.HikServerUser, VideoConstant.HikServerPwd);
            string xml_org = hik.GetResourceByPage(1, 500, 0, HikResourceType.Org);
            List<HikOrg> all_orgs = XmlHelper.DeserializeToList<HikOrg>(xml_org, "row");

            Dictionary<int, CameraTreeView> catalogList = new Dictionary<int, CameraTreeView>();
            List<CameraTreeView> cameraList = new List<CameraTreeView>();

            GetAll(all_orgs, VideoConstant.ProjectCode, ref cameraList);

            return cameraList.Where(a => a.NodeType == 1).ToList();
        }

        private void GetAll(List<HikOrg> source, string rootCode, ref List<CameraTreeView> list)
        {
            HikOrg pClass = source.Find(a => a.c_index_code == rootCode);
            if (pClass != null)
            {
                list.Add(CreareOrgView(pClass));
                List<HikOrg> sClass = source.FindAll(a => a.i_parent_id == pClass.i_id);
                if (sClass != null && sClass.Count > 0)
                {
                    foreach (HikOrg org in sClass)
                    {
                        GetAll(source, org.c_index_code, ref list);
                    }
                }
                else
                {
                    string xml_point = hik.GetResourceByOrgCode(pClass.c_index_code, HikResourceType.Point);
                    List<HikPoint> all_points = XmlHelper.DeserializeToList<HikPoint>(xml_point, "row");
                    foreach (HikPoint point in all_points)
                    {
                        CameraTreeView view = CrearePointView(point, pClass);
                        list.Add(view);
                    }
                }
            }
        }


        private CameraTreeView CreareOrgView(HikOrg org)
        {
            CameraTreeView view = new CameraTreeView()
            {
                NodeId = (string.IsNullOrEmpty(org.i_id) || org.i_id.Equals("null")) ? 0 : Convert.ToInt32(org.i_id),
                NodeImg = "fico.png",
                NodeType = 0,
                ParentCode = org.i_parent_id.ToString(),
                IndexCode = org.c_index_code,
                NodeValue = org.c_org_name,
                ParentId = (string.IsNullOrEmpty(org.i_parent_id) || org.i_parent_id.Equals("null")) ? 0 : Convert.ToInt32(org.i_parent_id)
            };
            return view;
        }

        private CameraTreeView CrearePointView(HikPoint point, HikOrg org)
        {
            CameraTreeView view = new CameraTreeView()
            {
                NodeId = (string.IsNullOrEmpty(org.i_id) || org.i_id.Equals("null")) ? 0 : Convert.ToInt32(org.i_id),
                NodeImg = "cico.png",
                NodeType = 1,
                ParentCode = org.i_id.ToString(),
                IndexCode = point.c_index_code,
                NodeValue = point.c_name,
                ParentId = (string.IsNullOrEmpty(org.i_parent_id) || org.i_parent_id.Equals("null")) ? 0 : Convert.ToInt32(org.i_parent_id)
            };
            return view;
        }


        #region 新视频tree

        public string GetVideoData(string cameraID, string indexCode)
        {
            cameraID = cameraID ?? "";
            indexCode = indexCode ?? "";

            string token = string.Empty;
            try
            {
                string userName = VideoConstant.HikServerUser;
                string pwd = VideoConstant.HikServerPwd;
                string ip = VideoConstant.HikServerIP;
                string port = VideoConstant.HikServerPort;
                string web = VideoConstant.WebServerIP;
                
                if (HttpContext.Current.Request.Url.ToString().Contains(ConfigurationManager.AppSettings["PortalIntranet"]))
                {
                    web = ip;
                }

                HikServiceHelper service = new HikServiceHelper(ip, port, userName, pwd);
                token = service.GetToken();

                if (!cameraID.Equals("1"))
                {
                    token = service.GetPreviewOcxOptions(indexCode, token, web);
                }
            }
            catch (Exception ex)
            {
                //Logger.Log(ex.ToString());
                token = "";
            }

            return token;
        }

        public List<TreeView> VideoTree()
        {
            hik = new HikServiceHelper(VideoConstant.HikServerIP, VideoConstant.HikServerPort, VideoConstant.HikServerUser, VideoConstant.HikServerPwd);
            string xml_org = hik.GetResourceByPage(1, 500, 0, HikResourceType.Org);
            List<HikOrg> all_orgs = XmlHelper.DeserializeToList<HikOrg>(xml_org, "row");

            List<TreeView> cameraList = new List<TreeView>();

            //添加主目录
            var FirstMenu = all_orgs.FirstOrDefault(a => a.c_index_code == VideoConstant.ProjectCode);

            if (FirstMenu == null)
                return cameraList;

            cameraList.Add(new TreeView()
            {
                text = FirstMenu.c_org_name,
                NodeIndex = "",
                nodes = GetAllTree(all_orgs, FirstMenu)
            });

            return cameraList;
        }

        private List<TreeView> GetAllTree(List<HikOrg> source, HikOrg CurItem)
        {
            List<TreeView> list = new List<TreeView>();

            var subItems = source.Where(a => a.i_parent_id == CurItem.i_id).ToList();

            //获取当前节点下子分支结点
            foreach (var item in subItems)
            {
                TreeView tree = new TreeView();
                tree.text = item.c_org_name;
                tree.NodeIndex = "";

                List<TreeView> nodes = new List<TreeView>();
                nodes = GetAllTree(source, item);
                tree.nodes = nodes;

                list.Add(tree);
            }

            //获取当前节点下叶子节点
            string xml_point = hik.GetResourceByOrgCode(CurItem.c_index_code, HikResourceType.Point);
            List<HikPoint> all_points = XmlHelper.DeserializeToList<HikPoint>(xml_point, "row");
            //按名称排序
            all_points = all_points.OrderBy(a => a.c_name).ToList();
            foreach (HikPoint point in all_points)
            {
                list.Add(new TreeView()
                {
                    text = point.c_name,
                    NodeIndex = point.c_index_code
                });
            }

            return list;
        }

        /// <summary>
        /// 获取视频所有叶子节点
        /// </summary>
        /// <returns>视频所有叶子节点</returns>
        public List<TreeView> GetAllNodes(List<TreeView> allTree)
        {
            var ret = new List<TreeView>();

            //List<TreeView> allTree = VideoTree();

            foreach (var item in allTree)
            {
                var retNodes = GetAllSubNode(item);
                if (retNodes.Count > 0)
                {
                    ret.AddRange(retNodes);
                }
            }

            return ret;
        }

        private List<TreeView> GetAllSubNode(TreeView videoNode)
        {
            var ret = new List<TreeView>();

            if (!string.IsNullOrWhiteSpace(videoNode.NodeIndex))
            {
                ret.Add(videoNode);
            }
            else
            {
                if (videoNode.nodes != null && videoNode.nodes.Count > 0)
                {
                    foreach (var node in videoNode.nodes)
                    {
                        ret.AddRange(GetAllSubNode(node));
                    }
                }
            }

            return ret;
        }

        

        #endregion

    }
}
