using Microsoft.EntityFrameworkCore;
using Product_CURD_API.Data;
using Product_CURD_API.Model;
using System.Runtime;

namespace Product_CURD_API.Repositories
{
    public class ProductRepository : IProductRepository   
    {
        private readonly DemoDbContext _demoDbContext;
        public  ProductRepository(DemoDbContext context)
        {
            _demoDbContext = context;
        }
        public async Task<Product> AddProductAsync(Product product)
        {
            _demoDbContext.Products.Add(product);
            await _demoDbContext.SaveChangesAsync();
            return product;
        }

        public async Task DeleteProductAsync(Product product)
        {
            _demoDbContext?.Products.Remove(product);
            await _demoDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _demoDbContext.Products.ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            var product = await _demoDbContext.Products.SingleOrDefaultAsync(x => x.Id == id);
            return product;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            _demoDbContext.Products.Update(product);
            await _demoDbContext.SaveChangesAsync();
            return product;
        }
    }
}
