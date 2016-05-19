using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Data.EntityFramework {
    public class ParameterConverter : ExpressionVisitor
    {
        protected LambdaExpression SourceExpression { get; set; }
        protected ParameterExpression Parameter { get; set; }
        protected bool UseOuterParameter = false;

        public ParameterConverter(LambdaExpression expression)
        {
            this.SourceExpression = (LambdaExpression)expression;
        }

        public ParameterConverter(LambdaExpression expression, ParameterExpression parameter) : this(expression)
        {
            this.Parameter = parameter;
            this.UseOuterParameter = true;
        }

        public LambdaExpression Replace(Type targetType)
        {
            if (this.SourceExpression == null)
                return null;

            if (!this.UseOuterParameter)
                this.Parameter = Expression.Parameter(targetType, this.SourceExpression.Parameters[0].Name);

            Expression body = this.Visit(this.SourceExpression.Body);
            this.SourceExpression = Expression.Lambda(body, this.Parameter);
            return this.SourceExpression;
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            return this.Parameter;
        }

        protected override Expression VisitMemberAccess(MemberExpression m)
        {
            Expression exp = this.Visit(m.Expression);
            PropertyInfo propertyInfo = m.Member as PropertyInfo;
            return propertyInfo == null ? m : Expression.Property(exp, propertyInfo.Name);
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            Expression left = this.Visit(b.Left);
            Expression right = Calc(b.Right);//对二元运算符右边的表达式进行求值
            Expression conversion = this.Visit(b.Conversion);
            if (b.NodeType == ExpressionType.Coalesce && b.Conversion != null)
                return Expression.Coalesce(left, right, conversion as LambdaExpression);
            else
                return Expression.MakeBinary(b.NodeType, left, right, b.IsLiftedToNull, b.Method);
        }

        protected override ReadOnlyCollection<Expression> VisitExpressionList(
                ReadOnlyCollection<Expression> original)
        {
            if (original == null || original.Count == 0)
                return original;

            //对参数进行求值运算
            List<Expression> list = new List<Expression>();
            for (int i = 0, n = original.Count; i < n; i++)
            {
                list.Add(Calc(original[i])); //对调用函数的输入参数进行求值
            }
            return list.AsReadOnly();
        }

        private static Expression Calc(Expression e)
        {
            LambdaExpression lambda = Expression.Lambda(e);
            return Expression.Constant(lambda.Compile().DynamicInvoke(null), e.Type);
        }
    }
}
