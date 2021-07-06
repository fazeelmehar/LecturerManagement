using System;

namespace LecturerManagement.Utility.Interface
{
   
    public interface ITrackCreated
    {
        DateTimeOffset Created { get; set; }
        string CreatedBy { get; set; }
    }
}
