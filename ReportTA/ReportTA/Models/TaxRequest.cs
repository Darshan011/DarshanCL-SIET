using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ReportTA.Models
{
    public class TaxRequest
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public double Income { get; set; }

        [Required]
        public string Regime { get; set; } 

        public double TaxAmount { get; set; }
        public double Cess { get; set; }
        public double TotalTaxPayable { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
