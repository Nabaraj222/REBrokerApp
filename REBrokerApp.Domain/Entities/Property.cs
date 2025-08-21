using Microsoft.AspNetCore.Identity;
using REBrokerApp.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REBrokerApp.Domain.Entities
{
    public class Property:EntityBase
    {
        [MaxLength(200)]
        public required string Title { get; set; }

        [MaxLength(500)]
        public required string Description { get; set; }

        public PropertyStatus PropertyStatus { get; set; }  

        public int BedRooms { get; set; }
        public int BathRooms { get; set; }
        public int Toilet { get; set; }
        public int CarPark { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [MaxLength(200)]
        public string? BuilderName { get; set; }

        [MaxLength(100)]
        public string BrokerName { get; set; }

        [MaxLength(15)]
        public string BrokerPhone { get; set; }

        [ForeignKey("Buyer")]
        public string? BuyerId { get; set; }
        public virtual IdentityUser Buyer { get; set; }

        public virtual PropertyLocation? PropertyLocation { get; set; }
        public virtual PropertyFeature? PropertyFeature { get; set; }
        public virtual ICollection<PropertyImage>? PropertyImages { get; set; } = new List<PropertyImage>();
    }
}
