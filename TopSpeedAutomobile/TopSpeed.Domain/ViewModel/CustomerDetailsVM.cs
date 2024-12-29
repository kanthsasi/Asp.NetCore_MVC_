

using TopSpeed.Domain.Models;

namespace TopSpeed.Domain.ViewModel
{
    public class CustomerDetailsVM
    {
        public CarDetails carDetails { get; set; }

        public List<CarDetails> carDetail { get; set; }
    }
}
