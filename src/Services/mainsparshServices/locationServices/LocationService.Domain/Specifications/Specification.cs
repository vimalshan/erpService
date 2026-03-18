using System.Linq.Expressions;
using LocationService.Domain.Entities;

namespace LocationService.Domain.Specifications
{
    /// <summary>
    /// Base specification class for building queries
    /// </summary>
    public abstract class Specification<T> where T : Entity
    {
        public IQueryable<T>? Query { get; set; }
        public List<string> Includes { get; } = new();
        public List<string> IncludeStrings { get; } = new();
        public Expression<Func<T, bool>>? Criteria { get; set; }
        public Expression<Func<T, object>>? OrderBy { get; set; }
        public Expression<Func<T, object>>? OrderByDescending { get; set; }
        public int? Take { get; set; }
        public int? Skip { get; set; }
        public bool IsPagingEnabled { get; set; }

        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression.GetPropertyName());
        }

        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }

        protected virtual void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }
    }

    public static class SpecificationExtensions
    {
        public static string GetPropertyName<T>(this Expression<Func<T, object>> expression)
        {
            if (expression.Body is MemberExpression memberExpression)
                return memberExpression.Member.Name;

            if (expression.Body is UnaryExpression unaryExpression && unaryExpression.Operand is MemberExpression unaryMemberExpression)
                return unaryMemberExpression.Member.Name;

            throw new InvalidOperationException("Unable to get property name from expression");
        }
    }
}
