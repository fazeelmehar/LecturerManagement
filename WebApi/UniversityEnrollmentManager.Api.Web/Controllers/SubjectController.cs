using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using UniversityEnrollmentManager.Core.Enrollments.RequestHandlers;
using UniversityEnrollmentManager.DomainModel.Subject;
using UniversityEnrollmentManager.Utils;

namespace UniversityEnrollmentManager.Api.Web.Controllers
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

                if (!result.ReturnStatus)
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
                var query = new EntityIdentifierQuery<int, EntityResponseModel<SubjectReadModel>>(User, Id);
                var result = await Mediator.Send(query, cancellationToken).ConfigureAwait(false);

                if (!result.ReturnStatus)
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

        [HttpPost("GetAllStudentsEnrolledInSubject")]
        [ProducesResponseType(typeof(EntityResponseListModel<SubjectEnrollmentsReadModel>), 200)]
        public async Task<IActionResult> GetAllStudentsEnrolledInSubject(CancellationToken cancellationToken, int Id)
        {
            var returnResponse = new EntityResponseListModel<SubjectEnrollmentsReadModel>();
            try
            {
                var query = new EntityIdentifierQuery<int, EntityResponseListModel<SubjectEnrollmentsReadModel>>(User, Id);
                var result = await Mediator.Send(query, cancellationToken).ConfigureAwait(false);

                if (!result.ReturnStatus)
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