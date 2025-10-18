using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.BLL.Models;
using Twilio.Rest.Api.V2010.Account;

namespace Company.BLL.Interfaces
{
    public interface ITwilioService
    {
        public Task<MessageResource> SendSms(Sms sms);
    }
}
