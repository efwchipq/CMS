using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Globalstech.Core;

namespace WebSite {
    public class MvcApplication : System.Web.HttpApplication {
        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //框架级IOC注入
            GlobalstechDependencyRegistrar.DependencyRegistrar();
        }
    }
}
