using System;
using System.Threading;
using System.Threading.Tasks;
using LecturerManagement.Core.CQRS.Model;
using LecturerManagement.DomainModel.Subject;
using LecturerManagement.Utility.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace LecturerManagement.Api.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : BaseController
    {
        public SubjectController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("Insert")]
        [ProducesResponseType(typeof(EntityResponseModel<SubjectReadModel>), 200)]
        public async Task<IActionResult> Insert(CancellationToken cancellationToken, SubjectCreateModel model)
        {
            var returnResponse = new EntityResponseModel<SubjectReadModel>();
            try
            {
                var query = new EntityRequestModel<SubjectCreateModel, EntityResponseModel<SubjectReadModel>>(model);
                var result = await Mediator.Send(query, cancellationToken).ConfigureAwait(false);
                if (result.ReturnStatus == false)
                    return BadRequest(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                returnResponse.ReturnStatus = false;
                returnResponse.ReturnMessage.Add(ex.Message);
                return BadRequest(returnResponse);
            }
        }

        [HttpPost("GetById")]
        [ProducesResponseType(typeof(EntityResponseModel<SubjectReadModel>), 200)]
        public async Task<IActionResult> GetById(CancellationToken cancellationToken, int Id)
        {
            var returnResponse = new EntityResponseModel<SubjectReadModel>();
            try
            {
                var query = new Intellix.Core.CQRS.Queries.EntityIdentifierQuery<int, EntityResponseModel<SubjectReadModel>>(User, Id);
                var result = await Mediator.Send(query, cancellationToken).ConfigureAwait(false);
                if (result.ReturnStatus == false)
                    return BadRequest(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                returnResponse.ReturnStatus = false;
                returnResponse.ReturnMessage.Add(ex.Message);
                return BadRequest(returnResponse);
            }
        }
    }
}