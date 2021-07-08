using System;
using System.Collections.Generic;
using System.Text;
using UniversityEnrollmentManager.DomainModel.Student;
using UniversityEnrollmentManager.Utils;

namespace UniversityEnrollmentManager.DomainModel.Subject
{
    public class SubjectEnrollmentsReadModel : EntityReadModel<int>
    {
        public string Name { get; set; }
        public List<StudentReadModel> Students { get; set; }
    }
}
