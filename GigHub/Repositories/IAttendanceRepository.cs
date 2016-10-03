using GigHub.Models;
using System.Collections.Generic;

namespace GigHub.Repositories
{
    public interface IAttendanceRepository
    {
        bool GetAttendance(int id, string userId);
        IEnumerable<Attendance> GetFutureAttendances(string userId);
    }
}