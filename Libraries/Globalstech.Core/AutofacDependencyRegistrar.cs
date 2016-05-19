using Autofac;
using Nop.Services.Authentication;

namespace Globalstech.Core {
    //public class AutofacDependencyRegistrar : Module {
    //    protected override void Load(ContainerBuilder builder) {
    //        //base.Load(builder);
    //        builder.RegisterType<FormsAuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();
    //    }
    //}

    public class AutofacDependencyRegistrar : IDependencyRegistrar {
        public void Register(ContainerBuilder builder) {
            builder.RegisterType<FormsAuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();
        }
    }
}
