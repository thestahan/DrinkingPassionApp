using API.Dtos.Cocktails;
using AutoMapper;
using Core.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class CocktailUrlResolver : IValueResolver<Cocktail, CocktailToReturnDto, string>
    {
        private readonly IConfiguration _config;

        public CocktailUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(Cocktail source, CocktailToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Picture))
            {
                return _config["ApiUrl"] + source.Picture;
            }

            return null;
        }
    }
}
