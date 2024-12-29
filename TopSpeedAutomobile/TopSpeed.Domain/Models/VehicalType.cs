using System.ComponentModel.DataAnnotations;
using TopSpeed.Domain.Common;

namespace TopSpeed.Domain.Models
{
    public class VehicalType : BaseModel
    {
        [Required]
        public string Name { get; set; }
    }
}
