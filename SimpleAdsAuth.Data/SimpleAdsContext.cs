using Microsoft.EntityFrameworkCore;

namespace SimpleAdsAuth.Data
{
    public class SimpleAdsContext : DbContext
    {
        private readonly string _connectionString;

        public SimpleAdsContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<SimpleAd> Ads { get; set; }
    }
}