using ItHappened.Domain;
using Microsoft.EntityFrameworkCore;

namespace ItHappened.Infrastructure
{
    public class TrackDbContext : DbContext
    {
        public TrackDbContext()
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(  "Server=tcp:mindbox-dev-school.database.windows.net,1433;Initial Catalog=MindboxDevSchool;Persist Security Info=False;User ID=margelov;Password=6LKj8Upi6ZRip6Mkh9Pj;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
                );
        }
        public TrackDbContext(DbContextOptions<TrackDbContext> options) : base(options)
        {
        }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<Track>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<User>()
                .HasKey(c => c.Id);
        }
        
    }
}