using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entity.DBs.Auth;
using Model.Entity.DBs.Dbo;

namespace Model.Contexts.Config.Auth
{
    internal class CredentialsConfig : IEntityTypeConfiguration<Credentials>
    {
        private readonly string tableName = nameof(Credentials).ToLower();
        private readonly string schema = SchemasHelpers.auth;
        public void Configure(EntityTypeBuilder<Credentials> builder)
        {

            builder.ToTable(tableName, schema);
            builder.HasKey(w => w.IdCredentials);
            builder.Property(w => w.IdCredentials).IsRequired().HasDefaultValueSql("NEWID()");
            builder.Property(w => w.Username).IsRequired(); 
            builder.Property(w => w.Password).IsRequired();
            builder.Property(w => w.IdUser).IsRequired();

            #region audit
            builder.Property(w => w.Created).HasColumnType(nameof(DateTimeOffset)).HasDefaultValueSql("GetDate()");
            builder.Property(w => w.Updated).HasColumnType(nameof(DateTimeOffset)).HasDefaultValueSql("GetDate()").IsRequired(false);
            builder.Property(w => w.IdUserCreater).IsRequired();
            builder.Property(w => w.IdUserModifier).IsRequired(false);
            #endregion
            #region relationships
            builder.HasOne(w => w.User)
                .WithOne(w => w.Credentials)
                .HasForeignKey<Credentials>(w => w.IdUser)
                .HasConstraintName($"fk_{tableName}_idUser")
               ;
            #endregion
        }
    }
}
