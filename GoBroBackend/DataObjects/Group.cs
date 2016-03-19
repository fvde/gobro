using Microsoft.WindowsAzure.Mobile.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoBroBackend.DataObjects
{
    public class Group : EntityData
    {
        public string DisplayName { get; set; }
        public virtual Challenge CurrentChallenge { get; set; }
        public virtual ICollection<Challenge> Challenges { get; set; }
        public virtual ICollection<Challenge> PastChallenges { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public DateTimeOffset LastChallengeChange { get; set; }
    }
}