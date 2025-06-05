using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Configurations
{
    internal sealed class BoardConfiguration : IEntityTypeConfiguration<Board>
    {
        public void Configure(EntityTypeBuilder<Board> b)
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.Title)
             .IsRequired()
             .HasMaxLength(100);

            b.HasMany(x => x.Tasks)
             .WithOne(t => t.Board!)
             .HasForeignKey(t => t.BoardId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
