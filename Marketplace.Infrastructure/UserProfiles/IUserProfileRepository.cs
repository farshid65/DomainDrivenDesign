using Marketplace.Domain.Shared;
using Marketplace.Domain.UserProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Infrastructure.UserProfiles
{
    public interface IUserProfileRepository
    {
        Task<UserProfile> Load(UserId id);
        Task Add(UserProfile entity);
        Task<bool> Exists (UserId id);
    }
}
