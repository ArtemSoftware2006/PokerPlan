namespace Service.Interfaces
{
    public interface ILinkService
    {
        Task<string> GenerateLink(Guid groupId);
    }
}