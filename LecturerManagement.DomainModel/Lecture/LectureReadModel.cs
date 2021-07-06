using System;
using LecturerManagement.DomainModel.LectureTheatre;
using LecturerManagement.DomainModel.Subject;
using LecturerManagement.Utility.Model;

namespace LecturerManagement.DomainModel.Lecture
{
    public class LectureReadModel : EntityReadModel<int>
    {
        public DateTimeOffset DateTime { get; set; }
        public int? SubjectId { get; set; }
        public int? LectureTheatreId { get; set; }
        public TimeSpan Duration { get; set; }
        public SubjectReadModel Subject { get; set; }
        public LectureTheatreReadModel LectureTheatre { get; set; }
    }
}