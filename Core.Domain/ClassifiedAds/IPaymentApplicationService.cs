using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.ClassifiedAds
{
    public interface IPaymentApplicationService
    {
        Guid Authorize(
            string creditCardNumber,
        int expiryYear,
        int expiryMonth,
        int cvcCode,
        int camount);
        void Capture(Guid authorizationId);


    }
}
