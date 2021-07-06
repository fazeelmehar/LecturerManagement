
using LecturerManagement.Utility.Interface;

namespace LecturerManagement.Utility.Model
{
    public class EntityIdentifierModel<TKey> : IHaveIdentifier<TKey>
    {
        public TKey Id { get; set; }
    }
}
