using System.Web.Mvc;
using Modules.News.Service.Interface;

namespace WebSite.Controllers {
    public class BackPageController : Controller {

        private readonly INewsService _newsService;

        public BackPageController(INewsService newsService) {
            _newsService = newsService;
        }

        //
        // GET: /BackPage/
        public ActionResult Index() {
            
            ViewBag.Title = "CMS后台管理";
            return View();
        }

        public ActionResult Default()
        {
            return View();
        }

    }

    public enum myen {
        //橙黄色
        TabMenu_OrangeYellow = 0,
        //浅灰色
        TabMenu_LightGray = 1,
    }
}