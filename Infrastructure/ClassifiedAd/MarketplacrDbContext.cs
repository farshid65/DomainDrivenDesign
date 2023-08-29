
using Infrastructure.UserProfiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Logging;




namespace Infrastructure.ClassifiedAd
{
    public class MarketplacrDbContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        public MarketplacrDbContext(DbContextOptions<MarketplacrDbContext> options,ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }
        public DbSet<Core.Domain.ClassifiedAds.ClassifiedAd> ClassifiedAds { get; set; }
        public DbSet<Core.Domain.UserProfile.UserProfile> UserProfies { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);
            optionsBuilder.EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClassiiedAdEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PictureEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserProfileEntityTypeConfiguration());
        }

    }

}


