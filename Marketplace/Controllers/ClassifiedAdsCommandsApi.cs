using Contracts;
using Marketplace.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
    [Route("/ad")]
    [ApiController]
    public class ClassifiedAdsCommandsApi : ControllerBase
    {
        private readonly ClassifiedAdsApplicationService _applicationService;

        public ClassifiedAdsCommandsApi(ClassifiedAdsApplicationService applicationService)
        {
            _applicationService = applicationService;
        }
        [HttpPost]
        public async Task <IActionResult> Post(ClassifiedAds.V1.Create requet)
        {
            await _applicationService.Handle(requet);
            return Ok();  

        }
        [Route("name")]
        [HttpPut]
        public async Task <IActionResult> Put(ClassifiedAds.V1.SetTitle request)
        {
            await _applicationService.Handle(request);
            return Ok();

        }
        [Route("text")]
        [HttpPut]
        public async Task <IActionResult> Put(ClassifiedAds.V1.UpateText request)
        {
            await _applicationService.Handle(request);
        }
        [Route("price")]
        [HttpPut]
        public async Task <IActionResult> Put(ClassifiedAds.V1.UpdatePrice request)
        {
            await _applicationService.Handle(request);
            return Ok();
        }
        [Route("publish")]
        [HttpPut]
        public async Task <IActionResult> Put(ClassifiedAds.V1.RequestToPublish request)
        {
            await _applicationService.Handle(request);
            return Ok();
        }
    }

}
