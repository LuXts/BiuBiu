using System;
using System.Collections.Generic;
using System.Text;

namespace BiuBiuShare.UserManagement
{
    public class UserInfo
    {
        public ulong UserId { get; set; }
        public string UserName { get; set; }
        public string JobNumber { get; set; }
        public string  UserProfiles{ get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public ulong IconId { get; set; }
        public bool Permissions { get; set; }
        public ulong CurrentPassword { get; set; }
    }
}
