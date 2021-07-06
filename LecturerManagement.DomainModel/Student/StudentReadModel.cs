using LecturerManagement.Utility.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LecturerManagement.DomainModel.Student
{
    public class StudentReadModel : EntityReadModel<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
