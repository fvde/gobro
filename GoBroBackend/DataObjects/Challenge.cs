using Microsoft.WindowsAzure.Mobile.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoBroBackend.DataObjects
{
    public class Challenge : EntityData
    {
        public virtual Group Group { get; set; }
        public virtual ICollection<Entry> Entries { get; set; }
        public string Content { get; set; }
        public bool IsUserGenerated { get; set; }
    }
}