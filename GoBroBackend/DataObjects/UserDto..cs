using Microsoft.WindowsAzure.Mobile.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoBroBackend.DataObjects
{
    public class UserDto : EntityData
    {
        public string DisplayName { get; set; }
        public string GroupId { get; set; }
    }
}