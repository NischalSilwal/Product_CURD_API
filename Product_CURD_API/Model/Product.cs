using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product_CURD_API.Model
{
    [Table("ProductCURD")]
    public class Product
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string ProductDescription { get; set; }

        [Required]
        public string? ProductImage { get; set; }

    }
}
