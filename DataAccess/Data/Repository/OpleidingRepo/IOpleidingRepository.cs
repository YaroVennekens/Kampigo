using DataAccess.Data.Repository.GenericRepo;
using Microsoft.EntityFrameworkCore;
using Models;

public interface IOpleidingRepository : IGenericRepository<Opleiding>
{
    Task<IEnumerable<Opleiding>> GetAllWithUsersAsync();
    Task<Opleiding?> GetWithUsersByIdAsync(int id);
    Task<IEnumerable<Opleiding>> GetOpleidingenByUserIdAsync(string userId);
    Task<IEnumerable<Opleiding>> GetRecommendedOpleidingenAsync(string userId, DateTime currentDate);
    Task<IEnumerable<Opleiding>> GetOtherOpleidingenAsync(string userId, DateTime currentDate);
    Task<Opleiding?> GetOpleidingWithUsersByIdAsync(int id);
    Task<List<CustomUser>> GetIngeschrevenMonitorsAsync(int opleidingId);
    Task<List<CustomUser>> GetGeaccepteerdeMonitorsAsync(int opleidingId);


}
