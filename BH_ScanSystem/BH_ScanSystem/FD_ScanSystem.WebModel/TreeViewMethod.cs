using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web.UI.WebControls;
using TrainingsApp.Helper;

namespace OfficeShop.Helper {
    public class TreeViewMethod {
        static PropertyInfo proKey = null;
        static PropertyInfo proText = null;
        static PropertyInfo proParent = null;
        static PropertyInfo proAction = null;

        public static void LoadTree<T>(TreeView tree, string rootId, List<T> list, List<string> vals, string valueMember, string displayMember, string parentKey, params object[] paras) {
            rootId = Convert.ToString(rootId);
            Type t = typeof(T);
            proKey = t.GetProperty(valueMember);
            proText = t.GetProperty(displayMember);
            proParent = t.GetProperty(parentKey);
            proAction = t.GetProperty("SupportedAction");
            T root = list.Find(o => string.Compare(Convert.ToString(proKey.GetValue(o, null)), rootId, true) == 0);
            string rootText = "功能列表";
            if (root != null)
                rootText = Convert.ToString(proText.GetValue(root, null));
            //create a root node for entire tree
            TreeNode rNode = new TreeNode(rootText);
            tree.Nodes.Add(rNode);
            //rNode.IsContainer = true;
            //rNode.CheckboxVisible = true;
            //rNode.Tag = root;
            CreateTree<T>(rootId, rNode, list, vals, paras);
        }

        private static void CreateTree<T>(string parentId, TreeNode parentNode, List<T> list, List<string> vals, params object[] paras) {
            List<T> subs = list.FindAll(o => string.Compare(Convert.ToString(proParent.GetValue(o, null)), parentId, true) == 0);
            foreach (T t in subs) {
                string key = Convert.ToString(proKey.GetValue(t, null));
                string text = Convert.ToString(proText.GetValue(t, null));
                TreeNode childNode = new TreeNode(text, key);
                //childNode.IsContainer = true;
                parentNode.ChildNodes.Add(childNode);
                //childNode.Tag = t;
                ////for enum options
                if (paras.Length > 0) {
                    if (IsLeaf<T>(key, list)) {
                        int supportAction = (int)proAction.GetValue(t, null);
                        Type type = (Type)paras[0];
                        List<EnumItem> supportedItems = EnumHelper.GetEnumItems(type);
                        foreach (EnumItem sitem in supportedItems) {
                            int f_action = (int)proAction.GetValue(t, null);
                            int s_action = (int)sitem.Key;
                            if ((f_action & s_action) == s_action)  //s_action > KAction.QUERY
                            {
                                TreeNode enumNode = new TreeNode(sitem.Value.ToString());
                                
                                childNode.ChildNodes.Add(enumNode);  
                            }
                        }
                    }
                }

                if (vals.Exists(a => a == key)) {
                    childNode.Checked = true;
                }
                //for enum options
                CreateTree<T>(key, childNode, list, vals, paras);
            }
        }

        private static bool IsLeaf<T>(string key, List<T> list) {
            List<T> subs = list.FindAll(o => string.Compare(Convert.ToString(proParent.GetValue(o, null)), key, true) == 0);
            return subs.Count > 0 ? false : true;
        }
    }
}
