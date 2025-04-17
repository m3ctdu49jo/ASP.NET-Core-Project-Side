using System.ComponentModel.DataAnnotations;

namespace ShoppingMall.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        [StringLength(15)]
        public string CategoryName { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
} 