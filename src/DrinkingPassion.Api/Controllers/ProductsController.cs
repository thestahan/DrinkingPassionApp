using AutoMapper;
using DrinkingPassion.Api.Core.Entities;
using DrinkingPassion.Api.Core.Entities.Identity;
using DrinkingPassion.Api.Core.Interfaces;
using DrinkingPassion.Api.Core.Specifications.Ingredients;
using DrinkingPassion.Api.Core.Specifications.Products;
using DrinkingPassion.Api.Dtos.Ingredients;
using DrinkingPassion.Api.Dtos.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DrinkingPassion.Api.Controllers
{
    [Authorize]
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductUnit> _productUnitsRepo;
        private readonly IGenericRepository<ProductType> _productTypesRepo;
        private readonly IGenericRepository<Ingredient> _ingredientsRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public ProductsController(
            IGenericRepository<Product> productsRepo,
            IGenericRepository<ProductUnit> productUnitsRepo,
            IGenericRepository<ProductType> productTypesRepo,
            IMapper mapper,
            UserManager<AppUser> userManager,
            IGenericRepository<Ingredient> ingredientsRepo)
        {
            _productsRepo = productsRepo;
            _productUnitsRepo = productUnitsRepo;
            _productTypesRepo = productTypesRepo;
            _mapper = mapper;
            _userManager = userManager;
            _ingredientsRepo = ingredientsRepo;
        }

        [AllowAnonymous]
        [HttpGet("Public")]
        public async Task<ActionResult<IReadOnlyList<DrinkingPassion.Shared.Models.Products.ProductToReturnDto>>> GetPublicProducts()
        {
            var spec = new ProductsWithTypesAndUnitsSpecification(false);

            var products = await _productsRepo.ListAsync(spec);

            var productsToReturn = _mapper.Map<IReadOnlyList<DrinkingPassion.Shared.Models.Products.ProductToReturnDto>>(products);

            return Ok(productsToReturn);
        }

        [HttpGet("Private")]
        public async Task<ActionResult<IReadOnlyList<DrinkingPassion.Shared.Models.Products.ProductToReturnDto>>> GetPrivateProducts()
        {
            var user = await GetAuthorizedUser();

            var spec = new ProductsWithTypesAndUnitsSpecification(true, user.Id);

            var products = await _productsRepo.ListAsync(spec);

            var productsToReturn = _mapper.Map<IReadOnlyList<DrinkingPassion.Shared.Models.Products.ProductToReturnDto>>(products);

            return Ok(productsToReturn);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DrinkingPassion.Shared.Models.Products.ProductToReturnDto>> GetProduct(int id)
        {
            var user = await GetAuthorizedUser();

            var spec = new ProductsWithTypesAndUnitsSpecification(id);

            var product = await _productsRepo.GetEntityWithSpec(spec);

            if (product == null ||
                product.IsPrivate && product.AuthorId != user.Id)
            {
                return NotFound(new Errors.ApiErrorResponse(404));
            }

            var productToReturn = _mapper.Map<DrinkingPassion.Shared.Models.Products.ProductToReturnDto>(product);

            return Ok(productToReturn);
        }

        [HttpPost]
        public async Task<ActionResult<DrinkingPassion.Shared.Models.Products.ProductToReturnDto>> AddProduct(DrinkingPassion.Shared.Models.Products.ProductToAddDto product)
        {
            var user = await GetAuthorizedUser();

            bool isAdmin = await _userManager.IsInRoleAsync(user, "admin");

            bool productTypeExists = await _productTypesRepo.EntityExistsAsync(product.ProductTypeId);

            if (!productTypeExists)
            {
                return BadRequest(new Errors.ApiErrorResponse(400, "Typ produktu o podanym id nie istnieje"));
            }

            bool productUnitExists = await _productUnitsRepo.EntityExistsAsync(product.ProductUnitId);

            if (!productUnitExists)
            {
                return BadRequest(new Errors.ApiErrorResponse(400, "Jednostka o podanym id nie istnieje"));
            }

            var productToAdd = _mapper.Map<Product>(product);

            if (!isAdmin)
            {
                productToAdd.IsPrivate = true;
            }

            productToAdd.AuthorId = user.Id;

            var createdProduct = await _productsRepo.AddAsync(productToAdd);

            var spec = new ProductsWithTypesAndUnitsSpecification(createdProduct.Id);

            var createdProductWithTypeAndUnit = await _productsRepo.GetEntityWithSpec(spec);

            var productToReturn = _mapper.Map<DrinkingPassion.Shared.Models.Products.ProductToReturnDto>(createdProductWithTypeAndUnit);

            return CreatedAtAction(nameof(GetProduct), new { id = productToReturn.Id }, productToReturn);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, DrinkingPassion.Shared.Models.Products.ProductToUpdateDto productToUpdate)
        {
            var user = await GetAuthorizedUser();

            bool isAdmin = await _userManager.IsInRoleAsync(user, "admin");

            bool productTypeExists = await _productTypesRepo.EntityExistsAsync(productToUpdate.ProductTypeId);

            if (!productTypeExists)
            {
                return BadRequest(new Errors.ApiErrorResponse(400, "Typ produktu o podanym id nie istnieje"));
            }

            bool productUnitExists = await _productUnitsRepo.EntityExistsAsync(productToUpdate.ProductUnitId);

            if (!productUnitExists)
            {
                return BadRequest(new Errors.ApiErrorResponse(400, "Jednostka o podanym id nie istnieje"));
            }

            if (id != productToUpdate.Id)
            {
                return BadRequest(new Errors.ApiErrorResponse(400, "Id URI zgadza się z id produktu"));
            }

            var product = await _productsRepo.GetByIdAsync(id);

            if (product == null ||
                product.IsPrivate && product.AuthorId != user.Id ||
                !product.IsPrivate && !isAdmin)
            {
                return NotFound(new Errors.ApiErrorResponse(404));
            }

            product.Name = productToUpdate.Name;
            product.ProductTypeId = productToUpdate.ProductTypeId;
            product.ProductUnitId = productToUpdate.ProductUnitId;

            await _productsRepo.UpdateAsync(product);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var user = await GetAuthorizedUser();

            bool isAdmin = await _userManager.IsInRoleAsync(user, "admin");

            var product = await _productsRepo.GetByIdAsync(id);

            if (product == null ||
                !product.IsPrivate && !isAdmin ||
                product.IsPrivate && product.AuthorId != user.Id)
            {
                return NotFound(new Errors.ApiErrorResponse(404));
            }

            await _productsRepo.DeleteAsync(product);

            return NoContent();
        }

        [Authorize]
        [HttpGet("IsPartOfCocktail")]
        public async Task<bool> IngredientIsPartOfAnyCocktail([FromQuery] IngredientAsPartOfCocktailDto dto)
        {
            var user = await GetAuthorizedUser();

            var spec = new IngredientIsPartOfAnyCocktailByPrivacySpec(dto.Id, user.Id, dto.IsPrivate);

            bool anyCocktailExists = await _ingredientsRepo.EntityExistsWithSpecAsync(spec);

            return anyCocktailExists;
        }

        private async Task<AppUser> GetAuthorizedUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            return await _userManager.FindByEmailAsync(email);
        }
    }
}
