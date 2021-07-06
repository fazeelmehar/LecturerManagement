using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using UniversityEnrollmentManager.Utils;

namespace UniversityEnrollmentManager.Api.Web
{
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ErrorModel), 422)]
    [ProducesResponseType(typeof(ErrorModel), 500)]
    public abstract class BaseController : ControllerBase
    {
        protected BaseController(IMediator mediator)
        {
            Mediator = mediator;
        }
        public IMediator Mediator { get; }
        protected virtual string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        protected virtual Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".mp4", "video/mp4" }
            };
        }
    }
}
