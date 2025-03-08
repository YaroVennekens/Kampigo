using DataAccess.Data.Repository.GenericRepo;
using Models;

namespace DataAccess.Data.Repository.KindRepo
{
	public interface IKindRepository : IGenericRepository<Kind>
	{
		Task<IEnumerable<Kind>> GetAllKinderenOnGebruikerIdAsync(string id);
	}
}
