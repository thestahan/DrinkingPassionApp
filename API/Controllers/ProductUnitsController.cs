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
    public class ProductUnitsController : BaseApiController
    {
        private readonly IGenericRepository<ProductUnit> _repo;
        private readonly IMapper _mapper;

        public ProductUnitsController(IGenericRepository<ProductUnit> _repo, IMapper mapper)
        {
            this._repo = _repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductUnitToReturnDto>>> GetProductUnits()
        {
            var units = await _repo.ListAllAsync();

            var unitsToReturn = _mapper.Map<IReadOnlyList<ProductUnitToReturnDto>>(units);

            return Ok(unitsToReturn);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductUnitToReturnDto>> GetProductUnitById(int id)
        {
            var unit = await _repo.GetByIdAsync(id);

            if (unit == null) return NotFound(new ApiResponse(404));

            var unitToReturn = _mapper.Map<ProductUnitToReturnDto>(unit);

            return Ok(unitToReturn);
        }

        [HttpPost]
        public async Task<ActionResult<ProductUnitToReturnDto>> AddProductUnit(ProductUnitToAddDto unitToAddDto)
        {
            var unit = _mapper.Map<ProductUnit>(unitToAddDto);

            var createdUnit = await _repo.AddAsync(unit);

            var unitToReturn = _mapper.Map<ProductUnitToReturnDto>(createdUnit);

            return CreatedAtAction(nameof(GetProductUnitById), new { id = unitToReturn.Id }, unitToReturn);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProductUnit(int id, ProductUnitToUpdateDto unitToUpdate)
        {
            if (id != unitToUpdate.Id) return BadRequest(new ApiResponse(400, "Id does not match with entity's id"));

            if (!await _repo.EntityExistsAsync(id)) return BadRequest(new ApiResponse(400, "Entity does not exist"));

            var unit = _mapper.Map<ProductUnit>(unitToUpdate);

            await _repo.UpdateAsync(unit);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProductUnit(int id)
        {
            if (! await _repo.DeleteByIdAsync(id)) return NotFound(new ApiResponse(404));

            return NoContent();
        }
    }
}
