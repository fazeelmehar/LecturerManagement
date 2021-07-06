using System;
using System.Collections.Generic;
using System.Text;
using UniversityEnrollmentManager.Utils;

namespace UniversityEnrollmentManager.DomainModel.Subject
{
    public class SubjectReadModel : EntityReadModel<int>
    {
        public string Name { get; set; }
    }
}
