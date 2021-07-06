using MediatR;
using System.Security.Principal;

namespace UniversityEnrollmentManager.Core.Enrollments.RequestHandlers
{
    // This could be moved out in to a CQRS style base class library.
    public abstract class PrincipalQueryBase<TResponse> : IRequest<TResponse>
    {
        protected PrincipalQueryBase(IPrincipal principal)
        {
            Principal = principal;
        }

        public IPrincipal Principal { get; set; }
    }
}
