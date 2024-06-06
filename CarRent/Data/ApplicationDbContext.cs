using CarRent.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRent.Data;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<CarRental> CarRentals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasMany(u => u.FavoriteCars)
            .WithMany(c => c.UsersWhoFavorited)
            .UsingEntity(j => j.ToTable("UserFavoriteCars"));

        modelBuilder.Entity<CarRental>()
            .HasKey(cr => new { cr.UserId, cr.CarId });

        modelBuilder.Entity<CarRental>()
            .HasOne(cr => cr.Car)
            .WithMany(c => c.Rentals)
            .HasForeignKey(cr => cr.CarId);

        modelBuilder.Entity<CarRental>()
            .HasOne(cr => cr.User)
            .WithMany(u => u.RentedCars)
            .HasForeignKey(cr => cr.UserId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.MyCars)
            .WithOne(c => c.Owner)
            .HasForeignKey(c => c.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}