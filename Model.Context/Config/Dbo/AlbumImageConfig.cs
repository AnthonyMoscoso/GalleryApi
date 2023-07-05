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
    public class AlbumImageConfig : IEntityTypeConfiguration<AlbumImage>
    {
        private readonly string tableName = nameof(AlbumImage).ToLower();
        private readonly string schema = SchemasHelpers.dbo;

        public void Configure(EntityTypeBuilder<AlbumImage> builder)
        {
            #region Schema
            builder.ToTable(tableName, schema);
            #endregion
            #region key
            builder.HasKey(w => w.IdAlbumImage);
            #endregion
            #region Properties
            builder.Property(w => w.IdAlbumImage).IsRequired().HasDefaultValueSql("NEWID()");
            builder.Property(w => w.IdAlbum).IsRequired();
            builder.Property(w => w.IdImage).IsRequired();
            #endregion
            #region relationships
            builder.HasOne(w => w.ImageFile)
                          .WithMany(w => w.AlbumImage)
                          .HasForeignKey(w => w.IdImage)
                          .HasConstraintName($"fk_{tableName}_idImage");

            builder.HasOne(w => w.Album)
                          .WithMany(w => w.AlbumImage)
                          .HasForeignKey(w => w.IdAlbum)
                    .HasConstraintName($"fk_{tableName}_idAlbum");
            #endregion
          
        }
    }
}