using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using GoBroBackend.Models;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using System.Threading.Tasks;

namespace GoBroBackend.Controllers
{
    [AuthorizeLevel(Microsoft.WindowsAzure.Mobile.Service.Security.AuthorizationLevel.Anonymous)]
    public class VotingController : ApiController
    {
        public ApiServices Services { get; set; }

        private MobileServiceContext context;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            context = new MobileServiceContext();
        }

        [HttpPost]
        private async Task<HttpResponseMessage> PostVote(string entryid, int vote)
        {
            if (Math.Abs(vote) > 1)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            // TODO is this atomic?
            var entry = context.Entries.FirstOrDefault(i => i.Id == entryid);
            if (entry != null)
            {
                entry.Votes += vote;
                await context.SaveChangesAsync();
                return new HttpResponseMessage(HttpStatusCode.OK);
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }
    }
}
