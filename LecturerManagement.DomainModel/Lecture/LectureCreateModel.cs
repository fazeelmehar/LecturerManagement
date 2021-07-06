using System;
using System.Collections.Generic;
using System.Text;

namespace LecturerManagement.DomainModel.Lecture
{
    public class LectureCreateModel
    {
        public DateTimeOffset DateTime { get; set; }
        public int? SubjectId { get; set; }
        public int? LectureTheatreId { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
