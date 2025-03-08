using DataAccess.Data.Repository.GenericRepo;
using System.Linq.Expressions;
using Monitor = Models.Monitor;

namespace DataAccess.Data.Repository.MonitorRepo
{
	public interface IMonitorRepository : IGenericRepository<Monitor>
	{
        Task<List<Monitor>> GetMonitorsForGroepsreisAsync(int groepsreisId);
        Task<Monitor> GetAsync(Func<Monitor, bool> predicate);
        Task<IEnumerable<Monitor>> GetMonitoredGroepsreizenForUserAsync(string userId);
        Task<IEnumerable<Monitor>> GetMonitorsByUserIdAsync(string userId);
        Task DeleteMonitorsByUserIdAsync(string userId);
        Task<Monitor> FindAsync(Expression<Func<Monitor, bool>> predicate);
    }
}
