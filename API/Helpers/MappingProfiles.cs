using API.Dtos.Accounts;
using API.Dtos.Cocktails;
using API.Dtos.Ingredients;
using API.Dtos.Products;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;

namespace API.Helpers
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
            CreateMap<Ingredient, IngredientToReturnDto>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.Product.Name));
            CreateMap<CocktailToManageDto, Cocktail>();
            CreateMap<IngredientToAddDto, Ingredient>();
            CreateMap<IngredientToUpdateDto, Ingredient>();
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(
                    dest => dest.ProductUnit,
                    opt => opt.MapFrom(src => src.ProductUnit.Name))
                .ForMember(
                    dest => dest.ProductUnitId,
                    opt => opt.MapFrom(src => src.ProductUnit.Id))
                .ForMember(
                    dest => dest.ProductType,
                    opt => opt.MapFrom(src => src.ProductType.Name))
                .ForMember(
                    dest => dest.ProductTypeId,
                    opt => opt.MapFrom(src => src.ProductType.Id));
            CreateMap<ProductToAddDto, Product>();
            CreateMap<ProductToUpdateDto, Product>();
            CreateMap<AppUser, UserDetailsDto>();
            CreateMap<UserRegisterDto, AppUser>()
                .ForMember(
                    dest => dest.UserName,
                    opt => opt.MapFrom(src => src.Email));
            CreateMap<UserUpdateDto, AppUser>()
                .ForMember(
                    dest => dest.UserName,
                    opt => opt.MapFrom(src => src.Email));
        }
    }
}
