using CRM.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    [TagEntity("User","user","UserModelSlot")]
    public class UserModel
    {
        public int Id { get; set; }

        [TagField("Username","username")]
        public string Username { get; set; }

        [TagField("Password", "password")]
        public string Password { get; set; }

        [TagField("Email", "email")]
        public string Email { get; set; }
        
        public string Roles { get; set; }
    }
}