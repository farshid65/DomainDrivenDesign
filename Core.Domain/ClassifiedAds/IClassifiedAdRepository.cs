using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.ClassifiedAds
{
    public interface IClassifiedAdRepository
    {
        Task Add(ClassifiedAd entity);
        Task<bool> ExistsAsync(ClassifiedAdId id);

        Task<ClassifiedAd> LoadAsync(ClassifiedAdId id);

    }
}
