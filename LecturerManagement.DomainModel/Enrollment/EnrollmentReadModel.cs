using LecturerManagement.DomainModel.Lecture;
using LecturerManagement.DomainModel.Student;
using LecturerManagement.Utility.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LecturerManagement.DomainModel.Enrollment
{
    public class EnrollmentReadModel :  EntityReadModel<int>
    {
        public StudentReadModel Student { get; set; }
        public LectureReadModel Lecture { get; set; }
    }
}
