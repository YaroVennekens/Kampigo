using DataAccess.Data.Repository.GenericRepo;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Linq.Expressions;
using Monitor = Models.Monitor;

namespace DataAccess.Data.Repository.MonitorRepo
{
	public class MonitorRepository : GenericRepository<Monitor>, IMonitorRepository
	{
		public MonitorRepository(KampigoDbContext context) : base(context)
		{
		}

        public async Task<List<Monitor>> GetMonitorsForGroepsreisAsync(int groepsreisId)
        {
            return await _context.Monitors
                .Where(m => m.GroepsreisDetailsId == groepsreisId)
                .ToListAsync();
        }

        public async Task<Monitor> GetAsync(Func<Monitor, bool> predicate)
        {
            return _context.Monitors
           .Where(predicate)
           .FirstOrDefault();
        }

        // Fetch all Monitors with groepsreis and bestemming data where user signed up for as monitor
        public async Task<IEnumerable<Monitor>> GetMonitoredGroepsreizenForUserAsync(string userId)
        {
            var monitoredGroepsreizen = await _context.Monitors
                .Include(m => m.Groepsreis)
                .ThenInclude(g => g!.Bestemming)
                .Where(m => m.GebruikerId == userId)
                .ToListAsync();

            return monitoredGroepsreizen;
        }


        public async Task<IEnumerable<Monitor>> GetMonitorsByUserIdAsync(string userId)
        {
            return await _context.Monitors
                .Where(m => m.GebruikerId == userId)
                .ToListAsync();
        }

        public async Task DeleteMonitorsByUserIdAsync(string userId)
        {
            var monitors = await GetMonitorsByUserIdAsync(userId);
            _context.Monitors.RemoveRange(monitors);
            await _context.SaveChangesAsync();
        }

        public async Task<Monitor> FindAsync(Expression<Func<Monitor, bool>> predicate)
        {
            return await _context.Monitors.FirstOrDefaultAsync(predicate);
        }
    }
}
