using Microsoft.EntityFrameworkCore;
using TopSpeed.Application.Contracts.Presistance;
using TopSpeed.Domain.Models;
using TopSpeed.Infrastructure.Common;

namespace TopSpeed.Infrastructure.Repositories
{
    public class VehicalTypeRepository : GenericRepository<VehicalType>, IVehicalTypeRepository
    {
        public VehicalTypeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<VehicalType> Update(Guid id, VehicalType vehicalType)
        {
           var existingData = await dbContext.VehicleType.FirstOrDefaultAsync(x => x.Id == id);

            if (existingData == null)
            {
                return null;
            }

            existingData.Name = vehicalType.Name;

            return existingData;
        }
    }
}
