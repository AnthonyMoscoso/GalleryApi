using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Model.Entity.DBs.Auth;

namespace Model.Context.Config.Auth
{
    public class SessionConfig : IEntityTypeConfiguration<Session>
    {
        private readonly string tableName = nameof(Session).ToLower();
        private readonly string schema = SchemasHelpers.auth;
        public void Configure(EntityTypeBuilder<Session> builder)
        {

            builder.ToTable(tableName, schema);
            builder.HasKey(w => w.Id);
            builder.Property(w => w.Id).IsRequired().HasDefaultValueSql("NEWID()");
            builder.Property(w => w.Token).IsRequired();
            builder.Property(w => w.ExpirationDate).HasColumnType(nameof(DateTimeOffset)).IsRequired();
            builder.Property(w=> w.IdUser).IsRequired();

            builder.HasOne(w => w.User)
            .WithOne(w => w.Session)
             .HasForeignKey<Session>(w => w.IdUser)
            .HasConstraintName($"fk_{tableName}_IdUser")
           ;

        }
    }
}



