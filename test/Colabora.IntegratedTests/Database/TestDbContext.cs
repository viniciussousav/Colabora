using Colabora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Colabora.IntegratedTests.Database;

public class TestDbContext : AppDbContext
{
    public TestDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("ColaboraTest");
    }
}