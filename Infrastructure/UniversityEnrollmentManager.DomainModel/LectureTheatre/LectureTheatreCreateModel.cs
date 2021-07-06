using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityEnrollmentManager.DomainModel.LectureTheatre
{
    public class LectureTheatreCreateModel
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public int SeatingCapacity { get; set; }
    }
}
