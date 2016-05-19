using Autofac;
using Globalstech.Core;

namespace Data.EntityFramework {
    public class AutofacDependencyRegistrar : IDependencyRegistrar {
        public void Register(ContainerBuilder builder) {
            builder.RegisterType<EFDb>().As<IEFDb>().InstancePerDependency();
            builder.RegisterType<TestAutofac>().As<ITestAutofac>().InstancePerDependency();

            
            //if (typeof(ISingletonDependency).IsAssignableFrom(type)) {
            //    registration.SingleInstance();
            //} else if (typeof(ITransientDependency).IsAssignableFrom(type)) {
            //    registration.InstancePerDependency();
            //} else {
            //    registration.InstancePerLifetimeScope();
            //}
        }
    }
}
