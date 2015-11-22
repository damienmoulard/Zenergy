using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zenergy.Models
{
    public class EventRegistrationByEventModel
    {
        public int eventId { get; set; }
        public string eventname { get; set; }
        public List<RegisteredUser> registeredUsers { get; set; }
    }

    public class RegisteredUser
    {
        public int userId { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
    }
}