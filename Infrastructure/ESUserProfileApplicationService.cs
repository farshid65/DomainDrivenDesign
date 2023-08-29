using Core.Domain.Shared;
using Core.Domain.UserProfile;
using Framework;
using Infrastructure.UserProfiles;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ESUserProfileApplicationService : IApplicationService
    {
        private readonly IAggregateStore _store;
        private readonly CheckTextForProfanity _checkText;

        public ESUserProfileApplicationService(IAggregateStore store,CheckTextForProfanity checkText)
        {
            _store = store;
            _checkText = checkText;
        }
        public Task Handle(object command) =>
            command switch
            {
                Contracts.V1.RegisterUser cmd =>
                 HandlerCreate(cmd),
                Contracts.V1.UpdateUserFullName cmd =>
                HandleUpdate(
                    cmd.UserId,
                    profile => profile.UpdateFullName(
                        FullName.FromString(cmd.FullName)
                        )
                    ),
                Contracts.V1.UpdateDisplayName cmd =>
                HandleUpdate(
                    cmd.UserId,
                    profile => profile.UpdateDisplayName(
                        DisplayName.FromString(
                            cmd.DisplayName,
                            _checkText
                            )
                            )),
                Contracts.V1.UpdateUserProfilePhoto cmd =>
                HandleUpdate(
                    cmd.UserId,
                    profile => profile.UpdateProfilePhoto(
                        new Uri(cmd.PhotoUrl)
                        )
                    ),
                _ => Task.CompletedTask
            };
        private async Task HandlerCreate(Contracts.V1.RegisterUser cmd)
        {
            if (await _store.Exists<Core.Domain.UserProfile.UserProfile,UserId>(
                new UserId(cmd.UserId)
                ))
                throw new InvalidCastException(
                    $"Entity with id {cmd.UserId} already exists");
            var userProfile = new Core.Domain.UserProfile.UserProfile(
                new UserId(cmd.UserId),
                FullName.FromString(cmd.FullName),
                DisplayName.FromString(cmd.DisplayName, _checkText)
                );
            await _store.Save<Core.Domain.UserProfile.UserProfile, UserId>(
                userProfile
                );
        }
        private Task HandleUpdate(
            Guid id,
            Action<Core.Domain.UserProfile.UserProfile> update
            ) =>
            this.HandleUpdate(
                _store, new UserId(id), update);
    }
}
