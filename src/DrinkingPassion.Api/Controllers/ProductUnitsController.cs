using AutoMapper;
using DrinkingPassion.Api.Core.Entities;
using DrinkingPassion.Api.Core.Interfaces;
using DrinkingPassion.Api.Core.Specifications.ProductUnits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DrinkingPassion.Api.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProductUnitsController : BaseApiController
    {
        private readonly IGenericRepository<ProductUnit> _repo;
        private readonly IMapper _mapper;

        public ProductUnitsController(IGenericRepository<ProductUnit> _repo, IMapper mapper)
        {
            this._repo = _repo;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Dtos.Products.ProductUnitToReturnDto>>> GetProductUnits()
        {
            var spec = new ProductUnitsOrderedByNameSpec();

            var units = await _repo.ListAsync(spec);

            var unitsToReturn = _mapper.Map<IReadOnlyList<Dtos.Products.ProductUnitToReturnDto>>(units);

            return Ok(unitsToReturn);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Dtos.Products.ProductUnitToReturnDto>> GetProductUnitById(int id)
        {
            var unit = await _repo.GetByIdAsync(id);

            if (unit == null)
            {
                return NotFound(new Errors.ApiErrorResponse(404));
            }

            var unitToReturn = _mapper.Map<Dtos.Products.ProductUnitToReturnDto>(unit);

            return Ok(unitToReturn);
        }

        [HttpPost]
        public async Task<ActionResult<Dtos.Products.ProductUnitToReturnDto>> AddProductUnit(Dtos.Products.ProductUnitToAddDto unitToAddDto)
        {
            var unit = _mapper.Map<ProductUnit>(unitToAddDto);

            var createdUnit = await _repo.AddAsync(unit);

            var unitToReturn = _mapper.Map<Dtos.Products.ProductUnitToReturnDto>(createdUnit);

            return CreatedAtAction(nameof(GetProductUnitById), new { id = unitToReturn.Id }, unitToReturn);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProductUnit(int id, Dtos.Products.ProductUnitToUpdateDto unitToUpdate)
        {
            if (id != unitToUpdate.Id)
            {
                return BadRequest(new Errors.ApiErrorResponse(400, "Id does not match with entity's id"));
            }

            if (!await _repo.EntityExistsAsync(id))
            {
                return BadRequest(new Errors.ApiErrorResponse(400, "Entity does not exist"));
            }

            var unit = _mapper.Map<ProductUnit>(unitToUpdate);

            await _repo.UpdateAsync(unit);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProductUnit(int id)
        {
            if (!await _repo.DeleteByIdAsync(id))
            {
                return NotFound(new Errors.ApiErrorResponse(404));
            }

            return NoContent();
        }
    }
}
