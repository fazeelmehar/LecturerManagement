using System.Collections.Generic;

namespace UniversityEnrollmentManager.Domain.Entities
{
    public class LectureTheatre : Entity<int>
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public int SeatingCapacity { get; set; }
        public ICollection<Lecture> Lectures { get; set; }
    }
}
