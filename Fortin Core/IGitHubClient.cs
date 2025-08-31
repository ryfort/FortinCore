namespace Fortin.API
{
    public interface IGitHubClient
    {
        Task<int> GetFollowersCount();
    }
}
