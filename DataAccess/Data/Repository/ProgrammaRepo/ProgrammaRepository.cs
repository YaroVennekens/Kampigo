using DataAccess.Data.Repository.GenericRepo;
using Models;

namespace DataAccess.Data.Repository.ProgrammaRepo
{
	public class ProgrammaRepository : GenericRepository<Programma>, IProgrammaRepository
	{
		public ProgrammaRepository(KampigoDbContext context) : base(context)
		{
		}
	}
}
