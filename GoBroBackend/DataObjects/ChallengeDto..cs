using Microsoft.WindowsAzure.Mobile.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoBroBackend.DataObjects
{
    public class ChallengeDto : EntityData
    {
        public List<EntryDto> Entries { get; set; }
        public string GroupId { get; set; }
        public string Content { get; set; }
        public bool IsUserGenerated { get; set; }
    }
}