using Marketplace.Framework;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Infrastructure.ClassifiedAd
{
    public class RavenDbUnitofWork : IUnitofWork
    {
        private readonly IAsyncDocumentSession _session;

        public RavenDbUnitofWork(IAsyncDocumentSession session)
        {
            _session = session;
        }
        public Task Commit()=>_session.SaveChangesAsync();
       
    }
}
