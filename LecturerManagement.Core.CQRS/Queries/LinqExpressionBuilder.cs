using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LecturerManagement.Core.CQRS.Queries
{
    public class LinqExpressionBuilder
    {
        private static readonly IDictionary<string, string> _operatorMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { EntityFilterOperators.Equal, "==" },
            { EntityFilterOperators.NotEqual, "!=" },
            { EntityFilterOperators.LessThan, "<" },
            { EntityFilterOperators.LessThanOrEqual, "<=" },
            { EntityFilterOperators.GreaterThan, ">" },
            { EntityFilterOperators.GreaterThanOrEqual, ">=" }
        };
        private readonly StringBuilder _expression = new StringBuilder();
        private readonly List<object> _values = new List<object>();
        public IReadOnlyList<object> Parameters => _values;
        public string Expression => _expression.ToString();
        public void Build<T>(EntityFilter entityFilter, T entity) where T : class
        {
            _expression.Length = 0;
            _values.Clear();
            if (entityFilter == null)
                return;
            Visit(entityFilter, entity);
        }

        private void Visit<T>(EntityFilter entityFilter, T entity) where T : class
        {
            WriteExpression(entityFilter, entity);

            WriteGroup(entityFilter, entity);
        }

        private void WriteExpression<T>(EntityFilter entityFilter, T entity) where T : class
        {

            if (string.IsNullOrWhiteSpace(entityFilter.Name))
                return;

            var properties = entityFilter.Name.Split(".");
            //if (IsPropertyIsCollection(properties, entity))
            //{
            var entityName = entity.GetType().FullName;
            int count = 1;
            int parenthesesCount = 0;
            var lastProperty = properties.LastOrDefault();
            StringBuilder expression = new StringBuilder();
            foreach (var propertyName in properties)
            {
                var propertyInfo = GetPropertyInfo(entityName, propertyName);
                if (typeof(ICollection<>).Name.Equals(propertyInfo.PropertyType.Name,
                    StringComparison.CurrentCultureIgnoreCase))
                {
                    entityName = propertyInfo.PropertyType.FullName.Split("[[")[1].Split(",")[0];
                    expression.Append($"{propertyName}.Any(x{count} => x{count}");

                    parenthesesCount++;
                }
                else
                {
                    entityName = propertyInfo.PropertyType.FullName;
                    expression.Append($"{propertyName}");
                }

                if (!lastProperty.Equals(propertyName, StringComparison.CurrentCultureIgnoreCase))
                    expression.Append($".");
                else
                {
                    entityFilter.Name = expression.ToString();
                    PropertyExpression(entityFilter);
                }
                count++;
            }
            for (int i = 0; i < parenthesesCount; i++)
            {
                _expression.Append(")");
            }
            //}
            //else
            // PropertyExpression(entityFilter);
        }

        private void PropertyExpression(EntityFilter entityFilter)
        {
            var index = _values.Count;
            var name = entityFilter.Name;
            var value = entityFilter.Value;
            var o = string.IsNullOrWhiteSpace(entityFilter.Operator) ? "==" : entityFilter.Operator;
            _operatorMap.TryGetValue(o, out string comparison);
            if (string.IsNullOrEmpty(comparison))
                comparison = o.Trim();
            var negation = comparison.StartsWith("!") || comparison.StartsWith("not", StringComparison.OrdinalIgnoreCase) ? "!" : string.Empty;
            if (comparison.EndsWith(EntityFilterOperators.StartsWith, StringComparison.OrdinalIgnoreCase))
                _expression.Append($"{negation}{name}.StartsWith(@{index})");
            else if (comparison.EndsWith(EntityFilterOperators.EndsWith, StringComparison.OrdinalIgnoreCase))
                _expression.Append($"{negation}{name}.EndsWith(@{index})");
            else if (comparison.EndsWith(EntityFilterOperators.Contains, StringComparison.OrdinalIgnoreCase))
                _expression.Append($"{negation}{name}.Contains(@{index})");
            else
                _expression.Append($"{name} {comparison} @{index}");
            _values.Add(value);
        }
        private PropertyInfo GetPropertyInfo(string entityName, string propertyName)
        {
            string entityDomain = "LecturerManagement.Domain";
            return Type.GetType($"{entityName},{entityDomain}").GetProperties()
                        .FirstOrDefault(x => x.Name.Equals(propertyName,StringComparison.CurrentCultureIgnoreCase));
        }

        private bool IsPropertyIsCollection<T>(string[] nameArray, T Tentity) where T : class
        {
            var entityName = Tentity.GetType().FullName;

            foreach (var name in nameArray)
            {
                if (typeof(ICollection<>).Name.Equals(GetPropertyInfo(entityName, name).PropertyType.Name,
                    StringComparison.CurrentCultureIgnoreCase))
                    return true;
                else
                    entityName = GetPropertyInfo(entityName, name).PropertyType.FullName;
            }
            return false;
        }

        private bool WriteGroup<T>(EntityFilter entityFilter, T entity) where T : class
        {
            var hasGroup = entityFilter.Filters != null && entityFilter.Filters.Any();
            if (!hasGroup)
                return false;

            if (!string.IsNullOrWhiteSpace(entityFilter.Name))
            {
                var logic = string.IsNullOrWhiteSpace(entityFilter.Logic)
                    ? EntityFilterLogic.And
                    : entityFilter.Logic;
                _expression.Append($" {logic} (");
            }
            else
                _expression.Append("(");

            foreach (var filter in entityFilter.Filters)
            {
                Visit(filter, entity);

                var isLast = entityFilter.Filters.LastOrDefault() == filter;
                if (filter == null) continue;
                var filterLogic = string.IsNullOrWhiteSpace(filter.Logic)
                    ? EntityFilterLogic.And
                    : filter.Logic;
                if (!isLast)
                    _expression.Append($" {filterLogic} ");
            }
            _expression.Append(")");
            return true;
        }
    }
}
