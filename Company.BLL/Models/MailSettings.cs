using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Models
{
    public class MailSettings
    {
        public string SenderEmail { get; set; }
        public string DisplayName { get; set; }
        public string SenderPassword { get; set; }
        public string SmtpClientServer { get; set; }
        public int SmtpClientPort { get; set; }
    }
}
