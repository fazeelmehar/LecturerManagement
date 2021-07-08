using System;
using System.Collections.Generic;
using System.Text;
using UniversityEnrollmentManager.DomainModel.Student;
using UniversityEnrollmentManager.Utils;

namespace UniversityEnrollmentManager.DomainModel.Enrollment
{
    public class EnrollmentStudentsReadModel : EntityReadModel<int>
    {
        public StudentReadModel Student { get; set; }

    }
}
