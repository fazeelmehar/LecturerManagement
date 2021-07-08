using System.Collections.Generic;

namespace UniversityEnrollmentManager.Domain.Entities
{
    public class Subject : Entity<int>
    {
        public string Name { get; set; }
        public ICollection<Lecture> Lectures { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
