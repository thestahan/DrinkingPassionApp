using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Interfaces;
using AutoMapper;
using API.Dtos;
using API.Errors;
using Core.Specifications;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _repo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var spec = new ProductsWithTypesAndUnitsSpecification();

            var products = await _repo.ListAsync(spec);

            var productsToReturn = _mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(productsToReturn);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndUnitsSpecification(id);

            var product = await _repo.GetEntityWithSpec(spec);

            var productToReturn = _mapper.Map<ProductToReturnDto>(product);

            if (productToReturn == null) return NotFound(new ApiResponse(404));

            return Ok(productToReturn);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(ProductToAddDto product)
        {
            var productToAdd = _mapper.Map<Product>(product);

            var createdProduct = await _repo.AddAsync(productToAdd);

            var spec = new ProductsWithTypesAndUnitsSpecification(createdProduct.Id);

            var createdProductWithTypeAndUnit = await _repo.GetEntityWithSpec(spec);

            var productToReturn = _mapper.Map<ProductToReturnDto>(createdProductWithTypeAndUnit);

            return CreatedAtAction(nameof(GetProduct), new { id = productToReturn.Id }, productToReturn);
        }
    }
}