using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Domain.ClassifiedAds.ClassifiedAd;
using static Infrastructure.ClassifiedAd.QueryModels;
using static Infrastructure.ClassifiedAd.ReadModels;

namespace Infrastructure.ClassifiedAd
{
    public static class Queries
    {
        public static Task<IEnumerable<PublicClassifiedAdListItem>> Query(
            this DbConnection connection, GetPublishClassifiedAds query)
        => connection.QueryAsync<PublicClassifiedAdListItem>(
            "SELECT \"ClassifiedAdId\",\"Price_Amount\",\"Title_Value\"" +
            "FROM \"ClassifiedAds\" WHERE \"State\"=@State LIMIT " +
            "@PageSize OFFSET @Offset",
            new
            {
                State = (int)ClassifiedAdState.Active,
                PageSize = (int)query.PageSize,
                Offset = Offset(query.Page,query.PageSize)
            });
        public static Task<IEnumerable<PublicClassifiedAdListItem>> Query(
            this DbConnection connection, GetOwnersClassifiedAd query)
            => connection.QueryAsync<PublicClassifiedAdListItem>(
                "SELECT \"ClassifiedAdId\",\"Price_Amount\"price ,\"Title_Value\" title" +
                "FROM \"ClassifiedAds\"WHERE \"OWnerId_Value\"=@OWnerId LIMIT" +
                "@PageSize OFFSET @Offset",
                new
                {
                    OwnerId = query.OwnerId,
                    PageSize = query.PageSize,
                    Offset = Offset(query.Page, query.PageSize)
                });
        public static Task<ClassifiedAdDetails> Query(
            this DbConnection connection, GetPublishClassifiedAd query)
            => connection.QuerySingleOrDefaultAsync<ClassifiedAdDetails>(
                "SELECT \"ClassifiedAdId\",\"Price_Amount\"price,\"Title_Value\" title" +
                "\"Text_Value\" description,\"DisplayName_Value\" sellerdisplayname" +
                "FROM \"CLassifiedAds\",\"UserProfiles\"" +
                "WHERE \"ClassifiedAdId\"=@Id AND" +
                "\"OwnerId_Value\"=\"ÜserProfileId\"",
                new { Id = query.ClassifiedAdId }
                );
        private static int Offset(int page, int pageSize)=>page*pageSize;
    }
}
