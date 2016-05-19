using Autofac;
using Globalstech.Core;
using Modules.News.Service.Implementations;
using Modules.News.Service.Interface;

namespace Modules.News {

    public class AutofacDependencyRegistrar : IDependencyRegistrar {
        public void Register(ContainerBuilder builder) {
            builder.RegisterType<NewsService>().As<INewsService>().InstancePerLifetimeScope();
            builder.RegisterType<NewsCategoryService>().As<INewsCategoryService>().InstancePerLifetimeScope();
        }
    }
}