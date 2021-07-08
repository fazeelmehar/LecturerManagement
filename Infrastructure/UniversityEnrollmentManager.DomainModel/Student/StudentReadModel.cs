using UniversityEnrollmentManager.DomainModel.Enrollment;
using UniversityEnrollmentManager.Utils;

namespace UniversityEnrollmentManager.DomainModel.Student
{
    public class StudentReadModel : EntityReadModel<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public EnrollmentReadModel Enrollments { get; set; }
    }
}
