using DataAccess.Data.Repository.GenericRepo;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.Data.Repository.KindRepo
{
	public class KindRepository : GenericRepository<Kind>, IKindRepository
	{
		public KindRepository(KampigoDbContext context) : base(context)
		{
		}

		public async Task<IEnumerable<Kind>> GetAllKinderenOnGebruikerIdAsync(string id)
		{
			return await _context.Kinderen
						.Where(k => k.GebruikerId == id)
						.ToListAsync();
		}
    }
}
