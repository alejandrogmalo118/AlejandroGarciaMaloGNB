using System.ComponentModel.DataAnnotations;

namespace AlejandroGarciaMalo.Models.Entities
{
    /// <summary>
    /// Class Transaction
    /// </summary>
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Sku { get; set; }
        [Required]
        public float Amount { get; set; }
        [Required]
        public string Currency { get; set; }
    }
}
