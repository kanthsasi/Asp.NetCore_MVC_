using System.ComponentModel.DataAnnotations;

namespace TopSpeed.Domain.Common
{
    public class BaseModel
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime CreateOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }
}
