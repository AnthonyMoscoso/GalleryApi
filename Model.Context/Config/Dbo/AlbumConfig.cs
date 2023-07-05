using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Model.Entity.DBs.Dbo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Context.Config.Dbo
{
    public class AlbumConfig : IEntityTypeConfiguration<Album>
    {
        private readonly string tableName = nameof(Album).ToLower();
        private readonly string schema = SchemasHelpers.dbo;

        public void Configure(EntityTypeBuilder<Album> builder)
        {
            #region Schema
            builder.ToTable(tableName, schema);
            #endregion
            #region key
            builder.HasKey(w => w.IdAlbum);
            #endregion
            #region Properties
            builder.Property(w => w.IdAlbum).IsRequired().HasDefaultValueSql("NEWID()");
            builder.Property(w => w.Name).IsRequired().HasMaxLength(120);
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