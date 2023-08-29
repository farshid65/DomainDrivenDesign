using Core.Domain;
using Core.Domain.ClassifiedAds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ClassifiedAd
{
    public class ClassifiedAdRepository : IClassifiedAdRepository
    {
        private readonly MarketplacrDbContext _dbContext;

        public ClassifiedAdRepository(MarketplacrDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public  async Task Add(Core.Domain.ClassifiedAds.ClassifiedAd entity)
        => await _dbContext.ClassifiedAds.AddAsync(entity);


        public async Task<bool> ExistsAsync(Core.Domain.ClassifiedAds.ClassifiedAdId id)
        => await _dbContext.ClassifiedAds.FindAsync(id.Value) != null;

        public async Task<Core.Domain.ClassifiedAds.ClassifiedAd> LoadAsync(Core.Domain.ClassifiedAds.ClassifiedAdId id)

            => await _dbContext.ClassifiedAds.FindAsync(id.Value);
        
    }
}
