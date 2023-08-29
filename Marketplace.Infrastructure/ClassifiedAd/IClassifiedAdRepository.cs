using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marketplace.Domain.ClassifiedAds;

namespace Marketplace.Infrastructure.ClassifiedAd
{
    public interface IClassifiedAdRepository
    {
        Task Add(Domain.ClassifiedAds.ClassifiedAd entity);
        Task<bool> ExistsAsync(ClassifiedAdId id);
        
        Task<Domain.ClassifiedAds.ClassifiedAd> LoadAsync(ClassifiedAdId id);

    }
}
