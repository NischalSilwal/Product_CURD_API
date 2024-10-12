using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace Product_CURD_API.DTO
{
    public class ProductDTO
    {
        [Required]
        [MaxLength(40)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(40)]
        public string ProductDescription { get; set; }

        [Required]
        public IFormFile ? ImageFile { get; set; }   
    }

    public class ProductUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? ProductDescription { get; set; }

        [Required]
        public string? ProductImage { get; set; }

        [Required]
        public IFormFile? ImageFile { get; set; }

        
    }
        

}
