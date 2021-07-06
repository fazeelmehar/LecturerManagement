namespace LecturerManagement.Utility.Interface
{
    public interface IHaveIdentifier<TKey>
    {
        TKey Id { get; set; }
    }
}
