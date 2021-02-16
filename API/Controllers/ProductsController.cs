using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRespository _repo;

        public ProductsController(IProductRespository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _repo.GetProductsAsync();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            return await _repo.GetProductByIdAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(Product product)
        {
            var createdProduct = await _repo.AddProductAsync(product);

            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, product);
        }
    }
}
