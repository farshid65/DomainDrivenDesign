using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.UserProfile
{
    public static class Events
    {
        public class UserRegistered
        {
            public Guid UserId { get; set; }
            public string FullName { get; set; }
            public string DisplayName { get; set; }
        }
        public class ProfilePhotoUpload            
        {
            public Guid UserId { get; set; }
            public string PhotoUrl { get; set; }
        }
        public class UserFullNameUpdated
        { 
        public Guid UserId { get; set;}
        public string FullName { get; set; }

        }
        public class UserDisplayNameUpdated
        {
            public Guid UserId { get; set; }
            public string DisplayName { get; set; }
        }
    }
}
