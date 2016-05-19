using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Models.Service;

namespace WebSite.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            var service = new HomeService();
            var menus = service.GetMenus("/Files/Menu/HomeMenu.xml");
            return View();
        }

        public ActionResult Menus() {
            var service = new HomeService();
            var menus = service.GetMenus("/Files/Menu/HomeMenu.xml");
            //查@functions @section  @helper
            return PartialView(menus);
        }

        public ActionResult About() {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact() {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}