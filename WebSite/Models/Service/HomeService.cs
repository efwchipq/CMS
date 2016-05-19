using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Common.Utils.XML;

namespace WebSite.Models.Service {
    public class HomeService {

        public IEnumerable<MenuInfo> GetMenus(string menusFile) {
            var service = new XMLTools();
            var root = service.GetRootElement(menusFile);
            var menus = new List<MenuInfo>();
            GetClassOneMenus(root, menus);
            return menus;
        }

        public void GetClassOneMenus(XElement root, List<MenuInfo> menus) {
            var service = new XMLTools();
            var classOneMenus = service.GetElements(root, "ItemClassOne");
            foreach (var element in classOneMenus) {
                //获取一级目录路由
                var classOneMenusTabRoute = service.GetElement(element, "TabRoute");
                //一级目录路由
                var menu = service.GetEntity<MenuInfo>(classOneMenusTabRoute);
                GetClassTwoMenus(element, menus, menu);
                menus.Add(menu);
            }
        }

        public void GetClassTwoMenus(XElement classTwoElement, List<MenuInfo> menus, MenuInfo classOneMenu) {
            var service = new XMLTools();
            //二级目录TabRoute
            var classTwoMenus = service.GetElements(classTwoElement, "ItemClassTwo");
            foreach (var element in classTwoMenus) {
                //获取二级级目录路由
                var classTwoMenusTabRoute = service.GetElement(element, "TabRoute");
                //二级目录路由
                var menu = service.GetEntity<MenuInfo>(classTwoMenusTabRoute);
                menu.ParentID = classOneMenu.ID;
                menus.Add(menu);
            }
        }
    }
}