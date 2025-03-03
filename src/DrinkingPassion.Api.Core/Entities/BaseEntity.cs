using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrinkingPassion.Api.Core.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }

        [NotMapped]
        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
