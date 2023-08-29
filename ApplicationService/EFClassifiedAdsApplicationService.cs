using Core.Domain.ClassifiedAds;
using Core.Domain.Shared;
using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService
{
    public class EFClssifiedAdsApplicationService : IApplicationService
    {
        private readonly IClassifiedAdRepository _repository;
        private readonly IUnitofWork _unitofWork;
        private readonly ICurrencyLookup _currencyLookup;


        public EFClssifiedAdsApplicationService(IClassifiedAdRepository repository, IUnitofWork unitofWork, ICurrencyLookup currencyLookup)
        {
            _repository = repository;
            _unitofWork = unitofWork;
            _currencyLookup = currencyLookup;
        }
        public Task Handle(object command)
        => command switch
        {
            ClassifiedAds.V1.Create cmd => HandleCreate(cmd),
            ClassifiedAds.V1.SetTitle cmd =>
            HandleUpdate(
                cmd.Id,
                c => c.SetTitle(
                    ClassifiedAdTitle.Fromstring(cmd.Title))),
            ClassifiedAds.V1.UpateText cmd =>
            HandleUpdate(
                cmd.Id,
                c => c.UpdateText(
                    ClassifiedAdText.FromString(cmd.Text))),
            ClassifiedAds.V1.UpdatePrice cmd =>
            HandleUpdate(
                cmd.Id,
                c => c.UpdatePrice(
                    Price.FromDecimal(cmd.Price, cmd.Currency, _currencyLookup))),
            ClassifiedAds.V1.RequestToPublish cmd =>
            HandleUpdate(
                cmd.Id,
                c => c.RequestToPublish())
        };
        private async Task HandleCreate(ClassifiedAds.V1.Create cmd)
        {
            if (await _repository.ExistsAsync(cmd.Id.ToString()))
                throw new InvalidOperationException(
                    $"Entity with id {cmd.Id} already exists");
            var classifiedAd = new ClassifiedAd(
                new ClassifiedAdId(cmd.Id),
                new UserId(cmd.OwnerId));
            await _repository.Add(classifiedAd);
            await _unitofWork.Commit();
        }
        private async Task HandleUpdate(
            Guid classifiedAdId, Action<ClassifiedAd> operation)
        {
            var classifiedAd = await
                _repository.LoadAsync(classifiedAdId.ToString());
            if (classifiedAd == null)
                throw new InvalidOperationException(
                    $"Entity with id {classifiedAdId} can not be found");
            operation(classifiedAd);
            await _unitofWork.Commit();

        }
    }
}
