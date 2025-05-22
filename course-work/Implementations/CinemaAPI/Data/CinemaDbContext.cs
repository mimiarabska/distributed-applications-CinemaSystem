using Microsoft.EntityFrameworkCore;
using CinemaAPI.Models;

namespace CinemaAPI.Data
{
    public class CinemaDbContext : DbContext
    {
        public CinemaDbContext(DbContextOptions<CinemaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Projection> Projections { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Movie -> Projections (1:M)
            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Projections)
                .WithOne(p => p.Movie)
                .HasForeignKey(p => p.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            // Hall -> Projections (1:M)
            modelBuilder.Entity<Hall>()
                .HasMany(h => h.Projections)
                .WithOne(p => p.Hall)
                .HasForeignKey(p => p.HallId)
                .OnDelete(DeleteBehavior.Cascade);

            // Projection -> Reservations (1:M)
            modelBuilder.Entity<Projection>()
                .HasMany(p => p.Reservations)
                .WithOne(r => r.Projection)
                .HasForeignKey(r => r.ProjectionId)
                .OnDelete(DeleteBehavior.Cascade);

            // User -> Reservations (1:M)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Reservations)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Projection>()
                .Property(p => p.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Reservation>()
                .Property(r => r.TotalPrice)
                .HasPrecision(10, 2); 

         
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Username = "admin",
                Password = "admin123",  
                FullName = "Admin Adminov",
                Email = "admin@cinema.com",
                Role = "Admin"
            });

        }
    }
}
