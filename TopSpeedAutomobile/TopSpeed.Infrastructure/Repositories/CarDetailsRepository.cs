using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TopSpeed.Application.Contracts.Presistance;
using TopSpeed.Domain.Models;
using TopSpeed.Infrastructure.Common;

namespace TopSpeed.Infrastructure.Repositories
{
    public class CarDetailsRepository : GenericRepository<CarDetails>, ICarDetailsRepository
    {
        public CarDetailsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<List<CarDetails>> GetAllCarDetails()
        {
            return await dbContext.CarDetails.Include(x => x.Brand).Include(x => x.VehicalType).ToListAsync();
        }

        public async Task<CarDetails> GetCarDetailsById(Guid id)
        {
            var existingId = await dbContext.CarDetails.Include(x => x.Brand).Include(x => x.VehicalType).FirstOrDefaultAsync(x => x.Id == id);

            if (existingId == null)
            {
                return null;
            }

            return existingId;
        }

        public async Task<CarDetails> Update(Guid id, CarDetails carDetails)
        {
            var existingData = await dbContext.CarDetails.FirstOrDefaultAsync(x => x.Id == id);

            if (existingData == null)
            {
                return null;
            }

            existingData.BrandId = carDetails.BrandId;
            existingData.VehicalTypeId = carDetails.VehicalTypeId;
            existingData.Name = carDetails.Name;
            existingData.EngineAndFuelType = carDetails.EngineAndFuelType;
            existingData.Transmission = carDetails.Transmission;
            existingData.Engine = carDetails.Engine;
            existingData.Range = carDetails.Range;
            existingData.Ratings = carDetails.Ratings;
            existingData.SeatingCapacity = carDetails.SeatingCapacity;
            existingData.Mileage = carDetails.Mileage;
            existingData.PriceFrom = carDetails.PriceFrom;
            existingData.PriceTo = carDetails.PriceTo;
            existingData.TopSpeed = carDetails.TopSpeed;

            if (carDetails.VehicalImage == null)
            {
                return null;
            }

            existingData.VehicalImage = carDetails.VehicalImage;
            return existingData;
        }

        public async Task<List<CarDetails>> GetAllCarDetails(Guid? skipRecord, Guid? brandId)
        {
            var query = dbContext.CarDetails.Include(x => x.Brand).Include(x => x.VehicalType).OrderByDescending(x => x.ModifiedOn);

            if (brandId == Guid.Empty)
            {
                return await query.ToListAsync();
            }

            if (brandId != Guid.Empty)
            {
                query = (IOrderedQueryable<CarDetails>)query.Where(x => x.BrandId == brandId);
            }

            var carDetails = await query.ToListAsync();

            if (skipRecord.HasValue)
            {
                var recordToRemove = carDetails.FirstOrDefault(x => x.Id == skipRecord.Value);
                if (recordToRemove != null)
                {
                    carDetails.Remove(recordToRemove);
                }
            }
            return carDetails;
        }

        public async Task<List<CarDetails>> GetAllCarDetails(string searchName, Guid? brandId, Guid? vehicleTypeId)
        {
            var query = dbContext.CarDetails.Include(x => x.Brand).Include(x => x.VehicalType).OrderByDescending(x => x.ModifiedOn);

            if (searchName == string.Empty && brandId == Guid.Empty && vehicleTypeId == Guid.Empty)
            {
                return await query.ToListAsync();
            }

            if (brandId != Guid.Empty)
            {
                query = (IOrderedQueryable<CarDetails>)query.Where(x => x.BrandId == brandId);
            }

            if (vehicleTypeId != Guid.Empty)
            {
                query = (IOrderedQueryable<CarDetails>)query.Where(x => x.VehicalTypeId == vehicleTypeId);
            }

            if (!string.IsNullOrEmpty(searchName))
            {
                query = (IOrderedQueryable<CarDetails>)query.Where(x => x.Name.Contains(searchName));
            }

            return await query.ToListAsync();
        }
    }
}
