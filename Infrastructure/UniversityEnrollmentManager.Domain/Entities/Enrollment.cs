namespace UniversityEnrollmentManager.Domain.Entities
{
    public class Enrollment : Entity<int>
    {
        public int? StudentId { get; set; }
        public int? SubjectId { get; set; }
        public Student Student { get; set; }
        public Subject Subject { get; set; }
    }
}
