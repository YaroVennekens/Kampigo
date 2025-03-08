using DataAccess.Data.Repository.GenericRepo;
using Microsoft.EntityFrameworkCore;
using Models;
using Deelnemer = Models.Deelnemer;

namespace DataAccess.Data.Repository.ReviewRepo
{
    public interface IReviewRepository
    {
        Task AddReviewAsync(Review review);

        Task<Review> GetReviewsByUserAndGroepsreisAsync(string userId, int bestemmingId);

        Task<IEnumerable<Review>> GetAllWithNames();


    }
}
