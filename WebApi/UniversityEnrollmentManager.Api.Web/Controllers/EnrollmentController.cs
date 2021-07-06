using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniversityEnrollmentManager.Core.Enrollments.RequestHandlers;
using UniversityEnrollmentManager.DomainModel.Enrollment;
using UniversityEnrollmentManager.Utils;

namespace UniversityEnrollmentManager.Api.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : BaseController
    {
        public EnrollmentController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("Insert")]
        [ProducesResponseType(typeof(EntityResponseModel<EnrollmentReadModel>), 200)]
        public async Task<IActionResult> Insert(CancellationToken cancellationToken, EnrollmentCreateModel model)
        {
            var returnResponse = new EntityResponseModel<EnrollmentReadModel>();
            try
            {
                var query = new EntityRequestModel<EnrollmentCreateModel, EntityResponseModel<EnrollmentReadModel>>(model);
                var result = await Mediator.Send(query, cancellationToken).ConfigureAwait(false);

                if (result.ReturnStatus == false)
                {
                    return BadRequest(result);
                }
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
        [ProducesResponseType(typeof(EntityResponseModel<EnrollmentReadModel>), 200)]
        public async Task<IActionResult> GetById(CancellationToken cancellationToken, int Id)
        {
            var returnResponse = new EntityResponseModel<EnrollmentReadModel>();
            try
            {
                var query = new EntityIdentifierQuery<int, EntityResponseModel<EnrollmentReadModel>>(User, Id);
                var result = await Mediator.Send(query, cancellationToken).ConfigureAwait(false);

                if (result.ReturnStatus == false)
                {
                    return BadRequest(result);
                }

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