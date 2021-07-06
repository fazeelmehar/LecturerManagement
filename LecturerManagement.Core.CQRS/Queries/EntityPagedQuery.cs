using System.Security.Principal;
namespace LecturerManagement.Core.CQRS.Queries
{
    public class EntityPagedQuery<TReadModel> : PrincipalQueryBase<TReadModel>
    {
        public EntityPagedQuery(IPrincipal principal, EntityQuery query)
            : base(principal)
        {
            Query = query;
        }
        public EntityPagedQuery(IPrincipal principal, EntityQuery query, string includeProperties)
          : base(principal)
        {
            Query = query;
            IncludeProperties = includeProperties;
        }
        public EntityQuery Query { get; }
        public string IncludeProperties { get; set; }
    }
}
