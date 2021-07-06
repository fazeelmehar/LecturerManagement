using MediatR;
using System.Security.Principal;

namespace UniversityEnrollmentManager.Core.CQRS.QueryTypes
{
    public abstract class PrincipalQueryBase<TResponse> : IRequest<TResponse>
    {
        protected PrincipalQueryBase(IPrincipal principal)
        {
            Principal = principal;
        }

        public IPrincipal Principal { get; set; }
    }
}
