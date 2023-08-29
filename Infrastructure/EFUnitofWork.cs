using Framework;
using Infrastructure.ClassifiedAd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class EfUnitofWork : IUnitofWork
    {
        private readonly ClassifiedAdDbContext _dbContext;

        public EfUnitofWork(ClassifiedAdDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task Commit()

            => _dbContext.SaveChangesAsync();

    }
}
