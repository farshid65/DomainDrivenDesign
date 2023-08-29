using Marketplace.Domain.ClassifiedAds;
using Raven.Client.Documents.Identity;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Infrastructure.ClassifiedAd
{
    public class ClassifiedAdRepository : IClassifiedAdRepository
    {
        private readonly IAsyncDocumentSession _session;
        public ClassifiedAdRepository(IAsyncDocumentSession session)
        {
            _session = session;
        }

        public  Task Add(Domain.ClassifiedAds.ClassifiedAd entity)
        
         => _session.StoreAsync(entity,EntityId(entity.Id));


        public Task<bool> ExistsAsync(ClassifiedAdId id)
       => _session.Advanced.ExistsAsync(EntityId(id));

        public Task<Domain.ClassifiedAds.ClassifiedAd> LoadAsync(ClassifiedAdId id)
        => _session.LoadAsync<Domain.ClassifiedAds.ClassifiedAd>(EntityId(id));
        private static string EntityId(ClassifiedAdId id)            
          =>$"ClassifiedAd/{id.ToString()}";
       
         




    }
}
