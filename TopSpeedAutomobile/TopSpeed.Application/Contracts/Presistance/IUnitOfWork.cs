namespace TopSpeed.Application.Contracts.Presistance
{
    public interface IUnitOfWork : IDisposable
    {
        public IBrandRepository Brand { get; }

        public IVehicalTypeRepository VehicalType { get; }

        public ICarDetailsRepository CarDetails { get; set; }

        Task Save();
    }
}
