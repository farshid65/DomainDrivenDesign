using Infrastructure.ClassifiedAd;
using Infrastructure.UserProfiles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Data.Common;
using System.Net;

namespace Endpoint.ClassifiedAds
{
    
    [Route("/ad")]
    [ApiController]
    public class ClassifiedAdsQueryApi : ControllerBase
    {
        private static Serilog.ILogger _log =
            Log.ForContext<ClassifiedAdsQueryApi>();
        private readonly DbConnection _connection;

        public ClassifiedAdsQueryApi(DbConnection connection) => _connection = connection;

        [HttpGet]
        [Route("list")]
        public Task<IActionResult> Get(QueryModels.GetPublishClassifiedAds request)
        => RequestHandler.HandleQuery(()
            => _connection.Query(request), _log);
        [HttpGet]
        [Route("myads")]
        public Task<IActionResult> Get(QueryModels.GetOwnersClassifiedAd request)
            => RequestHandler.HandleQuery(()
                => _connection.Query(request), _log);
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public Task<IActionResult> Get(QueryModels.GetPublishClassifiedAd request)
        => RequestHandler.HandleQuery(()
            => _connection.Query(request), _log);


    }
}
