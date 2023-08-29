using Microsoft.EntityFrameworkCore;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Marketplace.Infrastructure.ClassifiedAd.ReadModels;

namespace Marketplace.Infrastructure.ClassifiedAd
{
    public static class Queries
    {
        public static Task<List<PublicClassifiedAdListItem>>
            Query(
            this IAsyncDocumentSession session,
            QueryModels.GetPublishClassifiedAds query) =>
            session.Query<Marketplace.Domain.ClassifiedAds.ClassifiedAd>()
            .Where(x => x.State == Domain.ClassifiedAds.ClassifiedAd.ClassifiedAdState.Active)
            .Select(x => new PublicClassifiedAdListItem
            {
                ClassifiedAdId = x.Id.Value,
                Price = x.Price.Amount,
                Title = x.Title.Value,
                CurrencyCode = x.Price.Currency.CurrencyCode
            })
            .PageList(query.Page, query.PageSize);
        public static Task <List<PublicClassifiedAdListItem>>Query(
            this IAsyncDocumentSession session,
            QueryModels.GetOwnersClassifiedAd query)=>
            session.Query<Marketplace.Domain.ClassifiedAds.ClassifiedAd>()
            .Where(x=>x.OwnerId.Value == query.OwnerId)
            .Select(
                x=>
                new PublicClassifiedAdListItem
                {
                    ClassifiedAdId=x.Id.Value,
                    Price = x.Price.Amount,
                    Title = x.Title.Value,
                    CurrencyCode=x.Price.Currency.CurrencyCode
                })
            .PageList(query.Page, query.PageSize);
        public static Task<ClassifiedAdDetails> Query(
            this IAsyncDocumentSession session,
            QueryModels.GetPublishClassifiedAd query)
        => (from ad in session.Query<Marketplace.Domain.ClassifiedAds.ClassifiedAd>()
            where ad.Id.Value == query.ClassifiedAdId
            let user = RavenQuery
            .Load<Marketplace.Domain.UserProfiles.UserProfile>(
                "UserProfile" + ad.OwnerId.Value)
            select new ClassifiedAdDetails
            {
                ClassifiedAdId = ad.Id.Value,
                Title = ad.Title.Value,
                Description = ad.Text.Value,
                Price = ad.Price.Amount,
                CurrencyCode = ad.Price.Currency.CurrencyCode,
                SellerDisplayName = user.DisplayName.Value
            }).SingleAsync();

        
            
        private static Task<List<T>> PageList<T>(
            this IRavenQueryable<T>query,int page,int pageSize) 
            =>
            query.Skip(page*pageSize)
            .Take(pageSize)
            .ToListAsync();

    }
}
