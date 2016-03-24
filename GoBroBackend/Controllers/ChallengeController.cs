using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using GoBroBackend.DataObjects;
using GoBroBackend.Models;
using GoBroBackend.Utilities;
using System;
using System.Web.Http.Results;
using AutoMapper;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace GoBroBackend.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Anonymous)]
    public class ChallengeController : TableController<ChallengeDto>
    {
        private MobileServiceContext context;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            context = new MobileServiceContext();
            DomainManager = new SimpleMappedEntityDomainManager<ChallengeDto, Challenge>(
                context, Request, Services);
        }

        // GET tables/Challenge
        public IQueryable<ChallengeDto> GetAllChallenge()
        {
            return Query(); 
        }

        // GET tables/Challenge/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public ChallengeDto GetChallenge(string groupid)
        {
            // get group 
            var group = context.Groups.Find(groupid);

            if (group != null)
            {
                return Mapper.Map<Challenge, ChallengeDto>(group.CurrentChallenge);
            }

            return null;
        }

        // PATCH tables/Challenge/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<ChallengeDto> PatchChallenge(string id, Delta<ChallengeDto> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Challenge
        public async Task<IHttpActionResult> PostChallenge(ChallengeDto item)
        {
            // check data quality
            if (String.IsNullOrEmpty(item.Content))
            {
                return new BadRequestResult(this);
            }

            // find group for later use
            var group = context.Groups.Find(item.GroupId);

            if (group == null)
            {
                return new BadRequestResult(this);
            }

            // Create new instance of db object
            Challenge newItem = Mapper.Map<ChallengeDto, Challenge>(item);

            newItem.Id = Guid.NewGuid().ToString();
            newItem.CreatedAt = DateTimeOffset.Now;
            newItem.Group = group;

            // Insert challenge
            context.Challenges.Add(newItem);
            group.Challenges.Add(newItem);

            // Set the new challenge as currently active challenge if there is no other
            if (group.CurrentChallenge == null)
            {
                group.CurrentChallenge = newItem;
            }

            // store
            await context.SaveChangesAsync();

            //convert back to dto before sending back
            ChallengeDto current = Mapper.Map<Challenge, ChallengeDto>(newItem);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Challenge/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteChallenge(string id)
        {
             return DeleteAsync(id);
        }

    }
}