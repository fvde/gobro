using Microsoft.WindowsAzure.Mobile.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoBroBackend.DataObjects
{
    public class Entry : EntityData
    {
        public virtual User User { get; set; }
        public virtual Challenge Challenge { get; set; }
        public string ContainerName { get; set; }
        public string ResourceName { get; set; }
        public string SasQueryString { get; set; }
        public string ImageUri { get; set; }
        public string Content { get; set; }
        public int Votes { get; set; }
        //TODO should have a user / vote table
    }
}