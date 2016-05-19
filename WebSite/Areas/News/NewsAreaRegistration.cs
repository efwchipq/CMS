using System.Web.Mvc;

namespace Modules.News {
    /// <summary>
    /// 重写区域路由，文件名称必须以AreaRegistration结尾  
    /// </summary>
    public class NewsAreaRegistration : AreaRegistration {

        public override string AreaName {
            get {
                return "News";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) {
            context.MapRoute(
                "NewsArea",
                "News/{controller}/{action}/{id}",
                new {
                    action = "List",
                    id = UrlParameter.Optional
                }, new[] { "Modules.News.Controllers" }
                );
            //context.MapRoute(
            //    "ArticleArea",
            //    "{area}/{controller}/{action}/{id}",
            //    new {
            //        area = "Article",
            //        controller = "List",
            //        action = "Index",
            //        id = UrlParameter.Optional
            //    }, new[] { "Modules.Article.Controllers" }
            //    );
            //ViewEngines.Engines.Add(new ArticleViewEngine());
        }

    }

    ///// <summary>
    /////  注册论坛视图引擎
    ///// </summary>
    //public sealed class ArticleViewEngine : RazorViewEngine {

    //    public ArticleViewEngine() {
    //        AreaViewLocationFormats = new[]
    //        {
    //            "~/Areas/Newsfocus/Views/{1}/{0}.cshtml",
    //            "~/Areas/Newsfocus/Views/Shared/{0}.cshtml",
    //            "~/Areas/Newsfocus/Views/Admin/{1}/{0}.cshtml"//我们的规则
    //        };

    //        PartialViewLocationFormats = AreaViewLocationFormats;
    //        AreaPartialViewLocationFormats = AreaViewLocationFormats;
    //    }
    //}

}