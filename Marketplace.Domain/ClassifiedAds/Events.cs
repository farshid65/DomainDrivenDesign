﻿namespace Marketplace.Domain.ClassifiedAds
{
    public static class Events
    {
        public class ClassifiedAdCreated
        {
            public Guid Id { get; set; }
            public Guid OwnerId { get; set; }
        }
        public class ClassifiedAdTitleChanged
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
        }
        public class ClassifiedAdTextUpdate
        {

            public Guid Id { get; set; }
            public string AdText { get; set; }
        }
        public class ClassifiedAdPriceUpdate
        {
            public Guid Id { get; set; }
            public decimal Price { get; set; }
            public string? CurrencyCode { get; set; }
        }
        public class ClassifiedAdSentForReview
        {
            public Guid Id { get; set; }
        }
        public class PictureAddToClassifiedAd
        {
            public Guid ClassifiedAdId { get; set; }
            public Guid PictureId { get; set; }
            public string Url { get; set; }
            public int Height { get; set; }
            public int Width { get; set; }
            public int Order { get; set; }
        }
        public class ClassifiedAdPictureResize
        {
            public Guid PictureId { get; set; }
            public int Height { get; set; }
            public int Width { get; set; }
        }
    }

}






