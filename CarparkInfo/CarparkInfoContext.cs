using Microsoft.EntityFrameworkCore;

namespace CarparkInfo;

public class CarparkInfoContext : DbContext
{
    public DbSet<CarparkInfo> CarparkInfos { get; set; }
    
    public CarparkInfoContext(DbContextOptions<CarparkInfoContext> options) : base(options)
    {
    }
}
