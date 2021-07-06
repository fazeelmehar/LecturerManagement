using Microsoft.AspNetCore.Http;

namespace LecturerManagement.Data.Interface
{
    public interface IEntityConfiguration
    {
        void Configure(IHttpContextAccessor httpContextAccessor);
    }
}
