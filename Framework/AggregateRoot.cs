﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public abstract class AggregateRoot<TID>
       : IInternalEventHandler 
    {
        public TID Id { get; protected set; }
        public int Version { get; protected set; }
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
        public void Load(IEnumerable<object> history)
        {
            foreach (var e in history)
            {
                When(e);
                Version++;

            }
        }
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
