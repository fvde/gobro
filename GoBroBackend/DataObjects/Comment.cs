using Microsoft.WindowsAzure.Mobile.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoBroBackend.DataObjects
{
    public class Comment : EntityData
    {
        public virtual Entry Entry { get; set; }
        public virtual User User { get; set; }
        public string Content { get; set; }
    }
}