using TopSpeed.Domain.Models;

namespace TopSpeed.Application.Contracts.Presistance
{
    public interface IVehicalTypeRepository : IGenericRepository<VehicalType>
    {
        Task<VehicalType> Update(Guid id, VehicalType vehicalType);
    }
}
