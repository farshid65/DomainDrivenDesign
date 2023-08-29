using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marketplace.Domain.Shared;
using Marketplace.Framework;

namespace Marketplace.Infrastructure.UserProfiles
{
    public class UserProfileApplicationService:IApplicationService
    {
        private readonly IUserProfileRepository _repository;
        private readonly IUnitofWork _unitofWork;
        private readonly  CheckTextForProfanity _checkText;

        public UserProfileApplicationService(IUserProfileRepository repository,IUnitofWork unitofWork,
            CheckTextForProfanity checkText)
        {
            _repository = repository;
            _unitofWork = unitofWork;
            _checkText = checkText;
        }
        public async Task Handle(object command)
        {
            switch (command)
            {
                case Contracts.V1.RegisterUser cmd:
                    if (await _repository.Exists(new UserId(cmd.UserId)))
                        throw new InvalidOperationException($"Entity with Id{cmd.UserId} already exists");
                    var userProfile = new Marketplace.Domain.UserProfiles.UserProfile(new UserId(cmd.UserId),
                         Marketplace.Domain.UserProfiles.FullName.FromString(cmd.FullName),
                         Marketplace.Domain.UserProfiles.DisplayName.FromString(cmd.DisplayName, _checkText));
                    await _repository.Add(userProfile);
                    await _unitofWork.Commit();
                    break;
                case Contracts.V1.UpdateUserFullName cmd:
                        await HandleUpdate(cmd.UserId,
                            profile => profile.UpdateFullName(Marketplace.Domain.UserProfiles.FullName.FromString(cmd.FullName)));
                    break;
                    case Contracts.V1.UpdateDisplayName cmd:
                        await HandleUpdate(cmd.UserId,
                            profile=>profile.UpdateDisplayName(Marketplace.Domain.UserProfiles.DisplayName.FromString(cmd.DisplayName,
                            _checkText)));
                    break;
                case Contracts.V1.UpdateUserProfilePhoto cmd:
                    await HandleUpdate(cmd.UserId,
                        profile=>profile.UpdateProfilePhoto(new Uri(cmd.PhotoUrl)));
                    break;
                default:
                    throw new InvalidOperationException($"Command type{command.GetType().FullName}" +
                        $"is unknown");                   
            }
        }
        private async Task HandleUpdate(Guid userProfileId,
           Action<Marketplace.Domain.UserProfiles.UserProfile>operation )
        {
            var classifiedAd=await
                _repository.Load(userProfileId.ToString());
            if (classifiedAd == null)
                throw new InvalidOperationException($"Entity with id {userProfileId}can not be found");
            operation( classifiedAd );
            await _unitofWork.Commit();
        }
            
        }
    }

