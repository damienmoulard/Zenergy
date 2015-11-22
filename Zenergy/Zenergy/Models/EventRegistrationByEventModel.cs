using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zenergy.Models
{
    public class EventRegistrationByEventModel
    {
        public int eventId { get; set; }
        public List<user> registeredUsers { get; set; }
    }
}