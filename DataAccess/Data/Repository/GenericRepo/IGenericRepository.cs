namespace DataAccess.Data.Repository.GenericRepo
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync<T>(T id);
        Task AddAsync(TEntity entity);
        Task<bool> ExistsAsync(int id);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Save();
    }
}
