using System.ComponentModel.DataAnnotations;

namespace AlejandroGarciaMalo.Models.Entities
{
    /// <summary>
    /// Class Rate
    /// </summary>
    public class Rate
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string From { get; set; }
        [Required]
        public string To { get; set; }
        [Required]
        public float RateValue { get; set; }
    }
}
