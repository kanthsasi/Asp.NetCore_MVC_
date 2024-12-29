using Microsoft.EntityFrameworkCore;
using TopSpeed.Application.Contracts.Presistance;
using TopSpeed.Domain.Models;
using TopSpeed.Infrastructure.Common;

namespace TopSpeed.Infrastructure.Repositories
{
    public class BrandRepository : GenericRepository<Brand> , IBrandRepository
    {
        public BrandRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<Brand> Update(Brand brand, Guid id)
        {
           var existingData = await dbContext.Brand.FirstOrDefaultAsync(x => x.Id == id);

            if (existingData == null)
            {
                return null;
            }

            existingData.Name = brand.Name;

            existingData.EstablishedYear = brand.EstablishedYear;

            if (brand.BrandLogo == null)
            {
                return null as Brand;
            }

            existingData.BrandLogo = brand.BrandLogo;

           return existingData;
        }
    }
}
