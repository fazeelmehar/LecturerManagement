using System.ComponentModel.DataAnnotations.Schema;
using UniversityEnrollmentManager.Utils.Interfaces;

namespace UniversityEnrollmentManager.Domain.Entities
{
    public abstract class Entity<TKey> : IWithIdentifier<TKey>
    {

        [Column(Order = 1)]
        public TKey Id { get; set; }
    }
}
