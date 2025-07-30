using Microsoft.EntityFrameworkCore;
using CarparkInfoApi.Models;

namespace CarparkInfoApi.Data;

public class CarparkInfoContext : DbContext
{
    public DbSet<CarparkInfo> CarparkInfos { get; set; }

    public CarparkInfoContext(DbContextOptions<CarparkInfoContext> options) : base(options)
    {
    }
}
