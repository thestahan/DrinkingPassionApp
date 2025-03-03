using System;
using System.Collections.Generic;
using System.Linq;

namespace DrinkingPassion.Api.Core.Specifications.Cocktails
{
    public class CocktailsPublicAndPrivateByIds : BaseSpecification<DrinkingPassion.Api.Core.Entities.Cocktail>
    {
        public CocktailsPublicAndPrivateByIds(IEnumerable<int> ids, string userId) : 
            base(c => ids.Contains(c.Id) && (!c.IsPrivate || c.AuthorId == userId))
        {
        }
    }
}
