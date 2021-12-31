using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Specifications.Cocktails
{
    public class CocktailsByIds : BaseSpecification<Cocktail>
    {
        public CocktailsByIds(IEnumerable<int> ids) : base(c => ids.Contains(c.Id))
        {
        }
    }
}
