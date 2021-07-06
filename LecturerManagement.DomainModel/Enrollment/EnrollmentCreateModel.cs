using System;
using System.Collections.Generic;
using System.Text;

namespace LecturerManagement.DomainModel.Enrollment
{
    public class EnrollmentCreateModel
    {
        public int? StudentId { get; set; }
        public int? LectureId { get; set; }
    }
}
