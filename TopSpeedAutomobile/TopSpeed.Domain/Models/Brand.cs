using System.ComponentModel.DataAnnotations;
using TopSpeed.Domain.Common;

namespace TopSpeed.Domain.Models
{
    public class Brand : BaseModel
    {
        [Required]
        public string Name { get; set; }

        [Display(Name = "Established Year")]
        public int EstablishedYear { get; set; }

        [Display(Name = "Brand_Logo")]
        public string BrandLogo { get; set; }
    }
}
