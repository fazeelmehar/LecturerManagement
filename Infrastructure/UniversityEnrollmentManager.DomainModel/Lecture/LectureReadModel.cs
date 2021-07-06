using System;
using UniversityEnrollmentManager.DomainModel.LectureTheatre;
using UniversityEnrollmentManager.DomainModel.Subject;
using UniversityEnrollmentManager.Utils;

namespace UniversityEnrollmentManager.DomainModel.Lecture
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
