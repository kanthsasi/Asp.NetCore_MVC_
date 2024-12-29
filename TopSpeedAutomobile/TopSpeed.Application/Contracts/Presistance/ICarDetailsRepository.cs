using Microsoft.Extensions.Hosting;
using TopSpeed.Domain.Models;

namespace TopSpeed.Application.Contracts.Presistance
{
    public interface ICarDetailsRepository : IGenericRepository<CarDetails>
    {
        Task<CarDetails> Update(Guid id, CarDetails carDetails);

        Task<CarDetails> GetCarDetailsById(Guid id);

        Task<List<CarDetails>> GetAllCarDetails();

        Task<List<CarDetails>> GetAllCarDetails(Guid? skipRecord, Guid? brandId);

        Task<List<CarDetails>> GetAllCarDetails(string? searchName, Guid? brandId, Guid? vehicleTypeId);

    }
}
