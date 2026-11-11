using DataEntity;
using DataEntity.Services;
using Model.Common;
using Model.HJPlat.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace Business.Manage
{
    public class MenuHelper
    {
        DBDataContext service = DataContextFactory.Create();
        public List<Base_Menu> GetMenus()
        {
            //1.获取全菜单，并缓存
            List<Base_Menu> list = new List<Base_Menu>();
            var MenuCache = CacheUtils.Get(CacheKey.菜单数据缓存);
            if (MenuCache == null)
            {
                list = service.Base_Menu.Where(a => !a.IsDelete.Value && a.Type == 1).ToList();
                CacheUtils.Set(CacheKey.菜单数据缓存, list, 120);
            }
            else
            {
                list = MenuCache as List<Base_Menu>;
            }

            List<Base_Menu> Menus = new List<Base_Menu>();

            //用户信息拿菜单ID
            var MemberInfo = CookieUtils.GetCookiesInfo();
            if (MemberInfo == null)
                return Menus;

            list = list.Where(a => MemberInfo.Menus.Contains(a.ID)).ToList();
            
            return list;
        }
        
    }
}
