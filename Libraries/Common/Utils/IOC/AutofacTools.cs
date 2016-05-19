using Autofac;
using Common.BaseClass;

namespace Common.IOCTools {
    public class AutofacTools {

        /// <summary>
        /// 通过IOC获取接口实现类实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="autofacModule"></param>
        /// <returns></returns>
        public static T Resolve<T>(Module autofacModule) where T : IBaseInterface {
            var builder = new ContainerBuilder();
            //builder.RegisterModule(new AutofacModule());
            builder.RegisterModule(autofacModule);
            using (var contailner = builder.Build()) {
                var entity = contailner.Resolve<T>();
                return entity;
            }
        }


    }
}
