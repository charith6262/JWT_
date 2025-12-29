using JWT.Model;

namespace JWT.Services.IServices
{
    public interface IProductService
    {
        Task<Product> Add(Product product);

        Task<Product> Update(Product product);

        Task<bool> Delete(int id);
    }
}
