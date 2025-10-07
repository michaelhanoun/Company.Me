using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.BLL.Models;

namespace Company.BLL.Interfaces
{
    public interface IEmailSender
    {
        Task<bool> SendEmail(EmailMessage message);
    }
}
