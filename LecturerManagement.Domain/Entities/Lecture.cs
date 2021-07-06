using System;
using System.Collections.Generic;
using System.Text;

namespace LecturerManagement.Domain.Entities
{
    public class Lecture : Entity<int>
    {
        public DateTimeOffset DateTime { get; set; }
        public int? SubjectId { get; set; }
        public int? LectureTheatreId { get; set; }
        public TimeSpan Duration { get; set; }
        public Subject Subject { get; set; }
        public LectureTheatre LectureTheatre { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}