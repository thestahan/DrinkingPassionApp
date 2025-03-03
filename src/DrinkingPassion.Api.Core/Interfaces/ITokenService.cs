using System.Collections.Generic;

namespace DrinkingPassion.Api.Core.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(DrinkingPassion.Api.Core.Entities.Identity.AppUser user, IList<string> roles);
    }
}
