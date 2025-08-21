using Microsoft.AspNetCore.Http;
using REBrokerApp.Infrastructure.Extensions;
using System.ComponentModel.DataAnnotations;

namespace REBrokerApp.Business.ViewModel
{
    public class PropertyCreateViewModel
    {
        // --- Property ---
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public int BedRooms { get; set; }
        public int BathRooms { get; set; }
        public int Toilet { get; set; }
        public int CarPark { get; set; }
        public decimal Price { get; set; }

        [MaxLength(200)]
        public string? BuilderName { get; set; }

        [MaxLength(100)]
        public string BrokerName { get; set; }

        [MaxLength(15)]
        public string BrokerPhone { get; set; }

        // --- Location ---
        [MaxLength(100)]
        public string? Address1 { get; set; }

        [MaxLength(100)]
        public string? Address2 { get; set; }

        [MaxLength(100)]
        public string? State { get; set; }

        [MaxLength(100)]
        public string? Suburb { get; set; }

        [MaxLength(20)]
        public string? PostalCode { get; set; }

        [MaxLength(20)]
        public string? Latitude { get; set; }

        [MaxLength(20)]
        public string? Longitude { get; set; }

        // --- Features ---
        [MaxLength(500)]
        public string? Internal { get; set; }

        [MaxLength(500)]
        public string External { get; set; } = string.Empty;

        [MaxLength(500)]
        public string General { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Security { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Location { get; set; } = string.Empty;

        [MaxLength(500)]
        public string LifeStyle { get; set; } = string.Empty;

        // --- Images ---
        [DataType(DataType.Upload)]
        [MaxFileSize(5 * 1024 * 1024)] // 5MB
        [AllowedExtensions(new[] { ".jpg", ".jpeg", ".png", ".gif" })]
        public List<IFormFile>? ImageFiles { get; set; }
    }
}
