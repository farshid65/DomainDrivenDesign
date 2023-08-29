using Infrastructure.UserProfiles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Endpoint.UserProfiles
{
    
    [Route("/profile")]
    [ApiController]
    public class UserProfileCommandApi : ControllerBase
    {
        private static readonly Serilog.ILogger Log =
            Log.ForContext<UserProfileCommandApi>();
        private readonly UserProfileApplicationService _applicationService;

        public UserProfileCommandApi(UserProfileApplicationService applicationService)
        => _applicationService = applicationService;

        [HttpPost]
        public Task<IActionResult> Post(Contracts.V1.RegisterUser
            request)
        => RequestHandler.HandleRequest(request, _applicationService.Handle, Log);
        [Route("fullName")]
        [HttpPut]
        public Task<IActionResult> put(Contracts.V1.UpdateUserFullName
            request)
            => RequestHandler.HandleRequest(request, _applicationService.Handle, Log);
        [Route("displayName")]
        [HttpPut]
        public Task<IActionResult> put(Contracts.V1.UpdateDisplayName
            request)
            => RequestHandler.HandleRequest(request, _applicationService.Handle, Log);
        [Route("photo")]
        [HttpPut]
        public Task<IActionResult>put (Contracts.V1.UpdateUserProfilePhoto
            request)
            => RequestHandler.HandleRequest(request, _applicationService.Handle, Log);
    }
}
