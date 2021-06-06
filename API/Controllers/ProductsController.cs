using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Interfaces;
using AutoMapper;
using API.Errors;
using Core.Specifications;
using API.Dtos.Products;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductUnit> _productUnitsRepo;
        private readonly IGenericRepository<ProductType> _productTypesRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productsRepo, IGenericRepository<ProductUnit> productUnitsRepo, IGenericRepository<ProductType> productTypesRepo, IMapper mapper)
        {
            _productsRepo = productsRepo;
            _productUnitsRepo = productUnitsRepo;
            _productTypesRepo = productTypesRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var spec = new ProductsWithTypesAndUnitsSpecification();

            var products = await _productsRepo.ListAsync(spec);

            var productsToReturn = _mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(productsToReturn);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndUnitsSpecification(id);

            var product = await _productsRepo.GetEntityWithSpec(spec);

            var productToReturn = _mapper.Map<ProductToReturnDto>(product);

            if (productToReturn == null) return NotFound(new ApiResponse(404));

            return Ok(productToReturn);
        }

        [HttpPost]
        public async Task<ActionResult<ProductToReturnDto>> AddProduct(ProductToAddDto product)
        {
            if (!await _productTypesRepo.EntityExists(product.ProductTypeId)) return BadRequest(new ApiResponse(400, "Product type of given id does not exist"));

            if (!await _productUnitsRepo.EntityExists(product.ProductUnitId)) return BadRequest(new ApiResponse(400, "Product unit of given id does not exist"));

            var productToAdd = _mapper.Map<Product>(product);

            var createdProduct = await _productsRepo.AddAsync(productToAdd);

            var spec = new ProductsWithTypesAndUnitsSpecification(createdProduct.Id);

            var createdProductWithTypeAndUnit = await _productsRepo.GetEntityWithSpec(spec);

            var productToReturn = _mapper.Map<ProductToReturnDto>(createdProductWithTypeAndUnit);

            return CreatedAtAction(nameof(GetProduct), new { id = productToReturn.Id }, productToReturn);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, ProductToUpdateDto productToUpdate)
        {
            if (!await _productTypesRepo.EntityExists(productToUpdate.ProductTypeId)) return BadRequest(new ApiResponse(400, "Product type of given id does not exist"));

            if (!await _productUnitsRepo.EntityExists(productToUpdate.ProductUnitId)) return BadRequest(new ApiResponse(400, "Product unit of given id does not exist"));

            if (id != productToUpdate.Id) return BadRequest(new ApiResponse(400, "Id does not match with product's id"));

            if (!await _productsRepo.EntityExists(id)) return BadRequest(new ApiResponse(400, "Product does not exist"));

            var product = _mapper.Map<Product>(productToUpdate);

            await _productsRepo.UpdateAsync(product);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _productsRepo.GetByIdAsync(id);

            if (product == null) return NotFound(new ApiResponse(404));

            await _productsRepo.DeleteAsync(product);

            return NoContent();
        }
    }
}