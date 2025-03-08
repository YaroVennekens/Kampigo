using DataAccess.Data.Repository.GenericRepo;
using Microsoft.EntityFrameworkCore;
using Models;
using Deelnemer = Models.Deelnemer;

namespace DataAccess.Data.Repository.ReviewRepo
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(KampigoDbContext context): base(context)
        {

        }

        // Voeg een review toe aan de database
        public async Task AddReviewAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
        }

        public async Task<Review> GetReviewsByUserAndGroepsreisAsync(string userId, int bestemmingId)
        {
            return await _context.Reviews
                .FirstOrDefaultAsync(r => r.PersoonId == userId && r.BestemmingId == bestemmingId);
        }

        public async Task<IEnumerable<Review>> GetAllWithNames()
        {
            return await _context.Reviews
                .Include(g => g.Bestemming)
                .Include(g => g.Persoon)
                .ToListAsync();
        }


    }

}
