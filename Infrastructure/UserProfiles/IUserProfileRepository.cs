using Core.Domain.Shared;
using Core.Domain.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UserProfiles
{
    public interface IUserProfileRepository
    {
        Task<UserProfile> Load(UserId id);
        Task Add(UserProfile entity);
        Task<bool> Exists (UserId id);
    }
}
