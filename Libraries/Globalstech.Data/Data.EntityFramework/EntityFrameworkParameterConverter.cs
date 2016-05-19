using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework {

    /*
     * 表达式树资料
     * http://blog.csdn.net/swarb/article/details/8098221  含导航属性列子
     * http://www.cnblogs.com/lyj/archive/2008/03/25/1122157.html#undefined
     */
    public class EntityFrameworkParameterConverter<TEntity> where TEntity : class {

        private DbSet<TEntity> _dbSet;

        public DbSet<TEntity> DbSet {
            get {
                if (_dbSet == null) {
                    _dbSet = new EFDb().GetDbSet<TEntity>();
                }
                return _dbSet;
            }
        }

        private ParameterExpression _parameterExpression = Expression.Parameter(typeof(TEntity), "t");//定义了一个全局的t，把TEntity传给t  简要意思为 t=>

        private Type _type = typeof(TEntity);

        #region  表达式树动态条件拼接

        /// <summary>
        /// 根据  属性==值  生成节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <param name="binaryType"></param>
        /// <returns></returns>
        public Expression GeExpressionNode<T>(string propertyName, T value, ExpressionType binaryType = ExpressionType.Equal) {
            //错误信息：（温习查看）没有为类型“System.String”定义属性“System.String Name” 
            //ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "t");
            //MemberExpression memberExpression = Expression.Property(parameterExpression, _type.GetProperty(propertyName));

            ParameterExpression parameterExpression = Expression.Parameter(typeof(TEntity), "t");
            //名称为propertyName的属性必须应该在参数（_parameterExpression）中
            MemberExpression memberExpression = Expression.Property(_parameterExpression, _type.GetProperty(propertyName));
            ConstantExpression constantExpression = Expression.Constant(value);
            BinaryExpression methodCallExpression = Expression.MakeBinary(binaryType, memberExpression, constantExpression);
            return methodCallExpression;
        }

        /// <summary>
        /// 拼接两个节点
        /// </summary>
        /// <param name="leftExpression"></param>
        /// <param name="rightExpression"></param>
        /// <param name="binaryType"></param>
        /// <returns></returns>
        public Expression CombineExpression(Expression leftExpression, Expression rightExpression, ExpressionType binaryType = ExpressionType.AndAlso) {
            BinaryExpression methodCallExpression = Expression.MakeBinary(binaryType, leftExpression, rightExpression);
            return methodCallExpression;
        }

        /// <summary>
        /// 生成表达式树
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public Expression<Func<TEntity, bool>> LamdaExpression(Expression expression) {
            return Expression.Lambda<Func<TEntity, bool>>(expression, _parameterExpression);
        }

        #endregion

    }
}
