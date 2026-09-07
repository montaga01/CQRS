using Microsoft.EntityFrameworkCore;
using Domain.Todos;
using Infrastructure.Data.Configurations;
using Application.Common.Interfaces;

namespace Infrastructure.Data;

public class AppDBContext(DbContextOptions<AppDBContext> options)
: DbContext(options), IAppDbContext
{
    public DbSet<Todo> Todos => Set<Todo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TodoConfiguration).Assembly);
    }
}