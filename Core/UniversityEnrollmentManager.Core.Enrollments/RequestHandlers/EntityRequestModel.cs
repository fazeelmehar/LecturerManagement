using MediatR;
using System;

namespace UniversityEnrollmentManager.Core.Enrollments.RequestHandlers
{
    // This could be moved out in to a CQRS style base class library.
    public class EntityRequestModel<TCreateModel, TReadModel> : IRequest<TReadModel>
    {
        public EntityRequestModel(TCreateModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            Model = model;
        }
        public TCreateModel Model { get; set; }
    }
}
