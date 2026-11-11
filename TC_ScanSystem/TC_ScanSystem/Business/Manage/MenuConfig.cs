//using DataEntity.ORM;
using Model.Common;
using Model.HJPlat.Response;
//using Repository;
using DataEntity;
using DataEntity.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Model.Manage.Response;

namespace Business.Manage
{
    public class MenuConfig
    {
        DBDataContext service = DataContextFactory.Create();

        /// <summary>
        /// 获取菜单树所有节点。
        /// </summary>
        /// <returns>所有菜单树节点。</returns>
        public List<MenuTree> GetMenuTree()
        {
            List<MenuTree> Menus = new List<MenuTree>();

            var AllMenus = service.Base_Menu.Where(a => !a.IsDelete.Value && a.Type == 1).ToList();

            AllMenus.Where(a => a.ParentID == 0).OrderBy(a => a.Order).ToList().ForEach(a =>
            {
                MenuTree tree = new MenuTree();
                tree.key = a.ID.ToString();
                tree.title = a.MenuName;
                tree.parentID = 0;
                tree.Path = "";
                tree.FunctionCode = a.FunctionCode;
                tree.Class = a.Class;
                tree.Order = Convert.ToInt32(a.Order);
                AllMenus.Where(b => b.ParentID == a.ID).OrderBy(b => b.Order).ToList().ForEach(b =>
                {
                    MenuTree child = new MenuTree();
                    child.key = b.ID.ToString();
                    child.title = b.MenuName;
                    child.parentID = Convert.ToInt32(b.ParentID);
                    child.Path = b.Path;
                    child.FunctionCode = b.FunctionCode;
                    child.Class = "";
                    child.Order = Convert.ToInt32(b.Order);
                    tree.children.Add(child);
                });

                Menus.Add(tree);
            });
            return Menus;
        }


        public List<MenuTree> GetRoleMenuTree(int RoleID)
        {
            List<MenuTree> Menus = new List<MenuTree>();

            var AllMenus = service.Base_Menu.Where(a => !a.IsDelete.Value && a.Type == 1).ToList();

            List<int> RoleMenuIDs = new List<int>();
            service.Base_RoleMenu.Where(a => a.RoleID == RoleID).ToList().ForEach(a =>
            {
                int MenuID = Convert.ToInt32(a.MenuID);
                if (!RoleMenuIDs.Contains(MenuID))
                {
                    RoleMenuIDs.Add(MenuID);
                }
                
            });

            AllMenus.Where(a => a.ParentID == 0).OrderBy(a => a.Order).ToList().ForEach(a =>
            {
                MenuTree tree = new MenuTree();
                tree.key = a.ID.ToString();
                tree.title = a.MenuName;
                tree.parentID = 0;
                tree.Class = a.Class;
                tree.Order = Convert.ToInt32(a.Order);

                int ChildSelectCount = 0;
                AllMenus.Where(b => b.ParentID == a.ID).OrderBy(b => b.Order).ToList().ForEach(b =>
                {
                    MenuTree child = new MenuTree();
                    child.key = b.ID.ToString();
                    child.title = b.MenuName;
                    child.parentID = Convert.ToInt32(b.ParentID);
                    child.Order = Convert.ToInt32(b.Order);
                    if (RoleMenuIDs.Contains(b.ID))
                    {
                        child.selected = true;
                        ChildSelectCount++;
                    }
                    tree.children.Add(child);
                });

                if (ChildSelectCount == 0)
                {
                    tree.selected = false;
                    tree.partsel = false;
                }
                else if (ChildSelectCount == tree.children.Count)
                {
                    tree.selected = true;
                    tree.partsel = false;
                }
                else
                {
                    tree.selected = false;
                    tree.partsel = true;
                }

                Menus.Add(tree);

            });

            return Menus;
        }

        public List<MenuTree> GetAllMenuTree()
        {
            List<MenuTree> Menus = new List<MenuTree>();

            var AllMenus = service.Base_Menu.Where(a => !a.IsDelete.Value && a.Type == 1).ToList();
            

            AllMenus.Where(a => a.ParentID == 0).OrderBy(a => a.Order).ToList().ForEach(a =>
            {
                MenuTree tree = new MenuTree();
                tree.key = a.ID.ToString();
                tree.title = a.MenuName;
                tree.parentID = 0;
                tree.Class = a.Class;
                tree.Order = Convert.ToInt32(a.Order);
                
                AllMenus.Where(b => b.ParentID == a.ID).OrderBy(b => b.Order).ToList().ForEach(b =>
                {
                    MenuTree child = new MenuTree();
                    child.key = b.ID.ToString();
                    child.title = b.MenuName;
                    child.parentID = Convert.ToInt32(b.ParentID);
                    child.Order = Convert.ToInt32(b.Order);
                    tree.children.Add(child);
                });
                
                Menus.Add(tree);

            });

            return Menus;
        }

        public List<MenuTree> GetProjectMenuTree(int ProjectID)
        {
            List<MenuTree> Menus = new List<MenuTree>();

            var AllMenus = service.Base_Menu.Where(a => !a.IsDelete.Value && a.Type == 1).ToList();

            List<int> ProjectMenuIDs = new List<int>();
            service.Base_ProjectMenu.Where(a => a.ProjectID == ProjectID).ToList().ForEach(a =>
            {
                int MenuID = Convert.ToInt32(a.MenuID);
                if (!ProjectMenuIDs.Contains(MenuID))
                {
                    ProjectMenuIDs.Add(MenuID);
                }

            });

            AllMenus.Where(a => a.ParentID == 0).OrderBy(a => a.Order).ToList().ForEach(a =>
            {
                MenuTree tree = new MenuTree();
                tree.key = a.ID.ToString();
                tree.title = a.MenuName;
                tree.parentID = 0;
                tree.Class = a.Class;
                tree.Order = Convert.ToInt32(a.Order);

                int ChildSelectCount = 0;
                AllMenus.Where(b => b.ParentID == a.ID).OrderBy(b => b.Order).ToList().ForEach(b =>
                {
                    MenuTree child = new MenuTree();
                    child.key = b.ID.ToString();
                    child.title = b.MenuName;
                    child.parentID = Convert.ToInt32(b.ParentID);
                    child.Order = Convert.ToInt32(b.Order);
                    if (ProjectMenuIDs.Contains(b.ID))
                    {
                        child.selected = true;
                        ChildSelectCount++;
                    }
                    tree.children.Add(child);
                });

                if (ChildSelectCount == 0)
                {
                    tree.selected = false;
                    tree.partsel = false;
                }
                else if (ChildSelectCount == tree.children.Count)
                {
                    tree.selected = true;
                    tree.partsel = false;
                }
                else
                {
                    tree.selected = false;
                    tree.partsel = true;
                }

                Menus.Add(tree);

            });

            return Menus;
        }
    }
}