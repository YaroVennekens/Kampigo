using DataAccess.Data.Repository.GenericRepo;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Data.Repository.DeelnemerRepo
{
    public class DeelnemerRepository : GenericRepository<Deelnemer>, IDeelnemerRepository
    {
        public DeelnemerRepository(KampigoDbContext context) : base(context)
        {
        }
        public async Task<Deelnemer> GetAsync(Func<Deelnemer, bool> predicate)
        {
            return _context.Deelnemers
           .Where(predicate)
           .FirstOrDefault();
        }

        public async Task<IEnumerable<Deelnemer>> GetRegisteredDeelnemersForUserAsync(string userId)
        {
            // Get all child IDs for the given user
            var kindIds = await _context.Kinderen
                .Where(k => k.GebruikerId == userId)
                .Select(k => k.Id)
                .ToListAsync();

            // Fetch all the Deelnemers for the user's children, including Groepsreis and Bestemming data
            var childrensDeelnemers = await _context.Deelnemers
                .Include(d => d.Groepsreis)
                    .ThenInclude(g => g!.Bestemming)
                .Include(d => d.Kind)
                .Where(d => kindIds.Contains(d.KindId))
                .ToListAsync();

            return childrensDeelnemers;
        }

        public async Task<IEnumerable<Deelnemer>> FindAllAsync(Expression<Func<Deelnemer, bool>> predicate)
        {
            return await _context.Deelnemers.Where(predicate).ToListAsync();
        }
    }
}
