using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq;
using System.Reflection;

namespace Core.Entities.Configurations
{
    public static class BaseEntityConfiguration
    {
        static void Configure<TEntity, T>(ModelBuilder modelBuilder)
            where TEntity : BaseEntity
        {
            modelBuilder.Entity<TEntity>(builder =>
            {
                builder.Property(x => x.CreatedDate)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP"); //TODO, use getutcdate() in sqlserver

                builder.Property(x => x.ModifiedDate)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP"); //TODO, use getutcdate() in sqlserver
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
                    method.MakeGenericMethod(entityType.ClrType, typeof(BaseEntity)).Invoke(null, new[] { modelBuilder });
                }
            }

            return modelBuilder;
        }
    }
}
