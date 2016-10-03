namespace GigHub.Repositories
{
    public interface IFollowingRepository
    {
        bool GetFollowing(string id, string userId);
    }
}