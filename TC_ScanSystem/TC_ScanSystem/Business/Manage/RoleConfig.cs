using DataEntity.Services;
using DataEntity;
using Model.Common;
using Model.HJPlat.Response;
//using Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Model.Manage.Response;

namespace Business.Manage
{
    public class RoleConfig
    {
        DBDataContext service = DataContextFactory.Create();

        public List<MenuTree> GetMenuTreeByRole(int roleIdx)
        {
            //List<FancyTreeNode> retTree = new List<FancyTreeNode>();
            MenuConfig menuBiz = new MenuConfig();

            var retTree = menuBiz.GetMenuTree();

            var authorLst = GetAuthorization(roleIdx);

            SetNodeSelectStatus(authorLst, retTree);

            return retTree;
        }

        private void SetNodeSelectStatus(int[] authorArr, List<MenuTree> menuTree)
        {
            int tempKey;
            foreach (var subNode in menuTree)
            {
                if (int.TryParse(subNode.key, out tempKey))
                {
                    SetNodeSelectStatus(authorArr, subNode);
                }
            }
        }

        private bool SetNodeSelectStatus(int[] authorArr, MenuTree menuNode)
        {
            bool ret = true;

            if (authorArr.Contains(Convert.ToInt32(menuNode.key)))
            {
                if (menuNode.children == null)
                {
                    menuNode.selected = true;
                }
                else
                {
                    bool isAllSubNodesSelected = true;
                    foreach (var item in menuNode.children)
                    {
                        //menuNode.selected = SetNodeSelectStatus(authorLst, item);
                        isAllSubNodesSelected &= SetNodeSelectStatus(authorArr, item);
                    }
                    if (isAllSubNodesSelected)
                    {
                        menuNode.selected = true;
                    }
                    else
                    {
                        menuNode.partsel = true;
                    }
                }
            }
            else
            {
                ret = false;
            }

            return ret;
        }


        public int[] GetAuthorization(int roleIdx)
        {
            //在数据库表New_RoleMenu中RoleID为roleIdx的所有记录中，取字段MenuID，且MenuID为非空的数组。
            int[] authorArr = service.Base_RoleMenu.Where(a => a.RoleID == roleIdx).Select(a => a.MenuID).Where(a => a != null).Select(a => a.Value).ToArray();

            return authorArr;
        }
        

        public Base_Role CopyCfgDataToDB(RoleInfoResponse source)
        {
            Base_Role ret = new Base_Role();

            ret.RoleCode = source.RoleCode;
            ret.RoleName = TrimStr(source.RoleName);

            return ret;
        }

        public Base_RoleMenu[] CopyMenuDataToDB(int roleIdx, int[] menuIDs)
        {
            DateTime createTime = DateTime.Now;

            List<Base_RoleMenu> ret = new List<Base_RoleMenu>();

            if (menuIDs != null && menuIDs.Length > 0)
            {
                foreach (var item in menuIDs)
                {
                    Base_RoleMenu menuData = new Base_RoleMenu();

                    menuData.RoleID = roleIdx;
                    menuData.MenuID = item;
                    menuData.CreateTime = createTime;
                    menuData.IsDelete = false;

                    ret.Add(menuData);
                }
            }

            return ret.ToArray();
        }
        
        public bool Validate(RoleInfoResponse request, out string errMsg)
        {
            bool ret = true;
            errMsg = string.Empty;

            if (string.IsNullOrWhiteSpace(request.RoleCode))
            {
                errMsg = "角色码不能为空！";
                ret = false;
            }
            if (string.IsNullOrWhiteSpace(request.RoleName))
            {
                errMsg = "角色名不能为空！";
                ret = false;
            }

            return ret;
        }

        private string TrimStr(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            return str.Trim();
        }
    }
}
