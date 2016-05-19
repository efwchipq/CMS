using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using EntityFramework.Extensions;
using Globalstech.Core.Models.BaseEntity;

namespace Data.EntityFramework {
    //http://www.cnblogs.com/lingyuan/archive/2010/10/29/1864345.html  EF学习
    //http://www.bubuko.com/infodetail-1149266.html  EF优化
    public class EFDb : IEFDb {

        private DbContext _context;
        //EF连接字符串
        private const string ConnectionStrings = "Name=PlatformSqlServer";

        /// <summary>
        /// 实体
        /// </summary>
        public DbContext Context {
            get {
                if (_context == null) {
                    _context = new GlobalstechContext(ConnectionStrings);
                }
                return _context;
            }
        }

        #region 增加

        /// <summary>
        /// 增加
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="saveChang"></param>
        public void Add<TEntity>(TEntity entity, bool saveChang = true) where TEntity : class {
            Context.Set<TEntity>().Add(entity);
            if (saveChang) {
                Context.SaveChanges();
            }
        }

        #endregion

        #region 删除

        /// <summary>
        /// 根据实体类删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="saveChang"></param>
        public void Delete<TEntity>(TEntity entity, bool saveChang = true) where TEntity : class {
            Context.Set<TEntity>().Remove(entity);
            if (saveChang) {
                Context.SaveChanges();
            }
        }

        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <param name="saveChang"></param>
        public void Delete<TEntity>(int id, bool saveChang = true) where TEntity : IDEntity {
            Context.Set<TEntity>().Delete(t => t.ID == id);
            if (saveChang) {
                Context.SaveChanges();
            }
        }

        /// <summary>
        /// 根据表达式树删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filterExpression"></param>
        /// <param name="saveChang"></param>
        public void Delete<TEntity>(Expression<Func<TEntity, bool>> filterExpression, bool saveChang = true) where TEntity : class {
            Context.Set<TEntity>().Delete(filterExpression);
            if (saveChang) {
                Context.SaveChanges();
            }
        }

        /// <summary>
        /// 根据查询条件删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="saveChang"></param>
        public void Delete<TEntity>(IQueryable<TEntity> query, bool saveChang = true) where TEntity : class {
            Context.Set<TEntity>().Delete(query);
            if (saveChang) {
                Context.SaveChanges();
            }
        }

        #endregion

        #region 查询

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns>单个实体或null</returns>
        public TEntity GetEntity<TEntity>(int id) where TEntity : IDEntity {
            return Context.Set<TEntity>().FirstOrDefault(t => t.ID == id);
        }

        /// <summary>
        /// 根据表达式树查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns>单个实体或null</returns>
        public TEntity GetEntity<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class {
            return Context.Set<TEntity>().FirstOrDefault(predicate);
        }

        /// <summary>
        /// 根据表达式树查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns>可查询对象</returns>
        public IQueryable<TEntity> GetEntities<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class {
            return Context.Set<TEntity>().Where(predicate);
        }

        /// <summary>
        /// 获取从数据库中查询的给定类型的所有实体的集合
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public DbSet<TEntity> GetDbSet<TEntity>() where TEntity : class {
            return Context.Set<TEntity>();
        }

        #endregion

        #region 修改
        //http://www.cnblogs.com/jameszou/archive/2013/03/12/2956281.html
        //http://www.bubuko.com/infodetail-251170.html EntityState四种方法的区别

        /// <summary>
        /// 更新实体，处理不更新字段
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="saveChang"></param>
        public void Update<TEntity>(TEntity entity, bool saveChang = true) where TEntity : class {
            //获取包装类
            var entry = Context.Entry<TEntity>(entity);//entry.State等于Detached
            //entry.State = EntityState.Modified;全部属性设置为改变
            //Attach 设置为全部为Unchanged

            Context.Set<TEntity>().Attach(entity);//context.Entry(destination).State = EntityState.Unchanged;   //跟Attach方法一样效果

            //处理公共字段
            if (entity is BaseEntity) {
                var efEntity = entity as BaseEntity;
                var efEntityEntry = Context.Entry(efEntity);

                var properties = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var property in properties) {
                    efEntityEntry.Property(property.Name).IsModified = true;
                }
                efEntity.LastModifiedOnDate = DateTime.Now;
                efEntity.LastModifiedByUserID = 0;
                efEntityEntry.Property(t => t.CreatedOnDate).IsModified = false;
                efEntityEntry.Property(t => t.CreatedByUserID).IsModified = false;
            } else {
                entry.State = EntityState.Modified;
            }
            if (saveChang) {
                Context.SaveChanges();
            }
        }

        /// <summary>
        /// 更新实体（根据表达式树）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="updateExpression"></param>
        /// <param name="saveChang"></param>
        public void Update<TEntity>(Expression<Func<TEntity, TEntity>> updateExpression, bool saveChang = true) where TEntity : class {
            Context.Set<TEntity>().Update(updateExpression);
            if (saveChang) {
                Context.SaveChanges();
            }
        }

        public void AddOrUpdate<TEntity>(Expression<Func<TEntity, object>> identifierExpression, bool saveChang = true, params TEntity[] entities) where TEntity : class {
            Context.Set<TEntity>().AddOrUpdate(identifierExpression, entities);
            if (saveChang) {
                Context.SaveChanges();
            }
        }

        #endregion

        public void Commit() {
            Context.SaveChanges();
        }
    }
}
