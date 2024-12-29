using Microsoft.AspNetCore.Identity;
using TopSpeed.Application.Services.Interface;

namespace TopSpeed.Application.Services
{
    public class UserNameService : IUserNameService
    {
        private readonly UserManager<IdentityUser> userManager;

        public UserNameService(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<string> GetUserName(string userId)
        {
           if (String.IsNullOrEmpty(userId))
           {
                return String.Empty;
           }

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return "NA";
            }

            return user.UserName;

        }
    }
}
