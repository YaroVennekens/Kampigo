using DataAccess.Data.Repository.GenericRepo;
using Models;
using System.Linq.Expressions;

namespace DataAccess.Data.Repository.DeelnemerRepo
{
	public interface IDeelnemerRepository : IGenericRepository<Deelnemer>
	{
        Task<Deelnemer> GetAsync(Func<Deelnemer, bool> predicate);
        Task<IEnumerable<Deelnemer>> GetRegisteredDeelnemersForUserAsync(string userId);
        Task<IEnumerable<Deelnemer>> FindAllAsync(Expression<Func<Deelnemer, bool>> predicate);
    }
}