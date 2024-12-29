using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TopSpeed.Domain.Common;

namespace TopSpeed.Infrastructure.Common
{
    public static class ExtensionMethods
    {
        public static async Task<string> GetCurrentUserId(UserManager<IdentityUser> userManager, IHttpContextAccessor httpContext)
        {
            var userId = httpContext.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if(userId == null)
            {
                var user = await userManager.GetUserAsync(httpContext.HttpContext.User);
                userId = user?.Id;
            }

            return userId;
        }

        public static async void SaveCommonFields(this ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContext)
        {
            var userId = await GetCurrentUserId(userManager,httpContext);

            IEnumerable<BaseModel> insertEntites = dbContext.ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added)
                .Select(x => x.Entity)
                .OfType<BaseModel>();

            IEnumerable<BaseModel> updateEntites = dbContext.ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Modified)
                .Select(x => x.Entity)
                .OfType<BaseModel>();

            foreach (var item in insertEntites)
            {
                item.CreateOn = DateTime.UtcNow;
                item.CreatedBy = userId;
            }

            foreach (var item in updateEntites)
            {
                item.ModifiedOn = DateTime.UtcNow;
                item.ModifiedBy = userId;
            }

        }
    }
}
