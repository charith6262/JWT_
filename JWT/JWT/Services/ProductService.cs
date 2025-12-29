using JWT.Model;
using JWT.Services.IServices;

namespace JWT.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> Add(Product product)
        {
            
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> Delete(int id)
        {
            var deleleProduct = await _context.Products.FindAsync(id);
            if(deleleProduct == null)
            {
                return false;
            }
            _context.Products.Remove(deleleProduct);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Product> Update(Product product)
        {
            var existingProduct = await _context.Products.FindAsync(product.Id);
            if (existingProduct == null)
            {
                return null;
            }
            existingProduct.Name = product.Name;
            await _context.SaveChangesAsync();
            return existingProduct;
        }
    }
}
