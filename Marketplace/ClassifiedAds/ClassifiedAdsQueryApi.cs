using Marketplace.Infrastructure;
using Marketplace.Infrastructure.ClassifiedAd;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using Serilog;
using System.Net;

namespace Marketplace.ClassifiedAds
{
    [Route("/ad")]
    [ApiController]
    public class ClassifiedAdsQueryApi : ControllerBase
    {
        private static Serilog.ILogger _log =
            Log.ForContext<ClassifiedAdsQueryApi>();
        private readonly IAsyncDocumentSession _session;

        public ClassifiedAdsQueryApi(IAsyncDocumentSession session)=> _session = session;

        [HttpGet]
        [Route("list")]
        public Task<IActionResult> Get(QueryModels.GetPublishClassifiedAds request)
        => RequestHandler.HandleQuery(()
            => _session.Query(request), _log);
        [HttpGet]
        [Route("myads")]
        public Task<IActionResult> Get(QueryModels.GetOwnersClassifiedAd request)
            => RequestHandler.HandleQuery(() 
                => _session.Query(request), _log);
        [HttpGet]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public Task<IActionResult> Get(QueryModels.GetPublishClassifiedAd request)
        =>RequestHandler.HandleQuery(()
            =>_session.Query(request), _log);


    }
}
