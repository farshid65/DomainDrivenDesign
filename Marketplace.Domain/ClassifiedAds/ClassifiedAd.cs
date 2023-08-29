using Marketplace.Domain.Shared;
using Marketplace.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Marketplace.Domain.ClassifiedAds.Events;
using static System.Net.Mime.MediaTypeNames;

namespace Marketplace.Domain.ClassifiedAds
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
        public UserId AprovedBy { get; set; }
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
            Apply(new ClassifiedAdCreated
            {
                Id = id,
                OwnerId = ownerId

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
            Apply(new ClassifiedAdTitleChanged
            {
                Id = Id,
                Title = title
            });
        }
        public void UpdateText(ClassifiedAdText text)
        {
            //Text = text;
            //EnsureValidState();
            Apply(new ClassifiedAdTextUpdate
            {
                Id = Id,
                AdText = text
            });
        }
        public void UpdatePrice(Price price)
        {
            //Price = price;
            //EnsureValidState();
            Apply(new ClassifiedAdPriceUpdate
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
            Apply(new ClassifiedAdSentForReview
            {
                Id = Id
            });
        }
        public void AddPicture(Uri pictureUri, PictureSize size) =>
            Apply(new PictureAddToClassifiedAd
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
                case ClassifiedAdCreated e:
                    Id = new ClassifiedAdId(e.Id);
                    OwnerId = new UserId(e.OwnerId);
                    State = ClassifiedAdState.InActive;
                    Title = ClassifiedAdTitle.NoTitle;
                    ClassifiedAdId = e.Id;
                    break;
                case ClassifiedAdTitleChanged e:
                    Title = new ClassifiedAdTitle(e.Title);
                    break;
                case ClassifiedAdTextUpdate e:
                    Text = new ClassifiedAdText(e.AdText);
                    break;
                case ClassifiedAdPriceUpdate e:
                    Price = new Price(e.Price, e.CurrencyCode);
                    break;
                case ClassifiedAdSentForReview _:
                    State = ClassifiedAdState.PendingReview;
                    break;
                case PictureAddToClassifiedAd e:
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
                    && AprovedBy != null,
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






