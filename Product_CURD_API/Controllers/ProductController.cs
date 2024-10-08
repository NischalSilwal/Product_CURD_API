using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_CURD_API.Data;
using Product_CURD_API.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Product_CURD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DemoDbContext _DemoDbContext;
        public ProductController(DemoDbContext demoDbContext)
        {
            _DemoDbContext = demoDbContext;
        }
        [HttpGet]
        //Get All Products
        public ActionResult<IEnumerable<Product>> Get()
        {
            return _DemoDbContext.Products;
        }
        [HttpGet("{id}")]
        //Get Product by ID
        public async Task<ActionResult<Product>> GetById(int id)
        {
            return await _DemoDbContext.Products.Where(x=> x.Id == id).SingleOrDefaultAsync();
        }

        [HttpPost]
        //Create a Product
        public async Task<ActionResult> Create(Product product)
        {
            await _DemoDbContext.Products.AddAsync(product);
            await _DemoDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut]
       // Update a Product
        public async Task<ActionResult> Update(Product product)
        {
            _DemoDbContext.Products.Update(product);
            await _DemoDbContext.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete]
        //Delete a Product
        public async Task<ActionResult> Delete(int id)
        {
            var ProductGetById = await GetById(id);
            if (ProductGetById == null)
            {
                return NotFound();
            }
            _DemoDbContext.Remove(ProductGetById.Value);
            await _DemoDbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
