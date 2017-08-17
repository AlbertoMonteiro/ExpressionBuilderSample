using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionBuilderSample
{
    internal sealed class ParameterReplaceVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression _parameterExpression;

        public ParameterReplaceVisitor(ParameterExpression parameterExpression)
        {
            this._parameterExpression = parameterExpression;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression is ParameterExpression parameterExpression)
            {
                if (parameterExpression.Type == _parameterExpression.Type)
                    return Expression.Property(_parameterExpression, (PropertyInfo)node.Member);
            }
            if (node.Expression is MemberExpression mExp)
            {
                return Expression.Property(VisitMember(mExp), (PropertyInfo)node.Member);
            }

            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var expression = node.Arguments[0];
            if (expression is ParameterExpression parameterExpression)
            {
                if (parameterExpression.Type == _parameterExpression.Type)
                    return Expression.Call(null, node.Method, new[] { _parameterExpression }.Concat(node.Arguments.Skip(1)));
            }
            if (expression is MemberExpression mExp)
                return VisitMember(mExp);

            return node;
        }
    }
}
