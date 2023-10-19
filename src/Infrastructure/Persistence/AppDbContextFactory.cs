using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Persistence;

class AppDbContextFactory : IDesignTimeDbContextFactory<AppContext>
{
    public AppContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppContext>();
        optionsBuilder.UseMySQL("Server=localhost;Port=3306;Database=A10P;Uid=root;Pwd=123456;");

        return new AppContext(optionsBuilder.Options);
    }
}