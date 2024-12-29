using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TopSpeed.Application.Contracts.Presistance;
using TopSpeed.Infrastructure.Common;

namespace TopSpeed.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext dbContext;
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager;
        private readonly IHttpContextAccessor httpContext;

        public UnitOfWork(ApplicationDbContext dbContext,UserManager<IdentityUser> userManager,IHttpContextAccessor httpContext)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.httpContext = httpContext;
            CarDetails = new CarDetailsRepository(dbContext);
        }

        public IBrandRepository Brand => new BrandRepository(dbContext);

        public IVehicalTypeRepository VehicalType => new VehicalTypeRepository(dbContext);

        public ICarDetailsRepository CarDetails { get; set; }



        public void Dispose()
        {
            dbContext.Dispose();
        }

        public async Task Save()
        {
            dbContext.SaveCommonFields(userManager,httpContext);
            await dbContext.SaveChangesAsync();
        }
    }
}
