using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniversityEnrollmentManager.Core.Enrollments.RequestHandlers;
using UniversityEnrollmentManager.DomainModel.LectureTheatre;
using UniversityEnrollmentManager.Utils;

namespace UniversityEnrollmentManager.Api.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LectureTheatreController : BaseController
    {
        public LectureTheatreController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("Insert")]
        [ProducesResponseType(typeof(EntityResponseModel<LectureTheatreReadModel>), 200)]
        public async Task<IActionResult> Insert(CancellationToken cancellationToken, LectureTheatreCreateModel model)
        {
            var returnResponse = new EntityResponseModel<LectureTheatreReadModel>();
            try
            {
                var query = new EntityRequestModel<LectureTheatreCreateModel, EntityResponseModel<LectureTheatreReadModel>>(model);
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
        [ProducesResponseType(typeof(EntityResponseModel<LectureTheatreReadModel>), 200)]
        public async Task<IActionResult> GetById(CancellationToken cancellationToken, int Id)
        {
            var returnResponse = new EntityResponseModel<LectureTheatreReadModel>();
            try
            {
                var query = new EntityIdentifierQuery<int, EntityResponseModel<LectureTheatreReadModel>>(User, Id);
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