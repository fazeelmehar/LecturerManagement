using System;
using System.Collections.Generic;
using System.Text;

namespace LecturerManagement.Domain.Entities
{
    public class LectureTheatre : Entity<int>
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public int SeatingCapacity { get; set; }
        public ICollection<Lecture> Lectures { get; set; }
    }
}
