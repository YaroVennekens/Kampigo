using DataAccess.Data.Repository.GenericRepo;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.Data.Repository.CustomUserRepo
{
	public class CustomUserRepository : GenericRepository<CustomUser>, ICustomUserRepository
	{
		public CustomUserRepository(KampigoDbContext context) : base(context)
		{
		}
        public async Task<CustomUser?> GetMonitorWithGroepsreizenAsync(string id)
        {
            var monitor = await _context.Users.OfType<CustomUser>()
                .Include(u => u.Monitors!)
                    .ThenInclude(m => m.Groepsreis)
                        .ThenInclude(gr => gr!.Bestemming!)
                .Include(u => u.OpleidingPersonen!)
                    .ThenInclude(op => op.Opleiding)
                .FirstOrDefaultAsync(u => u.Id == id);

            return monitor;
        }
        public async Task<List<CustomUser>> GetAllMonitorsAsync()
        {
            return await _context.Users
                .OfType<CustomUser>()
                .Join(_context.UserRoles, user => user.Id, userRole => userRole.UserId, (user, userRole) => new { user, userRole })
                .Join(_context.Roles, userRole => userRole.userRole.RoleId, role => role.Id, (userRole, role) => new { userRole.user, role })
                .Where(ur => ur.role.Name!.ToLower() == "monitor")
                .Select(ur => ur.user)
                .ToListAsync();
        }

        public async Task<List<CustomUser>> GetMonitorsNotInOpleidingAsync(int opleidingId)
        {
            var monitorsInOpleiding = await _context.OpleidingPersonen
                .Where(op => op.OpleidingId == opleidingId)
                .Select(op => op.GebruikerId)
                .ToListAsync();

            return await _context.Users
                .OfType<CustomUser>()
                .Join(_context.UserRoles, user => user.Id, userRole => userRole.UserId, (user, userRole) => new { user, userRole })
                .Join(_context.Roles, userRole => userRole.userRole.RoleId, role => role.Id, (userRole, role) => new { userRole.user, role })
                .Where(ur => ur.role.Name!.ToLower() == "monitor" && !monitorsInOpleiding.Contains(ur.user.Id))
                .Select(ur => ur.user)
                .ToListAsync();
        }
    }
}
