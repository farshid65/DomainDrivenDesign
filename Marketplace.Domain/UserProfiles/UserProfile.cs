using Marketplace.Domain.ClassifiedAds;
using Marketplace.Domain.Shared;
using Marketplace.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Domain.UserProfiles
{
   public class UserProfile : AggregateRoot<UserId>
    {
        private string DbId
        {
            get => $"UserProfile/{Id.Value}";
            set { }
        }
        //public Guid Id { get;private set; } 
        public FullName FullName { get; private set; }
        public DisplayName DisplayName { get; private set; }
        public string PhotoUrl { get; private set; }
        public UserProfile(UserId id, FullName fullName, DisplayName displayName)
        => Apply(new Events.UserRegistered
        {
            UserId = id,
            FullName = fullName,
            DisplayName = displayName
        });
        public void UpdateFullName(FullName fullName)
            => Apply(new Events.UserFullNameUpdated
            {
                UserId = Id,
                FullName = fullName
            });
        public void UpdateDisplayName(DisplayName displayName)
            => Apply(new Events.UserDisplayNameUpdated
            {
                UserId = Id,
                DisplayName = displayName
            });
        public void UpdateProfilePhoto(Uri photoUrl)
            => Apply(new Events.ProfilePhotoUpload
            {
                UserId = Id,
                PhotoUrl = photoUrl.ToString()
            });
        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.UserRegistered e:
                    Id = new UserId(e.UserId);
                    FullName = new FullName(e.FullName);
                    DisplayName = new DisplayName(e.DisplayName);
                    break;
                case Events.UserFullNameUpdated e:
                    FullName = new FullName(e.FullName);
                    break;
                case Events.UserDisplayNameUpdated e:
                    DisplayName = new DisplayName(e.DisplayName);
                    break;
                case Events.ProfilePhotoUpload e:
                    PhotoUrl = e.PhotoUrl;
                    break;

            }
        }
        protected override void EnsureValidState()
        {

        }
    }

}
