using System.Threading.Tasks;
using Colabora.Infrastructure.Persistence;

namespace Colabora.IntegrationTests.Database;

public class DatabaseFixture
{
    public async Task ClearDatabase(AppDbContext context)
    {
        context.RemoveRange(context.Volunteers);
        context.RemoveRange(context.Organizations);
        await context.SaveChangesAsync();
    }
}