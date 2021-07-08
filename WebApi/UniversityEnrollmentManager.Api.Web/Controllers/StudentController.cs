using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniversityEnrollmentManager.Core.Enrollments.RequestHandlers;
using UniversityEnrollmentManager.DomainModel.Student;
using UniversityEnrollmentManager.Utils;

namespace UniversityEnrollmentManager.Api.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : BaseController
    {
        public StudentController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("Insert")]
        [ProducesResponseType(typeof(EntityResponseModel<StudentReadModel>), 200)]
        public async Task<IActionResult> Insert(CancellationToken cancellationToken, StudentCreateModel model)
        {
            var returnResponse = new EntityResponseModel<StudentReadModel>();
            try
            {
                var query = new EntityRequestModel<StudentCreateModel, EntityResponseModel<StudentReadModel>>(model);
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
        [ProducesResponseType(typeof(EntityResponseModel<StudentReadModel>), 200)]
        public async Task<IActionResult> GetById(CancellationToken cancellationToken, int Id)
        {
            var returnResponse = new EntityResponseModel<StudentReadModel>();
            try
            {
                var query = new EntityIdentifierQuery<int, EntityResponseModel<StudentReadModel>>(User, Id);
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
