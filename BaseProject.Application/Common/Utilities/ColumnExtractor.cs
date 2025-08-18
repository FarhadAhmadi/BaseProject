using System.Linq.Expressions;

namespace BaseProject.Application.Common.Utilities
{
    public static class ColumnExtractor
    {
        /// <summary>
        /// Extracts the property names from a selector expression.
        /// Always ensures "Id" is included.
        /// </summary>
        public static List<string> GetSelectedColumns<T, TResult>(Expression<Func<T, TResult>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var columnNames = selector.Body switch
            {
                NewExpression newExpr => newExpr.Arguments.Select(GetMemberName).ToList(),
                MemberInitExpression initExpr => initExpr.Bindings
                    .OfType<MemberAssignment>()
                    .Select(b => GetMemberName(b.Expression))
                    .ToList(),
                MemberExpression memberExpr => new List<string> { GetMemberName(memberExpr) },
                _ => throw new ArgumentException(
                    "Selector must be a valid projection, e.g., x => new DTO { x.Id, x.Name } or x => x.Id",
                    nameof(selector))
            };

            // Ensure "Id" is always included
            if (!columnNames.Contains("Id"))
                columnNames.Insert(0, "Id");

            return columnNames;
        }

        private static string GetMemberName(Expression expression)
        {
            if (expression is MemberExpression memberExpr)
                return memberExpr.Member.Name;

            throw new ArgumentException("Expression must be a member access", nameof(expression));
        }
    }
}
