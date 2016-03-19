using GoBroBackend.DataObjects;
using GoBroBackend.Models;
using GoBroBackend.Utilities;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using thestoryShared.Misc;

namespace GoBroBackend.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Anonymous)]
    public class CustomRegistrationController : ApiController
    {
        public ApiServices Services { get; set; }

        // POST api/CustomRegistration
        public async Task<HttpResponseMessage> Post(RegistrationRequest registrationRequest)
        {
            if (!Helpers.IsValidEmail(registrationRequest.Email))
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid email");
            }
            else if (!Helpers.IsValidPassword(registrationRequest.Password))
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid password (at least 4 chars, at most 30 chars required)");
            }

            MobileServiceContext context = new MobileServiceContext();
            User account = context.Users.Where(a => a.Email == registrationRequest.Email).SingleOrDefault();
            if (account != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "That email has already been used.");
            }
            else
            {
                byte[] salt = CustomLoginProviderUtils.generateSalt();

                User newAccount = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    DisplayName = registrationRequest.DisplayName,
                    Email = registrationRequest.Email,
                    Salt = salt,
                    SaltedAndHashedPassword = CustomLoginProviderUtils.hash(registrationRequest.Password, salt)
                };

                // TODO enable user to choose group
                // insert into group
                var group = context.Groups.Find("default");
                group.Users.Add(newAccount);

                context.Users.Add(newAccount);

                await context.SaveChangesAsync();
                return this.Request.CreateResponse(HttpStatusCode.Created);
            }
        }
    }
}