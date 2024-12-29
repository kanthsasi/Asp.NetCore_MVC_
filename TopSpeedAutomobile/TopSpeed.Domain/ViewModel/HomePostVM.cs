using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using TopSpeed.Domain.Models;

namespace TopSpeed.Domain.ViewModel
{
    public class HomePostVM
    {
        public List<CarDetails> carDetails { get; set; }

        public string? searchBox { get; set; } = string.Empty;

        public Guid? BrandId { get; set; }

        public Guid? VehicleTypeId { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> BrandList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> VehicleTypeList { get; set; }
    }
}
