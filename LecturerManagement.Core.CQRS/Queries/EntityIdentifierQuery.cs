using System.Security.Principal;

namespace Intellix.Core.CQRS.Queries
{
    public class EntityIdentifierQuery<TKey, TReadModel> : PrincipalQueryBase<TReadModel>
    {
        public EntityIdentifierQuery(IPrincipal principal, TKey id)
            : base(principal)
        {
            Id = id;
        }
        public EntityIdentifierQuery(IPrincipal principal, TKey id, string includeProperties)
           : base(principal)
        {
            Id = id;
            IncludeProperties = includeProperties;
        }
        public TKey Id { get; }
        public string IncludeProperties { get; set; }
    }
}
