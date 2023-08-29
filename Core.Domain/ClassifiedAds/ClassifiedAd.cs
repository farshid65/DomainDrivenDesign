using Core.Domain.Shared;
using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.ClassifiedAds
{
    public class ClassifiedAd : AggregateRoot<ClassifiedAdId>
    {

        public Guid ClassifiedAdId { get; private set; }
        protected ClassifiedAd() { }
        public UserId OwnerId { get; private set; }
        public ClassifiedAdTitle Title { get; private set; }
        public ClassifiedAdText Text { get; private set; }
        public Price Price { get; private set; }
        public ClassifiedAdState State { get; private set; }
        public UserId ApprovedBy { get; private set; }
        public List<Picture> Pictures { get; private set; }
        public ClassifiedAd(Guid id, Guid ownerId)
        {
            //if (id == default)
            //{
            //    throw new ArgumentException("Identity must be specified", nameof(id));
            //}
            //if (ownerId==default)
            //{
            //    throw new ArgumentException("Owner id must be specified", nameof(ownerId));
            //}
            //Id = id;
            //OwnerId= ownerId;
            //State = ClassifiedAdState.InActive;
            //EnsureValidState();

            Pictures = new List<Picture>();
            Apply(new Events.ClassifiedAdCreated
            {
                Id = id,
                OwnerId = ownerId,


            });
        }
        //private string DbId
        //{
        //    get => $"classifiedAd/{Id.Value}";
        //    set { }
        //}

        public void SetTitle(ClassifiedAdTitle title)
        {
            //Title = title;
            //EnsureValidState();
            Apply(new Events.ClassifiedAdTitleChanged
            {
                Id = Id,
                Title = title
            });
        }
        public void UpdateText(ClassifiedAdText text)
        {
            //Text = text;
            //EnsureValidState();
            Apply(new Events.ClassifiedAdTextUpdate
            {
                Id = Id,
                AdText = text
            });
        }
        public void UpdatePrice(Price price)
        {
            //Price = price;
            //EnsureValidState();
            Apply(new Events.ClassifiedAdPriceUpdate
            {
                Id = Id,
                Price = Price.Amount,
                CurrencyCode = Price.Currency.CurrencyCode
            });
        }
        public void RequestToPublish()
        {
            //State = ClassifiedAdState.PendingReview;
            //EnsureValidState();
            Apply(new Events.ClassifiedAdSentForReview
            {
                Id = Id
            });
        }
        public void AddPicture(Uri pictureUri, PictureSize size) =>
            Apply(new Events.PictureAddToClassifiedAd
            {
                PictureId = new Guid(),
                ClassifiedAdId = Id,
                Url = pictureUri.ToString(),
                Height = size.Height,
                Width = size.Width,
                Order = Pictures.Max(x => x.Order)
            });
        private Picture FindPicture(PictureId id)
            => Pictures.FirstOrDefault(x => x.Id == id);
        public void ResizePicture(PictureId pictureId, PictureSize newSize)
        {
            var picture = FindPicture(pictureId);
            if (picture == null)
                throw new InvalidOperationException(
                    "cannot resize a picture that I don't have");
            picture.Resize(newSize);
        }
        protected override void When(object @event)
        {
            Picture picture;
            switch (@event)
            {
                case Events.ClassifiedAdCreated e:
                    Id = new ClassifiedAdId(e.Id);
                    OwnerId = new UserId(e.OwnerId);
                    State = ClassifiedAdState.InActive;
                    Title = ClassifiedAdTitle.NoTitle;
                    Text = ClassifiedAdText.NoText;
                    Price = Price.NoPrice;
                    ApprovedBy = UserId.NoUser;
                    ClassifiedAdId = e.Id;
                    break;
                case Events.ClassifiedAdTitleChanged e:
                    Title = new ClassifiedAdTitle(e.Title);
                    break;
                case Events.ClassifiedAdTextUpdate e:
                    Text = new ClassifiedAdText(e.AdText);
                    break;
                case Events.ClassifiedAdPriceUpdate e:
                    Price = new Price(e.Price, e.CurrencyCode);
                    break;
                case Events.ClassifiedAdSentForReview _:
                    State = ClassifiedAdState.PendingReview;
                    break;
                case Events.PictureAddToClassifiedAd e:
                    var newPicture = new Picture(Apply);
                    ApplyToEntity(newPicture, e);
                    Pictures.Add(newPicture);
                    break;


            }
        }
        protected override void EnsureValidState()
        {

            var valid =
                Id != null &&
                OwnerId != null &&
                State switch
                {
                    ClassifiedAdState.PendingReview =>
                    Title != null
                    && Text != null
                    && Price.Amount > 0,
                    ClassifiedAdState.Active =>
                    Title != null
                    && Text != null
                    && Price.Amount > 0
                    && ApprovedBy != null,
                    _ => true
                };
            if (!valid)
            {
                throw new InvalidEntityStateException(this, $"Post check faied in state {State}");
            }
        }

        public enum ClassifiedAdState
        {
            PendingReview,
            Active,
            InActive,
            MarkedAsSold
        }
    }
}
