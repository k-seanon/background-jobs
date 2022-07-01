using JobService.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;

namespace JobService.SqlServer;

public class JobsDbContext : DbContext
{
    public DbSet<JobEntity> Jobs => Set<JobEntity>();

    public JobsDbContext(DbContextOptions<JobsDbContext> options) : base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}


//facilitates migrations in standalone project (see https://go.microsoft.com/fwlink/?linkid=851728)
public class TempDbContextFactory : IDesignTimeDbContextFactory<JobsDbContext>
{
    public JobsDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<JobsDbContext>();
        optionsBuilder.UseSqlServer("server=.;trusted_connection=true;database=jobstemp");
        return new(optionsBuilder.Options);
    }
}