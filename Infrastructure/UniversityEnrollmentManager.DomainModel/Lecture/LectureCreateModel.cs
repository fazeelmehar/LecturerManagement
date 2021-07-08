﻿using System;

namespace UniversityEnrollmentManager.DomainModel.Lecture
{
    public class LectureCreateModel
    {
        public DateTime DateTime { get; set; }
        public int? SubjectId { get; set; }
        public int? LectureTheatreId { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
