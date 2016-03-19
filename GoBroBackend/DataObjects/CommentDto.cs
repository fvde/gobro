using Microsoft.WindowsAzure.Mobile.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoBroBackend.DataObjects
{
    public class CommentDto : EntityData
    {
        public string EntryId { get; set; }
        public new DateTimeOffset CreatedAt { get; set; }
        public string UserId { get; set; }
        public string UserUsername { get; set; }
        public string Content { get; set; }
    }
}