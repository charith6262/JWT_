using JWT.Model;
using JWT.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    //This controller is for Admin access only .

    [Authorize(Roles = "Admin , student")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("Add")]

        public async Task<ActionResult<Product>> Add(Product product)
        {
            var newproduct = await _productService.Add(product);
            return newproduct;

        }

        [HttpPut("Update")]

        public async Task<ActionResult<Product>> Update(Product product)
        {
            var existingProduct = await _productService.Update(product);
            return existingProduct;
        }

        [HttpDelete("Id")]

        public async Task<bool> Delete(int id)
        {
            var deleteProduct = await _productService.Delete(id);
           // return true;
            return deleteProduct;   
        }





    }
}
