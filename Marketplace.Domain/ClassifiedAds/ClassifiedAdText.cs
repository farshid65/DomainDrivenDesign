﻿using Marketplace.Domain;
using Marketplace.Framework;

public class ClassifiedAdText:Value<ClassifiedAdText>
    {
        public string Value { get;  }
        internal ClassifiedAdText( string text)=>Value= text;
    
    public static ClassifiedAdText FromString(string text)
            =>new ClassifiedAdText(text);
        public static implicit operator string(ClassifiedAdText text) => text.Value;
    }






