using Core.Domain.Shared;
using Core.Domain.UserProfile;
using Infrastructure.ClassifiedAd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UserProfiles
{
    public class UserProfileRepository : IUserProfileRepository, IDisposable
    {
        private readonly MarketplacrDbContext _dbcontext;

        public UserProfileRepository(MarketplacrDbContext context)
        {
            _dbcontext = context;
        }
        public  async Task Add(UserProfile entity)
        
           => await _dbcontext.UserProfies.AddAsync(entity);
              

        public async Task<bool> Exists(UserId id)
        
           =>await _dbcontext.UserProfies.FindAsync(id.Value)!=null;
       

        public async Task<UserProfile> Load(UserId id)
        =>await _dbcontext.UserProfies.FindAsync(id.Value);
        public void Dispose() => _dbcontext.Dispose();
        
            
        
    }
}
