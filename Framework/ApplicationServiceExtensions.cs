using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public static class ApplicationServiceExtensions
    {
        public static async Task HandleUpdate<T,TId>(
            this IApplicationService service,
            IAggregateStore store,TId aggregateId,
            Action<T>operation)
            where T : AggregateRoot<TId>
        {
            var aggregate=await store.Load<T,TId>(aggregateId);
            if (aggregate == null)
                throw new InvalidOperationException(
                    $"Entity with Id{aggregateId.ToString()}" +
                    $"cannot be found");
            operation(aggregate);
            await store.Save<T,TId>(aggregate);
        }
    }
}
