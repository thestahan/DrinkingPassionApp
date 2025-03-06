using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;

namespace DrinkingPassion.Api.Core.Entities.Configurations
{
    public static class BaseEntityConfiguration
    {
        private static void Configure<TEntity>(ModelBuilder modelBuilder)
            where TEntity : BaseEntity
        {
            modelBuilder.Entity<TEntity>(builder =>
            {
                builder.Property(x => x.CreatedDate)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                builder.Property(x => x.ModifiedDate)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }

        public static ModelBuilder ApplyBaseEntityConfiguration(this ModelBuilder modelBuilder)
        {
            var method = typeof(BaseEntityConfiguration).GetTypeInfo().DeclaredMethods
                .Single(m => m.Name == nameof(Configure));

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.IsSubclassOf(typeof(BaseEntity)))
                {
                    method.MakeGenericMethod(entityType.ClrType).Invoke(null, new object[] { modelBuilder });
                }
            }

            return modelBuilder;
        }
    }
}
