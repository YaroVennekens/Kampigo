using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataAccess.Data.Repository.GenericRepo;
using Models;

namespace DataAccess.Data.Repository.OnkostenRepo
{
    public class OnkostenRepository : GenericRepository<Onkosten>, IOnkostenRepository
    {
        public OnkostenRepository(KampigoDbContext context) : base(context)
        {
        }

     
    }
}