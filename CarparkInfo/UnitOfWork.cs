namespace CarparkInfo;

public class UnitOfWork : IDisposable
{
    private readonly CarparkInfoContext context;
    private GenericRepository<CarparkInfo> carparkInfoRepository;

    public UnitOfWork(CarparkInfoContext context)
    {
        this.context = context;
    }

    public GenericRepository<CarparkInfo> CarparkInfoRepository
    {
        get
        {
            if (this.carparkInfoRepository == null)
            {
                this.carparkInfoRepository = new GenericRepository<CarparkInfo>(context);
            }

            return carparkInfoRepository;
        }
    }

    public void Save()
    {
        context.SaveChanges();
    }

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }

        this.disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}