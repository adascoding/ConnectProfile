using Microsoft.EntityFrameworkCore;
using ConnectProfile.Api.Entities;

namespace ConnectProfile.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<UserInfo> UserInfos { get; set; }
    public DbSet<Image> Images { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserInfo>()
            .HasOne(ui => ui.Account)
            .WithOne()
            .HasForeignKey<UserInfo>(ui => ui.AccountId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<UserInfo>()
            .HasOne(ui => ui.ProfilePicture)
            .WithOne()
            .HasForeignKey<UserInfo>(ui => ui.ProfilePictureId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<UserInfo>()
            .OwnsOne(ui => ui.Address, address =>
            {
                address.Property(a => a.City).HasMaxLength(100);
                address.Property(a => a.Street).HasMaxLength(200);
                address.Property(a => a.HouseNumber).HasMaxLength(20);
                address.Property(a => a.ApartmentNumber).HasMaxLength(10);
            });
    }
}
