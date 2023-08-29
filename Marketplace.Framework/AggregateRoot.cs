using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Framework
{
    public abstract class AggregateRoot<TID>
       : IInternalEventHandler where TID : Value<TID>
    {
        public TID Id { get; set; }        
        protected abstract void When(object @event);
        private readonly List<object> _changes;
        protected AggregateRoot() => _changes = new List<object>();
        protected void Apply(object @event)
        {
            When(@event);
            EnsureValidState();
            _changes.Add(@event);
        }
        public IEnumerable<object> GetChanges()
            => _changes.AsEnumerable();
        public void clearChanges() => _changes.Clear();
        protected abstract void EnsureValidState();
        protected void ApplyToEntity(
            IInternalEventHandler entity,
            object @event)
        => entity?.Handle(@event);
        void IInternalEventHandler.Handle(object @event)
       => When(@event);
    }
}
