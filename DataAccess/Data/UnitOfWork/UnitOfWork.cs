using DataAccess.Data.Repository.ActiviteitRepo;
using DataAccess.Data.Repository.BestemmingRepo;
using DataAccess.Data.Repository.CustomUserRepo;
using DataAccess.Data.Repository.DeelnemerRepo;
using DataAccess.Data.Repository.FotoRepo;
using DataAccess.Data.Repository.GroepsreisRepo;
using DataAccess.Data.Repository.KindRepo;
using DataAccess.Data.Repository.MonitorRepo;
using DataAccess.Data.Repository.OnkostenRepo;
using DataAccess.Data.Repository.OpleidingRepo;
using DataAccess.Data.Repository.OpleidingPersoonRepo;
using DataAccess.Data.Repository.ProgrammaRepo;
using DataAccess.Data.Repository.ReviewRepo;

namespace DataAccess.Data.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly KampigoDbContext _context;

		private IActiviteitRepository _activiteitRepository;
		private IBestemmingRepository _bestemmingRepository;
		private ICustomUserRepository _customUserRepository;
		private IDeelnemerRepository _deelnemerRepository;
		private IFotoRepository _fotoRepository;
		private IGroepsreisRepository _groepsreisRepository;
		private IKindRepository _kindRepository;
		private IMonitorRepository _monitorRepository;
		private IOnkostenRepository _onkostenRepository;
		private IOpleidingRepository _opleidingRepository;
		private IOpleidingPersoonRepository _opleidingPersoonRepository;
		private IProgrammaRepository _programmaRepository;
		private IReviewRepository _reviewRepository;

		public UnitOfWork(KampigoDbContext context)
		{
			_context = context;
		}

		public IActiviteitRepository ActiviteitRepository => _activiteitRepository ??= new ActiviteitRepository(_context);
		public IBestemmingRepository BestemmingRepository => _bestemmingRepository ??= new BestemmingRepository(_context);
		public ICustomUserRepository CustomUserRepository => _customUserRepository ??= new CustomUserRepository(_context);
		public IDeelnemerRepository DeelnemerRepository => _deelnemerRepository ??= new DeelnemerRepository(_context);
		public IFotoRepository FotoRepository => _fotoRepository ??= new FotoRepository(_context);
		public IGroepsreisRepository GroepsreisRepository => _groepsreisRepository ??= new GroepsreisRepository(_context);
		public IKindRepository KindRepository => _kindRepository ??= new KindRepository(_context);
		public IMonitorRepository MonitorRepository => _monitorRepository ??= new MonitorRepository(_context);
		public IOnkostenRepository OnkostenRepository => _onkostenRepository ??= new OnkostenRepository(_context);
		public IOpleidingRepository OpleidingRepository => _opleidingRepository ??= new OpleidingRepository(_context);
		public IOpleidingPersoonRepository OpleidingPersoonRepository => _opleidingPersoonRepository ??= new OpleidingPersoonRepository(_context);
		public IProgrammaRepository ProgrammaRepository => _programmaRepository ??= new ProgrammaRepository(_context);
		public IReviewRepository ReviewRepository => _reviewRepository ??= new ReviewRepository(_context);

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
