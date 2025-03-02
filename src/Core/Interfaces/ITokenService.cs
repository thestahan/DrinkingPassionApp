using Core.Entities.Identity;
using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user, IList<string> roles);
    }
}
