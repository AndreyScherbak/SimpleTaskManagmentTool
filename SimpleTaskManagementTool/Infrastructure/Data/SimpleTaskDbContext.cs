using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public sealed class SimpleTaskDbContext : DbContext
    {
        public SimpleTaskDbContext(DbContextOptions<SimpleTaskDbContext> options)
            : base(options) { }

        public DbSet<Board> Boards => Set<Board>();
        public DbSet<TaskEntity> Tasks => Set<TaskEntity>();

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.ApplyConfigurationsFromAssembly(typeof(SimpleTaskDbContext).Assembly);
            base.OnModelCreating(mb);
        }
    }
}
