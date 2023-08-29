using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ClassifiedAd
{
    public static class QueryModels
    {
        public class GetPublishClassifiedAds
        {
            public int Page { get; set; }
            public int PageSize { get; set; }
        }
        public class GetOwnersClassifiedAd
        {
            public Guid OwnerId { get; set; }
            public int Page { get; set; }
            public int PageSize { get; set; }
        }
        public class GetPublishClassifiedAd
        {
            public Guid ClassifiedAdId { get; set; }
        }
    }
}
