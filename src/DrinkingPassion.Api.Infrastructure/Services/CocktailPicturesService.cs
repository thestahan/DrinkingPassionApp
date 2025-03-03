using DrinkingPassion.Api.Core.Entities;
using DrinkingPassion.Api.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace DrinkingPassion.Api.Infrastructure.Services
{
    public class CocktailPicturesService
    {
        private readonly IBlobService _blobService;

        public CocktailPicturesService(IBlobService blobService)
        {
            _blobService = blobService;
        }

        public async Task DeletePreviousPictureIfExists(Cocktail cocktailFromDb, string container)
        {
            bool pictureExists = !string.IsNullOrEmpty(cocktailFromDb.Picture);

            if (pictureExists)
            {
                string blobName = cocktailFromDb.Picture.Split("/")[1];

                await _blobService.DeleteBlobAsync(container, blobName);
            }
        }

        public string GenerateNewBlobName(int cocktailId)
        {
            string sufix = Guid.NewGuid().ToString().Substring(0, 4);

            return $"cocktail-picture-{cocktailId}-{sufix}";
        }
    }
}
