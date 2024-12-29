using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using TopSpeed.Domain.Models;

namespace TopSpeed.Domain.ViewModel
{
    public class CarDetailsVM
    {
        public CarDetails CarDetails { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> BrandList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> VehicleTypeList { get; set; }

        public IEnumerable<SelectListItem> EngineAndFuelTypeList { get; set; }

        public IEnumerable<SelectListItem> TransmissionList { get; set; }
    }
}
