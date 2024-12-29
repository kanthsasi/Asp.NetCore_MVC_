using TopSpeed.Domain.Models;

namespace TopSpeed.Application.Contracts.Presistance
{
    public interface IBrandRepository : IGenericRepository<Brand>
    {
        Task<Brand> Update(Brand brand,Guid id);
    }
}
