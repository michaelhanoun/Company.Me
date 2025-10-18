using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.BLL.Interfaces;
using Company.BLL.Models;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Company.BLL.Services
{
    public class TwilioService(IOptions<TwilioSettings>_options) : ITwilioService
    {
        public async Task<MessageResource> SendSms(Sms sms)
        {
            TwilioClient.Init(_options.Value.AccountSID, _options.Value.AuthToken);
            var message = await MessageResource.CreateAsync(body:sms.Body,to:new Twilio.Types.PhoneNumber(sms.To), from: new Twilio.Types.PhoneNumber (_options.Value.PhoneNumber));
            return message;
        }
    }
}
