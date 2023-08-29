using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Domain.ClassifiedAds
{
    public interface IHandleCommand<in T>
    {
        Task Handle(T command);
    }
}
