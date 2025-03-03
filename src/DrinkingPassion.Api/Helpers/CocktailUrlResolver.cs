using AutoMapper;
using DrinkingPassion.Api.Core.Entities;
using Microsoft.Extensions.Configuration;
using System;

namespace DrinkingPassion.Api.Helpers
{
    public class CocktailUrlResolver : IValueResolver<Cocktail, Object, string>
    {
        private readonly IConfiguration _config;

        public CocktailUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(Cocktail source, Object destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Picture))
            {
                return _config["AzureBlobStorage:Url"] + source.Picture;
            }

            return null;
        }
    }
}
