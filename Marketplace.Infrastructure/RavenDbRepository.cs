using Marketplace.Framework;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Infrastructure
{
    public class RavenDbRepository<T,TId>
        where T: AggregateRoot<TId>
        where TId : Value<TId>
    {
        private readonly IAsyncDocumentSession _session;
        private readonly Func<TId,string> _entityId;

        public RavenDbRepository(IAsyncDocumentSession session,Func<TId,string>entityId)
        {
            _session = session;
            _entityId = entityId;
        }
        public async Task Add(T entity)
            =>_session.StoreAsync(entity,_entityId(entity.Id));
        public Task <bool> Exists(TId id)
            =>_session.Advanced.ExistsAsync(_entityId(id));
        public Task<T> Load(TId id)
            => _session.LoadAsync<T>(_entityId(id));

    }
}
