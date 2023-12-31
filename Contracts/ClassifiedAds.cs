﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
   public static class ClassifiedAds
    {
        public static class V1
        {
            public class Create
            {
                public Guid Id { get; set;} 
                public Guid OwnerId { get; set;}     
            }
            public class SetTitle
            {

                public Guid Id { get; set; }
                public string Title { get; set; }   
            }
            public class UpateText
            {
                public Guid Id { get; set; }
                public string Text { get; set; }
            }
            public class UpdatePrice
            {
                public Guid Id { get; set; }
                public decimal Price { get; set; }
                public string Currency { get; set; }
            }
            public class RequestToPublish
            {
                public Guid Id { get; set; }
            }
        }
    }
}
