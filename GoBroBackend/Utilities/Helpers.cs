using Microsoft.WindowsAzure.Mobile.Service.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace GoBroBackend.Utilities
{
    public class Helpers
    {
        public static async Task<String> GetId(IPrincipal user)
        {
            return await GetServiceUserValue(user, "uid");
        }

        private static async Task<String> GetServiceUserValue(IPrincipal user, string claim)
        {
            var serviceUser = user as ServiceUser;
            var identities = await serviceUser.GetIdentitiesAsync();
            var result = identities.FirstOrDefault().UserId.Split(':').Last();
            return result;
        }

        public static bool IsValidEmail(String attempt)
        {
            return Regex.IsMatch(attempt, "^[a-zA-Z0-9@.]{4,}$")
                && attempt.Length <= 30;
        }

        public static bool IsValidPassword(String attempt)
        {
            return attempt.Length >= 4 && attempt.Length <= 30;
        }

        public static string GetPreview(String input, int length = 60)
        {
            if (input.Length <= length)
            {
                return input;
            }
            else
            {
                return input.Remove(length) + "...";
            }
        }
    }
}