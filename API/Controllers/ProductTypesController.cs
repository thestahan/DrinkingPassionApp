using API.Dtos.Products;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class ProductTypesController : BaseApiController
    {
        private readonly IGenericRepository<ProductType> _repo;
        private readonly IMapper _mapper;

        public ProductTypesController(IGenericRepository<ProductType> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductTypeToReturnDto>>> GetProductTypes()
        {
            var types = await _repo.ListAllAsync();

            var typesToReturn = _mapper.Map<IReadOnlyList<ProductType>>(types);

            return Ok(typesToReturn);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductTypeToReturnDto>> GetProductTypeById(int id)
        {
            var type = await _repo.GetByIdAsync(id);

            if (type == null) return NotFound(new ApiResponse(404));

            var typeToReturn = _mapper.Map<ProductTypeToReturnDto>(type);

            return Ok(typeToReturn);
        }

        [HttpPost]
        public async Task<ActionResult<ProductTypeToAddDto>> AddProductType(ProductTypeToAddDto typeToAddDto)
        {
            var type = _mapper.Map<ProductType>(typeToAddDto);

            var createdType = await _repo.AddAsync(type);

            var typeToReturn = _mapper.Map<ProductTypeToReturnDto>(createdType);

            return CreatedAtAction(nameof(GetProductTypeById), new { id = typeToReturn.Id }, typeToReturn);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProductType(int id, ProductTypeToUpdateDto typeToUpdate)
        {
            if (id != typeToUpdate.Id) return BadRequest(new ApiResponse(400, "Id does not match with entity's id"));

            if (!await _repo.EntityExists(id)) return BadRequest(new ApiResponse(400, "Entity does not exist"));

            var type = _mapper.Map<ProductType>(typeToUpdate);

            await _repo.UpdateAsync(type);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProductType(int id)
        {
            var productType = await _repo.GetByIdAsync(id);

            if (productType == null) return NotFound(new ApiResponse(404));

            await _repo.DeleteAsync(productType);

            return NoContent();
        }
    }
}
