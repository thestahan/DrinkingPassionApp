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
        public async Task<ActionResult<List<ProductUnitToReturnDto>>> GetProductUnits()
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
        public async Task<ActionResult<ProductUnitToAddDto>> AddProductUnit(ProductUnitToAddDto unitToAddDto)
        {
            var unit = _mapper.Map<ProductUnit>(unitToAddDto);

            var createdUnit = await _repo.AddAsync(unit);

            var unitToReturn = _mapper.Map<ProductUnitToReturnDto>(createdUnit);

            return CreatedAtAction(nameof(GetProductUnitById), new { id = unitToReturn.Id }, unitToReturn);
        }
    }
}
