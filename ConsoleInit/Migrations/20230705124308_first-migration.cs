using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsoleInit.Migrations
{
    /// <inheritdoc />
    public partial class firstmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.EnsureSchema(
                name: "auth");

            migrationBuilder.CreateTable(
                name: "album",
                schema: "dbo",
                columns: table => new
                {
                    IdAlbum = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    IdUserCreater = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdUserModifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "DateTimeOffset", nullable: false, defaultValueSql: "GetDate()"),
                    Updated = table.Column<DateTimeOffset>(type: "DateTimeOffset", nullable: true, defaultValueSql: "GetDate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_album", x => x.IdAlbum);
                });

            migrationBuilder.CreateTable(
                name: "imagefile",
                schema: "dbo",
                columns: table => new
                {
                    IdImage = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdUserCreater = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdUserModifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "DateTimeOffset", nullable: false, defaultValueSql: "GetDate()"),
                    Updated = table.Column<DateTimeOffset>(type: "DateTimeOffset", nullable: true, defaultValueSql: "GetDate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_imagefile", x => x.IdImage);
                });

            migrationBuilder.CreateTable(
                name: "user",
                schema: "dbo",
                columns: table => new
                {
                    IdUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    UrlImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdUserCreater = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdUserModifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "DateTimeOffset", nullable: false, defaultValueSql: "GetDate()"),
                    Updated = table.Column<DateTimeOffset>(type: "DateTimeOffset", nullable: true, defaultValueSql: "GetDate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.IdUser);
                });

            migrationBuilder.CreateTable(
                name: "albumimage",
                schema: "dbo",
                columns: table => new
                {
                    IdAlbumImage = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    IdAlbum = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdImage = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_albumimage", x => x.IdAlbumImage);
                    table.ForeignKey(
                        name: "fk_albumimage_idAlbum",
                        column: x => x.IdAlbum,
                        principalSchema: "dbo",
                        principalTable: "album",
                        principalColumn: "IdAlbum",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_albumimage_idImage",
                        column: x => x.IdImage,
                        principalSchema: "dbo",
                        principalTable: "imagefile",
                        principalColumn: "IdImage",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "credentials",
                schema: "auth",
                columns: table => new
                {
                    IdCredentials = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    IdUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdUserCreater = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdUserModifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "DateTimeOffset", nullable: false, defaultValueSql: "GetDate()"),
                    Updated = table.Column<DateTimeOffset>(type: "DateTimeOffset", nullable: true, defaultValueSql: "GetDate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credentials", x => x.IdCredentials);
                    table.ForeignKey(
                        name: "fk_credentials_idUser",
                        column: x => x.IdUser,
                        principalSchema: "dbo",
                        principalTable: "user",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "session",
                schema: "auth",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpirationDate = table.Column<DateTimeOffset>(type: "DateTimeOffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_session", x => x.Id);
                    table.ForeignKey(
                        name: "fk_session_IdUser",
                        column: x => x.IdUser,
                        principalSchema: "dbo",
                        principalTable: "user",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_albumimage_IdAlbum",
                schema: "dbo",
                table: "albumimage",
                column: "IdAlbum");

            migrationBuilder.CreateIndex(
                name: "IX_albumimage_IdImage",
                schema: "dbo",
                table: "albumimage",
                column: "IdImage");

            migrationBuilder.CreateIndex(
                name: "IX_credentials_IdUser",
                schema: "auth",
                table: "credentials",
                column: "IdUser",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_session_IdUser",
                schema: "auth",
                table: "session",
                column: "IdUser",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "albumimage",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "credentials",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "session",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "album",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "imagefile",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "user",
                schema: "dbo");
        }
    }
}
