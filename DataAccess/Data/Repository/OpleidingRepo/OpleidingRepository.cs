using DataAccess.Data.Repository.GenericRepo;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.Data.Repository.OpleidingRepo
{
	public class OpleidingRepository : GenericRepository<Opleiding>, IOpleidingRepository
	{
		public OpleidingRepository(KampigoDbContext context) : base(context)
		{
        }
        public async Task<IEnumerable<Opleiding>> GetAllWithUsersAsync()
        {
            return await _context.Opleidingen.Include(o => o.OpleidingPersonen).ToListAsync();
        }

        public async Task<Opleiding?> GetWithUsersByIdAsync(int id)
        {
            return await _context.Opleidingen.Include(o => o.OpleidingPersonen).FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Opleiding>> GetOpleidingenByUserIdAsync(string userId)
        {
            return await _context.Opleidingen
                .Include(o => o.OpleidingPersonen)
                .Include(o => o.VoorwaardeOpleiding)
                .Include(o => o.OpleidingPersonen)
                .Where(o => o.OpleidingPersonen.Any(op => op.GebruikerId == userId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Opleiding>> GetRecommendedOpleidingenAsync(string userId, DateTime currentDate)
        {
            var userCompletedOpleidingen = await _context.Opleidingen
                .Where(o => o.OpleidingPersonen.Any(op => op.GebruikerId == userId && o.EindDatum < currentDate))
                .Select(o => o.Id)
                .ToListAsync();

            var userSubscribedOpleidingen = await _context.Opleidingen
                .Where(o => o.OpleidingPersonen.Any(op => op.GebruikerId == userId))
                .Select(o => o.Id)
                .ToListAsync();

            return await _context.Opleidingen
                .Where(o => o.AantalPlaatsen > 0 && o.EindDatum > currentDate && o.OpleidingVereist.HasValue && userCompletedOpleidingen.Contains(o.OpleidingVereist.Value) && !userSubscribedOpleidingen.Contains(o.Id))
                .OrderBy(o => o.Naam)
                .ToListAsync();
        }

        public async Task<IEnumerable<Opleiding>> GetOtherOpleidingenAsync(string userId, DateTime currentDate)
        {
            var userSubscribedOpleidingen = await _context.Opleidingen
                .Where(o => o.OpleidingPersonen.Any(op => op.GebruikerId == userId))
                .Select(o => o.Id)
                .ToListAsync();

            return await _context.Opleidingen
                .Include(o => o.OpleidingPersonen)
                .Include(o => o.VoorwaardeOpleiding)
                .Where(o => o.AantalPlaatsen > 0 && o.EindDatum > currentDate && !userSubscribedOpleidingen.Contains(o.Id))
                .ToListAsync();
        }
        public async Task<Opleiding?> GetOpleidingWithUsersByIdAsync(int id)
        {
            return await _context.Opleidingen
                .Include(o => o.OpleidingPersonen)
                .ThenInclude(op => op.Gebruiker)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<CustomUser>> GetIngeschrevenMonitorsAsync(int opleidingId)
        {
            return await _context.OpleidingPersonen
                .Where(op => op.OpleidingId == opleidingId && !op.IsGeaccepteerd)
                .Select(op => op.Gebruiker!)
                .ToListAsync();
        }

        public async Task<List<CustomUser>> GetGeaccepteerdeMonitorsAsync(int opleidingId)
        {
            return await _context.OpleidingPersonen
                .Where(op => op.OpleidingId == opleidingId && op.IsGeaccepteerd)
                .Select(op => op.Gebruiker!)
                .ToListAsync();
        }
    }
}
