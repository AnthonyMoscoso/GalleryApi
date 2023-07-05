using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Model.Entity.DBs.Dbo;

namespace Model.Contexts.Config.Dbo
{
    internal class UserConfig : IEntityTypeConfiguration<User>
    {
        private readonly string tableName = nameof(User).ToLower();
        private readonly string schema = SchemasHelpers.dbo;

        public void Configure(EntityTypeBuilder<User> builder)
        {
            #region Schema
            builder.ToTable(tableName, schema);
            #endregion
            #region key
            builder.HasKey(w => w.IdUser);
            #endregion
            #region Properties
            builder.Property(w => w.IdUser).IsRequired().HasDefaultValueSql("NEWID()");
            builder.Property(w => w.Name).IsRequired().HasMaxLength(120);
            builder.Property(w => w.UrlImage);
            #endregion

            #region audit
            builder.Property(w => w.Created).HasColumnType(nameof(DateTimeOffset)).HasDefaultValueSql("GetDate()");
            builder.Property(w => w.Updated).HasColumnType(nameof(DateTimeOffset)).HasDefaultValueSql("GetDate()").IsRequired(false);
            builder.Property(w => w.IdUserCreater).IsRequired();
            builder.Property(w => w.IdUserModifier).IsRequired(false);
            #endregion
            #region RelationShips

           
            #endregion
        }
    }
}