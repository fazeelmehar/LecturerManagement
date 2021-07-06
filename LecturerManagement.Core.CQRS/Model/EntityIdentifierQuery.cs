using MediatR;
using System;

namespace LecturerManagement.Core.CQRS.Model
{
    public class EntityIdentifierQuery<TKey, TReadModel> : IRequest<TReadModel>
    {
        public EntityIdentifierQuery(TKey id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            Id = id;
        }
        public TKey Id { get; set; }
    }
}
