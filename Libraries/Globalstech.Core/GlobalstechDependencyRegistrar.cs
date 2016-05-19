using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Globalstech.Core.Infrastructure;
using Globalstech.Core.Models;

namespace Globalstech.Core {
    public static class GlobalstechDependencyRegistrar {

        public static void DependencyRegistrar() {
            //依赖注入
            var builder = new ContainerBuilder();
            var domainAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            //Controller依赖注入
            if (domainAssemblies.Any()) {
                var moudelAssemblies = domainAssemblies.Where(t => t.GetName().Name.StartsWith("Modules")).Distinct().ToList();//WebSite
                moudelAssemblies.AddRange(domainAssemblies.Where(t => t.GetName().Name.StartsWith("WebSite")).Distinct().ToList());
                //moudelAssemblies.AddRange(domainAssemblies.Where(t => t.GetName().Name.StartsWith("Data")).Distinct().ToList());
                foreach (var assembly in moudelAssemblies) {
                    builder.RegisterControllers(assembly);
                    //builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces();
                }
                ////注册所有模块(调用所有自定义Module的load方法)
                //builder.RegisterAssemblyModules(assemblies.ToArray());
                RegistrationModule(builder);
            }
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        /// <summary>
        /// 注册自定义依赖
        /// </summary>
        /// <param name="builder"></param>
        private static void RegistrationModule(ContainerBuilder builder) {
            //获取所有实现IDependencyRegistrar接口的类型
            var drTypes = AssembliesTools.FindClassesOfType<IDependencyRegistrar>();
            var drInstances = new List<IDependencyRegistrar>();
            foreach (var drType in drTypes) {
                //实例化
                drInstances.Add((IDependencyRegistrar)Activator.CreateInstance(drType));
            }
            foreach (var dependencyRegistrar in drInstances) {
                //调用实例中的注册方法
                dependencyRegistrar.Register(builder);
            }
        }
    }
}
