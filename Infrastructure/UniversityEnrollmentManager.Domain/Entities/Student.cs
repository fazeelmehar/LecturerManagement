using System.Collections.Generic;

namespace UniversityEnrollmentManager.Domain.Entities
{
    public class Student : Entity<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }

    }
}
