using System;

namespace LecturerManagement.Utility.Interface
{
    public interface ITrackUpdated
    {
        DateTimeOffset? Updated { get; set; }
        string UpdatedBy { get; set; }
    }
}
