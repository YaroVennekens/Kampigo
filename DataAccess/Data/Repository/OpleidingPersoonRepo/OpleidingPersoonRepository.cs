using DataAccess.Data.Repository.GenericRepo;
using Models;

namespace DataAccess.Data.Repository.OpleidingPersoonRepo
{
	public class OpleidingPersoonRepository : GenericRepository<OpleidingPersoon>, IOpleidingPersoonRepository
	{
		public OpleidingPersoonRepository(KampigoDbContext context) : base(context)
		{
		}
	}
}
