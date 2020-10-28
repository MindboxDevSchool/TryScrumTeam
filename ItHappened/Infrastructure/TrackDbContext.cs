using System.Data.Entity;
using ItHappened.Domain;

namespace ItHappened.Infrastructure
{
    public class TrackDbContext : DbContext
    {
        public DbSet<Track> Tracks { get; set; }
    }
}