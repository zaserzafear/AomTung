using AomTung.DataAccessLayer.DataModels;
using Microsoft.EntityFrameworkCore;

namespace AomTung.DataAccessLayer.Data;

public partial class AomTungDbContext : DbContext
{
    public AomTungDbContext(DbContextOptions<AomTungDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<tbl_member> tbl_members { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<tbl_member>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
