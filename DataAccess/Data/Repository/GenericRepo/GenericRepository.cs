using Microsoft.EntityFrameworkCore;

namespace DataAccess.Data.Repository.GenericRepo
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        // TODO: Verify correct dbContext name
        protected readonly KampigoDbContext _context;

        public GenericRepository(KampigoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync<T>(T id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task AddAsync(TEntity entity)
        {
            try
            {
                await _context.Set<TEntity>().AddAsync(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void Save()
        {
            _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            var exists = await _context.Set<TEntity>().FindAsync(id);

            return exists != null;
        }
    }
}
