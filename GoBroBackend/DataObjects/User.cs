using Microsoft.WindowsAzure.Mobile.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoBroBackend.DataObjects
{
    public class User : EntityData
    {
        public string DisplayName { get; set; }
        public virtual Group Group { get; set; }
        public string Email { get; set; }
        public string MicrosoftAccountId { get; set; }
        public byte[] Salt { get; set; }
        public byte[] SaltedAndHashedPassword { get; set; }
    }
}