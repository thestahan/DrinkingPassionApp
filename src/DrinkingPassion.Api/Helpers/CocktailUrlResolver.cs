using AutoMapper;
using DrinkingPassion.Api.Core.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace DrinkingPassion.Api.Helpers
{
    public class CocktailUrlResolver : IValueResolver<Cocktail, Object, string>
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CocktailUrlResolver(
            IConfiguration config,
            IWebHostEnvironment environment,
            IHttpContextAccessor httpContextAccessor)
        {
            _config = config;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        public string Resolve(Cocktail source, Object destination, string destMember, ResolutionContext context)
        {
            string imagePath = string.IsNullOrEmpty(source.Picture)
                ? "/images/default/default_cocktail.jpg"
                : $"/{source.Picture}";

            string baseUrl = _environment.IsDevelopment()
               ? $"{_httpContextAccessor.HttpContext?.Request?.Scheme}://{_httpContextAccessor.HttpContext?.Request?.Host.Value}"
               : _config["AzureBlobStorage:Url"]
                   ?? throw new InvalidOperationException("AzureBlobStorage:Url configuration is missing");

            return baseUrl + imagePath;
        }
    }
}
