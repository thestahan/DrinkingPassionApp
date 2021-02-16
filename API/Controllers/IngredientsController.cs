using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        private readonly IIngredientRepository _repo;

        public IngredientsController(IIngredientRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Ingredient>>> GetIngredients()
        {
            var ingredients = await _repo.GetIngredientsAsync();

            return Ok(ingredients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ingredient>> GetIngredient(int id)
        {
            return await _repo.GetIngredientByIdAsync(id);
        }
    }
}
