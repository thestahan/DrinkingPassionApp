using API.Dtos;
using AutoMapper;
using Core.Entities;
using System.Collections.Generic;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Cocktail, CocktailToReturnDto>()
                .ForMember(
                    dest => dest.Picture,
                    opt => opt.MapFrom<CocktailUrlResolver>());
            CreateMap<Ingredient, IngredientToReturnDto>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.Product.Name));
            CreateMap<CocktailToAddDto, Cocktail>();
            CreateMap<IngredientToAddDto, Ingredient>();
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(
                    dest => dest.Unit,
                    opt => opt.MapFrom(src => src.Unit.ToString()));
            CreateMap<ProductToAddDto, Product>();
        }
    }
}
