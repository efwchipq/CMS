using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Common.BaseClass;
using Globalstech.Core.Models.BaseEntity;


namespace Data.EntityFramework {
    public interface IEFDb : IBaseInterface {

        void Add<TEntity>(TEntity entity, bool saveChang = true) where TEntity : class;

        void Delete<TEntity>(TEntity entity, bool saveChang = true) where TEntity : class;

        void Delete<TEntity>(int id, bool saveChang = true) where TEntity : IDEntity;

        void Delete<TEntity>(Expression<Func<TEntity, bool>> filterExpression, bool saveChang = true)
            where TEntity : class;

        void Delete<TEntity>(IQueryable<TEntity> query, bool saveChang = true) where TEntity : class;

        TEntity GetEntity<TEntity>(int id) where TEntity : IDEntity;

        TEntity GetEntity<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        IQueryable<TEntity> GetEntities<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        DbSet<TEntity> GetDbSet<TEntity>() where TEntity : class;

        void Update<TEntity>(TEntity entity, bool saveChang = true) where TEntity : class;

        void Update<TEntity>(Expression<Func<TEntity, TEntity>> updateExpression, bool saveChang = true)
            where TEntity : class;

        void AddOrUpdate<TEntity>(Expression<Func<TEntity, object>> identifierExpression, bool saveChang = true,
            params TEntity[] entities) where TEntity : class;

        void Commit();


    }
}
