using MediatR;
using System;

namespace LecturerManagement.Core.CQRS.Model
{
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
