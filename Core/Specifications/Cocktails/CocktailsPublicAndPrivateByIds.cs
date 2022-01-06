using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Specifications.Cocktails
{
    public class CocktailsPublicAndPrivateByIds : BaseSpecification<Cocktail>
    {
        public CocktailsPublicAndPrivateByIds(IEnumerable<int> ids, string userId) : 
            base(c => ids.Contains(c.Id) && (!c.IsPrivate || c.AuthorId == userId))
        {
        }
    }
}
