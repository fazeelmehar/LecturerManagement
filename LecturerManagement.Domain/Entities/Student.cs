using System;
using System.Collections.Generic;
using System.Text;

namespace LecturerManagement.Domain.Entities
{
    public class Student : Entity<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }

    }
}
