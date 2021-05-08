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
    }
}
