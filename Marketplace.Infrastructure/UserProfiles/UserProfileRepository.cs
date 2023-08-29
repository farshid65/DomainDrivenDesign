using Marketplace.Domain.Shared;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Infrastructure.UserProfiles
{
    public class UserProfileRepository:RavenDbRepository<Marketplace.Domain.UserProfiles.UserProfile,UserId>, IUserProfileRepository
    {
        public UserProfileRepository(IAsyncDocumentSession session)
            :base(session,id=>$"UserProfile{id.Value.ToString()}")
        {
            
        }
    }
}
