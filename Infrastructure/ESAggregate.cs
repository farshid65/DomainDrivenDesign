﻿using EventStore.ClientAPI;
using Framework;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ESAggregate : IAggregateStore
    {
        private readonly IEventStoreConnection _connection;

        public ESAggregate(IEventStoreConnection connection) 
        {
            _connection = connection;
        }
        public async Task<bool> Exists<T, TId>(TId aggregateId)
        {
            var stream = GetStreamName<T, TId>(aggregateId);
            var result = await _connection.ReadEventAsync(stream, 1, false);
            return result.Status != EventReadStatus.NoStream;
        }

        public async Task<T> Load<T, TId>(TId aggregateId)
            where T:AggregateRoot<TId>
        {
            if(aggregateId == null)
                throw new ArgumentNullException(nameof(aggregateId));
            var stream=GetStreamName<T, TId>(aggregateId);
            var aggregate=(T) Activator.CreateInstance(typeof(T),true);
            var page=await _connection.ReadStreamEventsForwardAsync(stream, 0,1024,false);
            aggregate.Load(page.Events.Select(resolvedEvent=>
            {
              var meta=JsonConvert.DeserializeObject<EventMetadata>(
                    Encoding.UTF8.GetString(resolvedEvent.Event.Metadata));
                var dataType = Type.GetType(meta.ClrType);
                var jsonData = Encoding.UTF8.GetString(resolvedEvent.Event.Data);
                var data = JsonConvert.DeserializeObject(jsonData, dataType);
                return data;
            }).ToArray());
            return aggregate;
        }

        public async Task Save<T, TId>(T aggregate)
            where T:AggregateRoot<TId>
        {
            if(aggregate == null) 
                throw new ArgumentNullException(nameof(aggregate));
            var changes = aggregate.GetChanges()
            .Select(@event =>
            new EventData(
                       eventId:Guid.NewGuid(),
                        type: @event.GetType().Name,
                        isJson: true,
                        data: Serialize(@event),
                        metadata:Serialize(new EventMetadata
                        { ClrType=@event.GetType().AssemblyQualifiedName})
                    )).ToArray();
            if (!changes.Any()) return;
            var streamName=GetStreamName<T,TId>(aggregate);
            await _connection.AppendToStreamAsync(streamName, aggregate.Version, changes);
            aggregate.clearChanges();
        }
        

        private static byte[] Serialize(object data)
            =>Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
        private static string GetStreamName<T, TId>(TId aggregateId)
            => $"{typeof(T).Name}-{aggregateId.ToString()}";
        private static string GetStreamName<T, TId>(T aggregate)
            where T:AggregateRoot<TId>
            => $"{typeof(T).Name}-{aggregate.Id.ToString()}";



    }
}
