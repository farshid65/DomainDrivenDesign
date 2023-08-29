using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Shared
{
    public class UserId:Value<UserId>
    {
        protected UserId() { }
        public Guid Value { get; set; }

        public UserId(Guid value)
        {
            if (value == default)
            {
                throw new ArgumentNullException("User id cannot be empty", nameof(value));
            }
            Value = value;
        }
        public static implicit operator Guid(UserId self) => self.Value;
        public static UserId NoUser =
            new UserId();
    }
}
