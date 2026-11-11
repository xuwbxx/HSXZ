using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Common
{
    public class TreeView
    {
        public TreeView()
        {
            //this.nodes = new List<TreeView>();
            //href = "javascript(void);";
        }
        public string text { set; get; }

        //public string href { set; get; }

        public string[] tags { set; get; }

        public string NodeIndex { set; get; }

        public List<TreeView> nodes { set; get; }
    }

    public class FancyTreeNode
    {
        public FancyTreeNode()
        {
            selected = false;
            partsel = false;
        }
        public string key { get; set; }
        public string title { get; set; }
        public bool selected { get; set; }
        public bool partsel { get; set; }
        public object data { get; set; }
        public int parentID { get; set; }

        public string Path { set; get; }

        public string FunctionCode { set; get; }
        public List<FancyTreeNode> children { get; set; }
    }
}
