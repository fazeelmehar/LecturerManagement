using UniversityEnrollmentManager.DomainModel.Student;
using UniversityEnrollmentManager.DomainModel.Subject;
using UniversityEnrollmentManager.Utils;

namespace UniversityEnrollmentManager.DomainModel.Enrollment
{
    public class EnrollmentReadModel : EntityReadModel<int>
    {
        public StudentReadModel Student { get; set; }
        public SubjectReadModel Subject { get; set; }
    }
}
