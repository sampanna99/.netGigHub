using GigHub.Models;
using System.Linq;

namespace GigHub.Repositories
{
    public class FollowingRepository : IFollowingRepository
    {
        private readonly ApplicationDbContext _context;
        public FollowingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool GetFollowing(string id, string userId)
        {
            return _context.Followings.Any(f => f.FolloweeId == id && f.FollowerId == userId);
        }

    }
}