using Microsoft.EntityFrameworkCore;
using CarparkInfoApi.Models;

namespace CarparkInfoApi.Data;

public class CarparkInfoContext(DbContextOptions<CarparkInfoContext> options) : DbContext(options)
{
    public DbSet<CarparkInfo> CarparkInfos { get; set; }

}
