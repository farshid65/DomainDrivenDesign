using Core.Domain.ClassifiedAds;
using Core.Domain.Shared;
using Framework;
using Infrastructure.ClassifiedAd;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ESClassifiedAdApplicationService:IApplicationService
    {
        private readonly ICurrencyLookup _currencyLookup;
        private readonly IAggregateStore _store;

        public ESClassifiedAdApplicationService(ICurrencyLookup currencyLookup,IAggregateStore store)
        {
            _currencyLookup = currencyLookup;
            _store = store;
        }

        public Task Handle(object command) =>
            command switch
            {
                ClassifiedAds.V1.Create cmd =>
                HandleCreate(cmd),
                ClassifiedAds.V1.SetTitle cmd=>
                HandleUpdate(
                    cmd.Id,
                    c=>c.SetTitle(
                       ClassifiedAdTitle.Fromstring(cmd.Title)
                        )
                    ),
                ClassifiedAds.V1.UpateText cmd=>
                HandleUpdate(
                    cmd.Id,
                    c=>c.UpdateText(
                        ClassifiedAdText.FromString(cmd.Text)
                        )
                    ),
                ClassifiedAds.V1.UpdatePrice cmd=>
                HandleUpdate(
                    cmd.Id,
                    c=>c.UpdatePrice(
                        Price.FromDecimal(
                            cmd.Price,cmd.Currency,_currencyLookup)
                        )
                    ),
                ClassifiedAds.V1.RequestToPublish cmd=>
                HandleUpdate(
                    cmd.Id,
                    c=>c.RequestToPublish()
                    ),
                _=>Task.CompletedTask

                
            };
        private async Task HandleCreate(ClassifiedAds.V1.Create cmd)
        {
            if (await _store.Exists<Core.Domain.ClassifiedAds.ClassifiedAd,ClassifiedAdId>
                (new ClassifiedAdId(cmd.Id))) 
                    throw new InvalidOperationException(
                        $"Entity with id{cmd.Id} already exists");
            var classifiedAd = new Core.Domain.ClassifiedAds.ClassifiedAd(
                new ClassifiedAdId(cmd.Id),
                new UserId(cmd.OwnerId)
                );
            await _store.Save<Core.Domain.ClassifiedAds.ClassifiedAd,ClassifiedAdId>( classifiedAd );                    
        }
        private Task HandleUpdate(
            Guid id,Action<Core.Domain.ClassifiedAds.ClassifiedAd>update)
        =>this.HandleUpdate(_store,new ClassifiedAdId(id),update);

    }
}
