using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework {
    public class GlobalstechContext : DbContext {

        static GlobalstechContext() {
            //http://www.mzwu.com/article.asp?id=3417
            Database.SetInitializer<GlobalstechContext>(null);
            /*
             Database.SetInitializer<testContext>(new CreateDatabaseIfNotExists<testContext>());数据库不存在时重新创建数据库
             Database.SetInitializer<testContext>(new DropCreateDatabaseAlways<testContext>());每次启动应用程序时创建数据库
             Database.SetInitializer<testContext>(new DropCreateDatabaseIfModelChanges<testContext>());模型更改时重新创建数据库
             Database.SetInitializer<testContext>(null);从不创建数据库
             */
        }

        public GlobalstechContext(string connection)
            : base(connection) {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            var domainAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            if (domainAssemblies.Any()) {
                var moudelAssemblies = domainAssemblies.Where(t => t.GetName().Name.StartsWith("Modules")).Distinct().ToList();//WebSite
                moudelAssemblies.AddRange(domainAssemblies.Where(t => t.GetName().Name.StartsWith("WebSite")).Distinct().ToList());
                foreach (var assembly in moudelAssemblies) {
                    modelBuilder.Configurations.AddFromAssembly(assembly);
                }
            }
        }
    }
}
