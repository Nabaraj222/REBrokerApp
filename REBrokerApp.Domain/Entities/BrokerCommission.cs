using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REBrokerApp.Domain.Entities
{
    public class BrokerCommission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Property")]
        public int PropertyId { get; set; }
        public virtual Property Property { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal CommissionAmount { get; set; }

        [Column(TypeName = "decimal(3,2)")]
        public decimal CommissionPercentage { get; set; }

        public DateTime TransactionDate { get; set; }
    }
}
