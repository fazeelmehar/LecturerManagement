
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace Intellix.Core.CQRS.Queries
{
    public class EntityIdentifiersQuery<TKey, TReadModel> : PrincipalQueryBase<TReadModel>
    {
        public EntityIdentifiersQuery(IPrincipal principal, IEnumerable<TKey> ids)
            : base(principal)
        {
            Ids = ids.ToList();
        }

        public IReadOnlyCollection<TKey> Ids { get; }       
    }
}

