using DataAccess.Data.Repository.ActiviteitRepo;
using DataAccess.Data.Repository.BestemmingRepo;
using DataAccess.Data.Repository.CustomUserRepo;
using DataAccess.Data.Repository.DeelnemerRepo;
using DataAccess.Data.Repository.FotoRepo;
using DataAccess.Data.Repository.GroepsreisRepo;
using DataAccess.Data.Repository.KindRepo;
using DataAccess.Data.Repository.MonitorRepo;
using DataAccess.Data.Repository.OnkostenRepo;
using DataAccess.Data.Repository.OpleidingPersoonRepo;
using DataAccess.Data.Repository.OpleidingRepo;
using DataAccess.Data.Repository.ProgrammaRepo;
using DataAccess.Data.Repository.ReviewRepo;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Data.UnitOfWork
{
	public interface IUnitOfWork
	{
		IActiviteitRepository ActiviteitRepository { get; }
		IBestemmingRepository BestemmingRepository { get; }
		ICustomUserRepository CustomUserRepository { get; }
		IDeelnemerRepository DeelnemerRepository { get; }
		IFotoRepository FotoRepository { get; }
		IGroepsreisRepository GroepsreisRepository { get; }
		IKindRepository KindRepository { get; }
		IMonitorRepository MonitorRepository { get; }
		IOnkostenRepository OnkostenRepository { get; }
		IOpleidingRepository OpleidingRepository { get; }
		IOpleidingPersoonRepository OpleidingPersoonRepository { get; }
		IProgrammaRepository ProgrammaRepository { get; }
		IReviewRepository ReviewRepository { get; }

		public Task SaveChangesAsync();

		Task CompleteAsync();
        
    }
}
