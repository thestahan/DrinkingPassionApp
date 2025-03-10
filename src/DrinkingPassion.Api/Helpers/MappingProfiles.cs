using AutoMapper;
using DrinkingPassion.Api.Core.Entities;
using DrinkingPassion.Api.Core.Entities.Identity;
using DrinkingPassion.Api.Dtos.Accounts;
using DrinkingPassion.Api.Dtos.Cocktails;
using DrinkingPassion.Api.Dtos.CocktailsLists;
using DrinkingPassion.Api.Dtos.Ingredients;
using DrinkingPassion.Api.Dtos.Products;
using DrinkingPassion.Shared.Models.Cocktails;
using DrinkingPassion.Shared.Models.Ingredients;

namespace DrinkingPassion.Api.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ProductUnit, ProductUnitToReturnDto>();
            CreateMap<ProductUnitToAddDto, ProductUnit>();
            CreateMap<ProductUnitToUpdateDto, ProductUnit>();
            CreateMap<ProductType, ProductTypeToReturnDto>();
            CreateMap<ProductTypeToAddDto, ProductType>();
            CreateMap<ProductTypeToUpdateDto, ProductType>();
            CreateMap<Cocktail, CocktailDetailsToReturnDto>()
                .ForMember(
                    dest => dest.Picture,
                    opt => opt.MapFrom<CocktailUrlResolver>())
                .ForMember(
                    dest => dest.BaseIngredient,
                    opt => opt.MapFrom(src => src.BaseProduct.Name));
            CreateMap<Cocktail, CocktailToReturnDto>()
                .ForMember(
                    dest => dest.Picture,
                    opt => opt.MapFrom<CocktailUrlResolver>())
                .ForMember(
                    dest => dest.BaseIngredient,
                    opt => opt.MapFrom(src => src.BaseProduct.Name));
            CreateMap<Cocktail, CocktailBasicInfoToReturnDto>();
            CreateMap<Ingredient, IngredientToReturnDto>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(
                    dest => dest.Unit,
                    opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Product.ProductUnit.Abbreviation) ?
                        src.Product.ProductUnit.Name :
                        src.Product.ProductUnit.Abbreviation));
            CreateMap<Dtos.Cocktails.CocktailToManageDto, Cocktail>()
                .ForMember(dest => dest.Ingredients, opt => opt.Ignore())
                .ForMember(dest => dest.Picture, opt => opt.Ignore());
            CreateMap<IngredientToAddDto, Ingredient>();
            CreateMap<IngredientToUpdateDto, Ingredient>();
            CreateMap<Product, DrinkingPassion.Shared.Models.Products.ProductToReturnDto>()
                .ForMember(
                    dest => dest.ProductUnit,
                    opt => opt.MapFrom(src => src.ProductUnit.Name))
                .ForMember(
                    dest => dest.ProductUnitAbbreviation,
                    opt => opt.MapFrom(src => src.ProductUnit.Abbreviation))
                .ForMember(
                    dest => dest.ProductUnitId,
                    opt => opt.MapFrom(src => src.ProductUnit.Id))
                .ForMember(
                    dest => dest.ProductType,
                    opt => opt.MapFrom(src => src.ProductType.Name))
                .ForMember(
                    dest => dest.ProductTypeId,
                    opt => opt.MapFrom(src => src.ProductType.Id));
            CreateMap<DrinkingPassion.Shared.Models.Products.ProductToAddDto, Product>();
            CreateMap<DrinkingPassion.Shared.Models.Products.ProductToUpdateDto, Product>();
            CreateMap<AppUser, UserDetailsToReturnDto>();
            CreateMap<UserRegisterDto, AppUser>()
                .ForMember(
                    dest => dest.UserName,
                    opt => opt.MapFrom(src => src.Email));
            CreateMap<UserUpdateDto, AppUser>()
                .ForMember(
                    dest => dest.UserName,
                    opt => opt.MapFrom(src => src.Email));
            CreateMap<CocktailsList, CocktailsListDetailsToReturnDto>();
            CreateMap<CocktailsList, CocktailsListToReturnDto>();
            CreateMap<CocktailsListToAddDto, CocktailsList>()
                .ForMember(
                    dest => dest.Cocktails,
                    opt => opt.Ignore());
        }
    }
}
