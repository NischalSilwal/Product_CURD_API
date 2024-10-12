using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Product_CURD_API.DTO;
using Product_CURD_API.Model;
using Product_CURD_API.Repositories;
using Product_CURD_API.Services;

namespace Product_CURD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IFileService fileService, IProductRepository productRepository, ILogger<ProductController> logger)
        {
            _fileService = fileService;
            _productRepository = productRepository;
            _logger = logger;
        }

        // POST: api/Product
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] ProductDTO productToAdd)
        {
            try
            {
                if (productToAdd.ImageFile?.Length > 1 * 1024 * 1024)
                {
                    return BadRequest("File should not exceed 1 MB");
                }

                string[] allowedFileExtensions = { ".jpg", ".jpeg", ".png" };
                string createdImageName = await _fileService.SaveFileAsync(productToAdd.ImageFile, allowedFileExtensions);

                var product = new Product
                {
                    Name = productToAdd.Name,
                    ProductDescription = productToAdd.ProductDescription,
                    ProductImage = createdImageName
                };

                var createdProduct = await _productRepository.AddProductAsync(product);
                return CreatedAtAction(nameof(CreateProduct), createdProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status422UnprocessableEntity, ex.Message);
            }
        }

        // GET: api/Product
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productRepository.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // GET: api/Product/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _productRepository.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound($"Product with ID {id} not found.");
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // DELETE: api/Product/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _productRepository.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound($"Product with ID {id} not found.");
                }

                await _productRepository.DeleteProductAsync(product);
                if (!string.IsNullOrEmpty(product.ProductImage))
                {
                    _fileService.DeleteFileAsync(product.ProductImage);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // PUT: api/Product/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductUpdateDTO productUpdateDTO)
        {
            try
            {
                if (id != productUpdateDTO.Id)
                {
                    return BadRequest("ID in URL and body do not match.");
                }

                var existingProduct = await _productRepository.GetProductByIdAsync(id);
                if (existingProduct == null)
                {
                    return NotFound($"Product with ID {id} not found.");
                }

                string oldImage = existingProduct.ProductImage;

                if (productUpdateDTO.ImageFile != null)
                {
                    if (productUpdateDTO.ImageFile?.Length > 1 * 1024 * 1024)
                    {
                        return BadRequest("File should not exceed 1 MB.");
                    }

                    string[] allowedFileExtensions = { ".jpg", ".jpeg", ".png" };
                    string createdImageName = await _fileService.SaveFileAsync(productUpdateDTO.ImageFile, allowedFileExtensions);
                    productUpdateDTO.ProductImage = createdImageName;
                }
                else
                {
                    productUpdateDTO.ProductImage = existingProduct.ProductImage;
                }

                // Mapping productDto to product manually
                existingProduct.Name = productUpdateDTO.Name;
                existingProduct.ProductDescription = productUpdateDTO.ProductDescription;
                existingProduct.ProductImage = productUpdateDTO.ProductImage;

                var updatedProduct = await _productRepository.UpdateProductAsync(existingProduct);

                // If the image was updated, delete the old one
                if (productUpdateDTO.ImageFile != null)
                {
                    _fileService.DeleteFileAsync(oldImage);
                }

                return Ok(updatedProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
