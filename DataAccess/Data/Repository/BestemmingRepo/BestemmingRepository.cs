using DataAccess.Data.Repository.GenericRepo;
using Models;

namespace DataAccess.Data.Repository.BestemmingRepo
{
	public class BestemmingRepository : GenericRepository<Bestemming>, IBestemmingRepository
	{
		public BestemmingRepository(KampigoDbContext context) : base(context)
		{
		}
	}
}
