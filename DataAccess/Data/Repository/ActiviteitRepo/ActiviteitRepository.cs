using DataAccess.Data.Repository.GenericRepo;
using Models;

namespace DataAccess.Data.Repository.ActiviteitRepo
{

	
	public class ActiviteitRepository : GenericRepository<Activiteit>, IActiviteitRepository
	{
		public ActiviteitRepository(KampigoDbContext context) : base(context)
		{
		}
	}
}
