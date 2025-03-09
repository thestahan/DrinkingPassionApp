using AutoMapper;
using DrinkingPassion.Api.Core.Entities;
using DrinkingPassion.Api.Core.Entities.Identity;

namespace DrinkingPassion.Api.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ProductUnit, Dtos.Products.ProductUnitToReturnDto>();
            CreateMap<Dtos.Products.ProductUnitToAddDto, ProductUnit>();
            CreateMap<Dtos.Products.ProductUnitToUpdateDto, ProductUnit>();
            CreateMap<ProductType, Dtos.Products.ProductTypeToReturnDto>();
            CreateMap<Dtos.Products.ProductTypeToAddDto, ProductType>();
            CreateMap<Dtos.Products.ProductTypeToUpdateDto, ProductType>();
            CreateMap<Cocktail, Dtos.Cocktails.CocktailDetailsToReturnDto>()
                .ForMember(
                    dest => dest.Picture,
                    opt => opt.MapFrom<CocktailUrlResolver>())
                .ForMember(
                    dest => dest.BaseIngredient,
                    opt => opt.MapFrom(src => src.BaseProduct.Name));
            CreateMap<Cocktail, Dtos.Cocktails.CocktailToReturnDto>()
                .ForMember(
                    dest => dest.Picture,
                    opt => opt.MapFrom<CocktailUrlResolver>())
                .ForMember(
                    dest => dest.BaseIngredient,
                    opt => opt.MapFrom(src => src.BaseProduct.Name));
            CreateMap<Cocktail, Dtos.Cocktails.CocktailBasicInfoToReturnDto>();
            CreateMap<Ingredient, Dtos.Ingredients.IngredientToReturnDto>()
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
            CreateMap<Dtos.Ingredients.IngredientToAddDto, Ingredient>();
            CreateMap<Dtos.Ingredients.IngredientToUpdateDto, Ingredient>();
            CreateMap<Product, Dtos.Products.ProductToReturnDto>()
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
            CreateMap<Dtos.Products.ProductToAddDto, Product>();
            CreateMap<Dtos.Products.ProductToUpdateDto, Product>();
            CreateMap<AppUser, Dtos.Accounts.UserDetailsToReturnDto>();
            CreateMap<Dtos.Accounts.UserRegisterDto, AppUser>()
                .ForMember(
                    dest => dest.UserName,
                    opt => opt.MapFrom(src => src.Email));
            CreateMap<Dtos.Accounts.UserUpdateDto, AppUser>()
                .ForMember(
                    dest => dest.UserName,
                    opt => opt.MapFrom(src => src.Email));
            CreateMap<CocktailsList, Dtos.CocktailsLists.CocktailsListDetailsToReturnDto>();
            CreateMap<CocktailsList, Dtos.CocktailsLists.CocktailsListToReturnDto>();
            CreateMap<Dtos.CocktailsLists.CocktailsListToAddDto, CocktailsList>()
                .ForMember(
                    dest => dest.Cocktails,
                    opt => opt.Ignore());
        }
    }
}
