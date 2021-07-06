using LecturerManagement.Utility.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LecturerManagement.DomainModel.Subject
{
    public class SubjectReadModel : EntityReadModel<int>
    {
        public string Name { get; set; }
    }
}
