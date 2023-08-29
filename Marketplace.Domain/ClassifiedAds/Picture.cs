﻿using Marketplace.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Domain.ClassifiedAds
{
    public class Picture : Entity<PictureId>
    {
        public Guid PictureId
        {
            get => Id.Value;
            set { }
        }
        public ClassifiedAdId ParentId { get; private set; }
        internal PictureSize Size { get; private set; }
        internal string Location { get; private set; }
        internal int Order { get; private set; }
        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.PictureAddToClassifiedAd e:
                    ParentId = new ClassifiedAdId(e.ClassifiedAdId);
                    Id = new PictureId(e.PictureId);
                    Location = e.Url;
                    Size = new PictureSize { Height = e.Height, Width = e.Width };
                    Order = e.Order;
                    break;
                case Events.ClassifiedAdPictureResize e:
                    Size = new PictureSize { Height = e.Height, Width = e.Width, };
                    //Size = new PictureSize { Height = e.Height, Width = e.Width };
                    break;
            }
        }

        public void Resize(PictureSize newSize)
            => Apply(new Events.ClassifiedAdPictureResize
            {
                PictureId = Id.Value,
                Height = newSize.Height,
                Width = newSize.Width
            });

        public Picture(Action<object> applier) : base(applier) { }

    }
    public class PictureId : Value<PictureId>
    {
        public PictureId(Guid value) => Value = value;

        public Guid Value { get; }
        protected PictureId() { }
    }

}
