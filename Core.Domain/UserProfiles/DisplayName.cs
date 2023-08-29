using Core.Domain.Shared;
using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core.Domain.UserProfile
{
    public class DisplayName:Value<DisplayName>
    {
        public string Value { get;  }
        internal DisplayName(string dispayName)=>Value = dispayName;
        public static DisplayName FromString(string displayName,CheckTextForProfanity hasProfanity)
        {
            if (displayName.IsEmpty())
                throw new ArgumentNullException(nameof(FullName));
            if(hasProfanity(displayName)) 
                throw new DomainException.ProfanityFound(displayName);
            return new DisplayName(displayName);

        }
            public static implicit operator string(DisplayName displayName) 
            =>displayName.Value;
        protected DisplayName() { }
    }
}
