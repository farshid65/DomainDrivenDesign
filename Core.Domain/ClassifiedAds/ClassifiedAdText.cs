using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.ClassifiedAds
{
    public class ClassifiedAdText : Value<ClassifiedAdText>
    {
        protected ClassifiedAdText() { }
        public string Value { get; }
        internal ClassifiedAdText(string text) => Value = text;

        public static ClassifiedAdText FromString(string text)
                => new ClassifiedAdText(text);
        public static implicit operator string(ClassifiedAdText text) => text.Value;
        public static ClassifiedAdText NoText =
            new ClassifiedAdText();
    }
}
