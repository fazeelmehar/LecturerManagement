using System;
using System.Collections.Generic;
using System.Text;

namespace LecturerManagement.Domain.Entities
{
    public class Subject : Entity<int>
    {
        public string Name { get; set; }
        public ICollection<Lecture> Lectures { get; set; }
    }
}