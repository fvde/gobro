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
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using AutoMapper;
using System.Web.Http.Results;

namespace GoBroBackend.Controllers
{
    [AuthorizeLevel(Microsoft.WindowsAzure.Mobile.Service.Security.AuthorizationLevel.Anonymous)]
    public class EntryController : TableController<EntryDto>
    {
        private MobileServiceContext context;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            context = new MobileServiceContext();
            DomainManager = new SimpleMappedEntityDomainManager<EntryDto, Entry>(
                context, Request, Services);
        }

        // GET tables/Entry
        public IQueryable<EntryDto> GetAllEntry()
        {
            return Query(); 
        }

        // GET tables/Entry/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<EntryDto> GetEntry(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Entry/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<EntryDto> PatchEntry(string id, Delta<EntryDto> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Entry
        public async Task<IHttpActionResult> PostEntry(EntryDto item)
        {
            string storageAccountName;
            string storageAccountKey;

            // Try to get the Azure storage account token from app settings.  
            if (!(Services.Settings.TryGetValue("STORAGE_ACCOUNT_NAME", out storageAccountName) |
            Services.Settings.TryGetValue("STORAGE_ACCOUNT_ACCESS_KEY", out storageAccountKey)))
            {
                Services.Log.Error("Could not retrieve storage account settings.");
            }

            // user 
            var user = context.Users.Find(item.UserId);

            if (user == null)
            {
                return new BadRequestResult(this);
            }

            // challenge
            var challenge = context.Challenges.Find(item.ChallengeId);

            // check if user already submitted for this challenge
            foreach (Entry e in challenge.Entries)
            {
                if (e.User.Id == user.Id)
                {
                    // TODO
                    //return new BadRequestResult(this);
                }
            }

            // create real item
            Entry newItem = Mapper.Map<EntryDto, Entry>(item);

            newItem.Id = Guid.NewGuid().ToString();
            newItem.CreatedAt = DateTimeOffset.Now;
            newItem.User = user;
            newItem.Challenge = challenge;

            // Set the URI for the Blob Storage service.
            Uri blobEndpoint = new Uri(string.Format("https://{0}.blob.core.windows.net", storageAccountName));

            // Create the BLOB service client.
            CloudBlobClient blobClient = new CloudBlobClient(blobEndpoint,
                new StorageCredentials(storageAccountName, storageAccountKey));

            if (item != null && !String.IsNullOrEmpty(item.ContainerName))
            {
                // Set the BLOB store container name on the item, which must be lowercase.
                newItem.ContainerName = item.ContainerName.ToLower();

                // Create a container, if it doesn't already exist.
                CloudBlobContainer container = blobClient.GetContainerReference(newItem.ContainerName);
                await container.CreateIfNotExistsAsync();

                // Create a shared access permission policy. 
                BlobContainerPermissions containerPermissions = new BlobContainerPermissions();

                // Enable anonymous read access to BLOBs.
                containerPermissions.PublicAccess = BlobContainerPublicAccessType.Blob;
                container.SetPermissions(containerPermissions);

                // Define a policy that gives write access to the container for 5 minutes.                                   
                SharedAccessBlobPolicy sasPolicy = new SharedAccessBlobPolicy()
                {
                    SharedAccessStartTime = DateTime.UtcNow,
                    SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(5),
                    Permissions = SharedAccessBlobPermissions.Write
                };

                // Get the SAS as a string.
                newItem.SasQueryString = container.GetSharedAccessSignature(sasPolicy);

                // Set the URL used to store the image.
                newItem.ImageUri = string.Format("{0}{1}/{2}", blobEndpoint.ToString(),
                    newItem.ContainerName, item.ResourceName);
            }

            // store in entries
            context.Entries.Add(newItem);

            // store in challenge as well
            challenge.Entries.Add(newItem);

            // store
            await context.SaveChangesAsync();

            //convert back to dto before sending back
            EntryDto current = Mapper.Map<Entry, EntryDto>(newItem);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Entry/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteEntry(string id)
        {
             return DeleteAsync(id);
        }

    }
}