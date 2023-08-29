using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Marketplace.Infrastructure.ClassifiedAd.ReadModels;

namespace Marketplace.Infrastructure.ClassifiedAd
{
    public interface IClassifiedAdQueryService
    {
        Task<IEnumerable<PublicClassifiedAdListItem>> GetPublishedAds(int page, int pageSize);
        Task<ClassifiedAdDetails> GetPublishClassifiedAd(Guid classifiedAdId);
        Task<IEnumerable<PublicClassifiedAdListItem>> GetClassifiedAdsOwnedBy(Guid userId, int page, int pageSize);
    }
}
