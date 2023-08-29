using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Infrastructure.ClassifiedAd
{
    public interface IRepository<T>
    { 
        void GetById(int id);
        void Save(T entity);
        IEnumerable<T> Query(Func<T,bool>filter);
    }
}
