namespace TopSpeed.Domain.Models
{
  
        public class ErrorViewModel
        {
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
            public string? RequestId { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

            public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        }
}

