using DataAccess.Data.Repository.GenericRepo;
using Models;

namespace DataAccess.Data.Repository.CustomUserRepo
{
	public interface ICustomUserRepository : IGenericRepository<CustomUser>
	{
        Task<List<CustomUser>> GetAllMonitorsAsync();
        Task<List<CustomUser>> GetMonitorsNotInOpleidingAsync(int opleidingId);
        Task<CustomUser?> GetMonitorWithGroepsreizenAsync(string id);
    }
}
