using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using System;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Threading;
using Microsoft.WindowsAzure.Mobile.Service.ScheduledJobs;
using GoBroBackend.Models;
using System.Linq;
using GoBroBackend.DataObjects;

namespace GoBroBackend.ScheduledJobs
{
    // POST request to the path "/jobs/app".
    public class AppControlJob : ScheduledJob
    {
        private MobileServiceContext context;
        private Random random;

        protected override void Initialize(ScheduledJobDescriptor scheduledJobDescriptor,
            CancellationToken cancellationToken)
        {
            base.Initialize(scheduledJobDescriptor, cancellationToken);

            // Create a new context with the supplied schema name.
            context = new MobileServiceContext();
        }



        public override async Task ExecuteAsync()
        {
            Services.Log.Info("App control initialized!");
            random = new Random(DateTime.Now.Second);

            try
            {
                await UpdateGroups();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
            }

            catch (Exception e)
            {
                Services.Log.Error("Critical Exception: " + e.ToString());
            }
        }

        private async Task UpdateGroups()
        {
            Services.Log.Info("Updating groups...");

            var groups = context.Groups.ToList();

            foreach (Group g in groups)
            {
                if (DateTimeOffset.Now - g.LastChallengeChange > TimeSpan.FromMinutes(60) - TimeSpan.FromSeconds(30))
                {
                    var challenges = g.Challenges.ToList();

                    if (challenges.Count() == 0)
                    {
                        // if we dont have any challenges use the built in ones
                        var builtInChallenges = context.Challenges.Where(c => c.IsUserGenerated == false);
                        challenges = challenges.Concat(builtInChallenges).ToList();
                    }
                    
                    var nextChallenge = challenges.Skip(Math.Min(random.Next(challenges.Count()), challenges.Count())).FirstOrDefault();

                    g.PastChallenges.Add(g.CurrentChallenge);
                    g.CurrentChallenge = nextChallenge;
                    g.Challenges.Remove(nextChallenge);

                    // update time
                    g.LastChallengeChange = DateTimeOffset.Now;
                }
            }

            await context.SaveChangesAsync();
        }
    }
}