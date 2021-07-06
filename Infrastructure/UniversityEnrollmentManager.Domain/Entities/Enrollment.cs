namespace UniversityEnrollmentManager.Domain.Entities
{
    public class Enrollment : Entity<int>
    {
        public int? StudentId { get; set; }
        public int? LectureId { get; set; }
        public Student Student { get; set; }
        public Lecture Lecture { get; set; }
    }
}
