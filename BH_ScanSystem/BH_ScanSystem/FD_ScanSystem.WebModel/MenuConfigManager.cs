using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ZJFramework10.Extension;
using FD_ScanSystem.WebModel.Auth;

namespace FD_ScanSystem.WebModel
{

    public class MenuItem
    {
        public static int _Idx = 0;
        static int CreateIdx()
        {
            return System.Threading.Interlocked.Increment(ref _Idx);
        }

        public static void ResetIdx()
        {
            System.Threading.Interlocked.Exchange(ref _Idx, 0);
        }

        public MenuItem()
        {
            this.Idx = CreateIdx();
        }

        /// <summary>
        /// 主键
        /// </summary>
        public int Idx { get; private set; }

        public int FunctionId { set; get; }

        /// <summary>
        /// title和描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 权限编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 样式
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// 深度
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Ico { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 父级Item
        /// </summary>
        public MenuItem ParentItem { get; set; }


        /// <summary>
        /// 子类Items
        /// </summary>
        public List<MenuItem> SubItems { get; set; }

    }


    public class MenuConfigManager
    {
        static List<MenuItem> _AllItems;

        static List<MenuItem> _AllVoteItems;

        public static void Initialize(IAuth auth)
        {
            string pyLoc = System.Web.HttpContext.Current.Server.MapPath("/config/menu.config");
            var doc = XDocument.Load(pyLoc);
            _AllItems = new List<MenuItem>();
            if (doc != null)
            {
                var menus = doc.Root.Elements("menu");
                foreach (var mItem in menus)
                {
                    Add(auth, null, mItem, t => { _AllItems.Add(t); });
                }
            }
            if (_AllItems.Count > 0)
            {
                _AllItems = _AllItems.OrderByDescending(u => u.Depth).ToList();
            }

            pyLoc = System.Web.HttpContext.Current.Server.MapPath("/config/votemenu.config");
            doc = XDocument.Load(pyLoc);
            _AllVoteItems = new List<MenuItem>();
            if (doc != null)
            {
                var menus = doc.Root.Elements("menu");
                foreach (var mItem in menus)
                {
                    Add(auth, null, mItem, t => { _AllVoteItems.Add(t); });
                }
            }
            if (_AllVoteItems.Count > 0)
            {
                _AllVoteItems = _AllVoteItems.OrderByDescending(u => u.Depth).ToList();
            }
        }

        static void Add(IAuth auth, MenuItem parent, XElement ele, Action<MenuItem> addCall)
        {
            MenuItem temp = new MenuItem();
            temp.Class = GetStr(ele.Attribute("class"));
            temp.Code = GetStr(ele.Attribute("code"));
            temp.Depth = GetInt(ele.Attribute("depth"));
            temp.Description = GetStr(ele.Attribute("desc"));
            temp.Ico = GetStr(ele.Attribute("ico"));
            temp.Link = GetStr(ele.Attribute("link"));
            temp.ParentItem = parent;
            temp.Sort = GetInt(ele.Attribute("sort"));
            if (!auth.Auth(temp.Code)) return;

            if (parent != null)
            {
                if (parent.SubItems == null)
                {
                    parent.SubItems = new List<MenuItem>(3);
                }
                parent.SubItems.Add(temp);
            }

            addCall(temp);
            var subMenus = ele.Elements("menu").ToList();
            if (subMenus != null && subMenus.Count > 0)
            {
                foreach (var mItem in subMenus)
                {
                    Add(auth, temp, mItem, addCall);
                }
            }
        }

        static int GetInt(XAttribute ele)
        {
            if (ele == null) return 0;
            if (string.IsNullOrEmpty(ele.Value)) return 0;
            return ele.Value.AsInt();
        }

        static string GetStr(XAttribute ele)
        {
            if (ele == null) return string.Empty;
            if (string.IsNullOrEmpty(ele.Value)) return string.Empty;
            return ele.Value;
        }

        public static List<MenuItem> ListRootMenus()
        {
            return _AllItems.Where(u => u.Depth == 1).OrderBy(u => u.Sort).ToList();
        }

        public static List<MenuItem> ListVoteRootMenus()
        {
            return _AllVoteItems.Where(u => u.Depth == 1).OrderBy(u => u.Sort).ToList();
        }


        public static MenuItem GetMenu(string code)
        {
            return _AllItems.FirstOrDefault(u => u.Code.Equals(code, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}

