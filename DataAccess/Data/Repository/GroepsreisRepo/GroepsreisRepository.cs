using DataAccess.Data.Repository.GenericRepo;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.Data.Repository.GroepsreisRepo
{
	public class GroepsreisRepository : GenericRepository<Groepsreis>, IGroepsreisRepository
	{
		public GroepsreisRepository(KampigoDbContext context) : base(context)
		{

		}

        public async Task<IEnumerable<Groepsreis>> GetAllWithActivities()
        {
            return await _context.Groepreizen
                .Include(g => g.Bestemming)
                .Include(g => g.Programmas)
                .ThenInclude(p => p.Activiteit)
                .ToListAsync();
        }

        public async Task<Groepsreis> GetWithBestemmingDeelnemersAndTheirOuderByIdAsync(int id)
        {
            return await _context.Groepreizen
                .Include(g => g.Bestemming)
                .Include(g => g.Deelnemers)
                .ThenInclude(g => g.Kind)
                .ThenInclude(g => g.Gebruiker)
                .AsSplitQuery()
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Groepsreis> GetWithOnkostenById(int id)
        {
            return await _context.Groepreizen
                .Include(g => g.Bestemming)
                .Include(g => g.Programmas)
                .Include(g => g.Deelnemers)
                .Include(g => g.Onkosten)
                .FirstOrDefaultAsync(g => g.Id == id);
        }
        public async Task<Groepsreis> GetWithDetailsAndMonitorenDetailsByIdAsync(int id)
        {
            return await _context.Groepreizen
                            .Include(gr => gr.Bestemming)
                            .Include(gr => gr.Monitors)
                                .ThenInclude(m => m.Gebruiker)
                                .ThenInclude(cu => cu.OpleidingPersonen)
                                    .ThenInclude(op => op.Opleiding)
                            .AsSplitQuery()
                            .FirstOrDefaultAsync(gr => gr.Id == id);
        }



        public async Task<Groepsreis> GetWithDetailsById(int id)
        {
            return await _context.Groepreizen
                .Include(g => g.Bestemming)
                .ThenInclude(b => b.Fotos)
                .Include(g => g.Programmas)
                .ThenInclude(p => p.Activiteit)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<IEnumerable<Groepsreis>> GetUpcomingGroepsreizenAsync()
        {
            var currentDate = DateTime.Now;

            return await _context.Groepreizen
                .Where(g => g.Begindatum >= currentDate) // Filter upcoming dates
                .Include(g => g.Bestemming)              
                .Include(g => g.Monitors) 
                .ThenInclude(g => g.Gebruiker)
                .Include(g => g.Deelnemers)              
                .ToListAsync();
        }

        // Fetching available groepsreizen
        public async Task<IEnumerable<Groepsreis>> GetAvailableGroepsreizenAsync()
        {
            return await _context.Groepreizen
                .Include(g => g.Bestemming)
                .Include(g => g.Programmas)
                .ThenInclude(g => g.Activiteit)
                .Where(g => g.Begindatum >= DateTime.Now && !g.IsArchived)
                .ToListAsync();
        }

        public async Task DeleteGroepsreisWithMonitorsAndOnkostenAsync(Groepsreis gr)
        {


            // Delete Onkosten associated with this Groepsreis
            var onkosten = _context.Onkosten.Where(o => o.GroepsreisId == gr.Id);
            _context.Onkosten.RemoveRange(onkosten);

            // Delete Monitors associated with this Groepsreis
            var monitors = _context.Monitors.Where(m => m.GroepsreisDetailsId == gr.Id);
            _context.Monitors.RemoveRange(monitors);

            // Delete the Groepsreis
            _context.Remove(gr);

        }

        public async Task<IEnumerable<Groepsreis>> GetMonitoredGroepsreizenForUserAsync(string userId)
        {
            var monitoredGroepsreizen = await _context.Monitors
                .Include(m => m.Groepsreis)
                .ThenInclude(g => g.Bestemming)
                .Where(m => m.GebruikerId == userId)
                .ToListAsync();

            // Return een lijst van de Groepsreis objecten (in plaats van Monitor objecten)
            return monitoredGroepsreizen.Select(m => m.Groepsreis);
        }


    }
}
