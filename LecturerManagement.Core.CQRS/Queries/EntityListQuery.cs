using System.Collections.Generic;
using System.Security.Principal;
namespace LecturerManagement.Core.CQRS.Queries

{
    public class EntityListQuery<TReadModel> : PrincipalQueryBase<TReadModel>
       
    {
        public EntityListQuery(IPrincipal principal )
            : this(principal, null, (IEnumerable<EntitySort>)null)
        {
        }

        public EntityListQuery(IPrincipal principal, EntityFilter filter)
            : this(principal, filter, (IEnumerable<EntitySort>)null)
        {
        }
        public EntityListQuery(IPrincipal principal, EntityFilter filter,string includeProperties)
           : this(principal, filter, (IEnumerable<EntitySort>)null)
        {
            this.IncludeProperties = includeProperties;
        }

        public EntityListQuery(IPrincipal principal, EntityFilter filter, EntitySort sort)
            : this(principal, filter, new[] { sort })
        {
        }

        public EntityListQuery(IPrincipal principal, EntityFilter filter, IEnumerable<EntitySort> sort)
            : base(principal)
        {
            Filter = filter;
            Sort = sort;           
        }
        public EntityFilter Filter { get; set; }
        public IEnumerable<EntitySort> Sort { get; set; }
        public string IncludeProperties { get; set; }

    }
}
