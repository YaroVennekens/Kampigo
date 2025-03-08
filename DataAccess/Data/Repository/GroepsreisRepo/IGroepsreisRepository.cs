using DataAccess.Data.Repository.GenericRepo;
using Models;

namespace DataAccess.Data.Repository.GroepsreisRepo
{
	public interface IGroepsreisRepository : IGenericRepository<Groepsreis>
	{
		Task<IEnumerable<Groepsreis>> GetAllWithActivities();
        Task<Groepsreis> GetWithDetailsById(int id);
        
        Task<Groepsreis> GetWithOnkostenById(int id);
		Task<Groepsreis> GetWithDetailsAndMonitorenDetailsByIdAsync(int id);
		Task<Groepsreis> GetWithBestemmingDeelnemersAndTheirOuderByIdAsync(int id);
		Task<IEnumerable<Groepsreis>> GetUpcomingGroepsreizenAsync();
		Task<IEnumerable<Groepsreis>> GetAvailableGroepsreizenAsync();
		Task DeleteGroepsreisWithMonitorsAndOnkostenAsync(Groepsreis gr);
		Task<IEnumerable<Groepsreis>> GetMonitoredGroepsreizenForUserAsync(string userId);


    }
}
