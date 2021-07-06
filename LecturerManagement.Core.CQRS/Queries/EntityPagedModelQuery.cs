using System.Security.Principal;
namespace LecturerManagement.Core.CQRS.Queries
{
    public class EntityPagedModelQuery<TFilterModel,TReadModel> : PrincipalQueryBase<EntityPagedResult<TReadModel>>
    {
        public EntityPagedModelQuery(IPrincipal principal, TFilterModel filterModel)
            : base(principal)
        {
            FilterModel = filterModel;
        }
        public TFilterModel FilterModel { get; }       
    }
}
