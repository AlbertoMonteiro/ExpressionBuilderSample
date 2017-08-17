using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionBuilderSample
{
    public class QueryBuilder<T1, T2>
    {
        public readonly List<Expression> Expressions = new List<Expression>();
        public readonly List<PropertyInfo> Properties = new List<PropertyInfo>();
        private ParameterReplaceVisitor _parameterReplaceVisitor;
        private ParameterExpression _parameterExpression;

        public QueryBuilder()
        {
            _parameterExpression = Expression.Parameter(typeof(T1), "p");
            _parameterReplaceVisitor = new ParameterReplaceVisitor(_parameterExpression);
        }

        public Expression<Func<T1, T2>> Build()
        {
            var constructorInfo = typeof(T2).GetConstructors()[0];
            Expression exp = Expression.New(constructorInfo, Expressions, Properties);

            return Expression.Lambda<Func<T1, T2>>(exp, new[] { _parameterExpression });
        }

        public QueryBuilder<T1, T2> SetProperty<TResult>(Expression<Func<T1, TResult>> source, Expression<Func<T2, TResult>> destination)
        {
            Expressions.Add(_parameterReplaceVisitor.Visit(source.Body));
            Properties.Add(GetProperty(destination));
            return this;
        }

        private static PropertyInfo GetProperty(Expression exp)
        {
            if (exp is LambdaExpression lb && lb.Body is MemberExpression memberExpression && memberExpression.Member is PropertyInfo pp)
            {
                return pp;
            }
            return null;
        }

    }
}