using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoBroBackend.Login
{
    /// <summary>
    /// This class is designed to look exactly like a built-in login result on the client, change with care!
    /// </summary>
    public class CustomLoginResult
    {
        public string UserId { get; set; }
        public string GroupId { get; set; }
        public string MobileServiceAuthenticationToken { get; set; }
    }
}