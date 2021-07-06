using UniversityEnrollmentManager.Utils;

namespace UniversityEnrollmentManager.DomainModel.LectureTheatre
{
    public class LectureTheatreReadModel : EntityReadModel<int>
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public int SeatingCapacity { get; set; }
    }
}
