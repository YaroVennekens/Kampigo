using DataAccess.Data.Repository.GenericRepo;
using Models;

namespace DataAccess.Data.Repository.FotoRepo
{
	public class FotoRepository : GenericRepository<Foto>, IFotoRepository
	{
		public FotoRepository(KampigoDbContext context) : base(context)
		{
		}
	}
}
