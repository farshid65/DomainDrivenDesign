using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core.Domain.UserProfile
{
    public class FullName:Value<FullName>
    {
        public string Value { get; private set; }
        internal FullName(string value) => Value = value;
        public static FullName FromString(string fullName)
        {
           if(fullName.IsEmpty())
                throw new ArgumentNullException(nameof(fullName));
            return new FullName(fullName);
        }
        public static implicit operator string(FullName fullName)
            => fullName.Value;
        protected FullName() { }
        
    }
    
}
