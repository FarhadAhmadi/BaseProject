using System.Linq.Expressions;

namespace BaseProject.Application.Common.Utilities
{
    public static class ExtracterHelper
    {
        public static List<string> ExtractSelectedColumns<T, TResult>(Expression<Func<T, TResult>> selector)
        {
            var selectedColumns = new List<string>();

            if (selector.Body is NewExpression newExpression)
            {
                selectedColumns.AddRange(
                    newExpression.Arguments
                        .Select(arg => GetMemberName(arg))
                );
            }
            else if (selector.Body is MemberInitExpression memberInitExpression)
            {
                selectedColumns.AddRange(
                    memberInitExpression.Bindings
                        .OfType<MemberAssignment>()
                        .Select(b => b.Expression)
                        .Select(expr => GetMemberName(expr))
                );
            }
            else if (selector.Body is MemberExpression memberExpression)
            {
                selectedColumns.Add(GetMemberName(memberExpression));
            }
            else
            {
                throw new ArgumentException("Selector must be a valid projection, such as x => new DTO { x.Id, x.Name } or x => x.Id");
            }

            // Ensure "Id" is always included
            if (!selectedColumns.Contains("Id"))
            {
                selectedColumns.Insert(0, "Id"); // Add "Id" at the beginning
            }

            return selectedColumns;
        }
        private static string GetMemberName(Expression expression)
        {
            if (expression is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }

            throw new ArgumentException("Invalid expression type");
        }
    }
}
