using LecturerManagement.Utility.Interface;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LecturerManagement.Domain.Entities
{
    //base entity model
    public abstract class Entity<TKey>: IHaveIdentifier<TKey>, ITrackDeleted
    {
       
        [Column(Order = 1)]
        public TKey Id { get; set; }
        [Column(Order = 4)]
        public bool IsDeleted { get; set; }
    }
}
