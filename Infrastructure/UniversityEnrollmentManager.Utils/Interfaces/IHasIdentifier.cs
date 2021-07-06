namespace UniversityEnrollmentManager.Utils.Interfaces
{
    public interface IWithIdentifier<TKey>
    {
        TKey Id { get; set; }
    }
}
