using System;
using System.Collections.Generic;
using System.Text;
using UniversityEnrollmentManager.DomainModel.Lecture;
using UniversityEnrollmentManager.DomainModel.Student;
using UniversityEnrollmentManager.Utils;

namespace UniversityEnrollmentManager.DomainModel.Enrollment
{
    public class EnrollmentReadModel : EntityReadModel<int>
    {
        public StudentReadModel Student { get; set; }
        public LectureReadModel Lecture { get; set; }
    }
}
