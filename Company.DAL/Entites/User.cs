using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Company.DAL.Entites
{
    public class User : IdentityUser
    {
        public string FName { get; set; }
        public string LName { get; set; }
        public bool IsAgree { get; set; }
    }
}
