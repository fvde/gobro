using Microsoft.WindowsAzure.Mobile.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoBroBackend.DataObjects
{
    public class EntryDto : EntityData
    {
        public string UserId { get; set; }
        public string UserUsername { get; set; }
        public string ChallengeId { get; set; }
        public string ContainerName { get; set; }
        public string ResourceName { get; set; }
        public string SasQueryString { get; set; }
        public string ImageUri { get; set; }
        public string Content { get; set; }
        public int Votes { get; set; }
        public int NumberOfComments { get; set; }
    }
}