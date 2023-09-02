using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TunaPiano.Migrations
{
    public partial class UpdatedSongGenreModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SongGenres",
                table: "SongGenres");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "SongGenres",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SongGenres",
                table: "SongGenres",
                column: "Id");

            migrationBuilder.InsertData(
                table: "SongGenres",
                columns: new[] { "Id", "GenreId", "SongId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 1 }
                });

            migrationBuilder.UpdateData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 1,
                column: "Album",
                value: "Travels");

            migrationBuilder.CreateIndex(
                name: "IX_SongGenres_GenreId",
                table: "SongGenres",
                column: "GenreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SongGenres",
                table: "SongGenres");

            migrationBuilder.DropIndex(
                name: "IX_SongGenres_GenreId",
                table: "SongGenres");

            migrationBuilder.DeleteData(
                table: "SongGenres",
                keyColumn: "Id",
                keyColumnType: "integer",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SongGenres",
                keyColumn: "Id",
                keyColumnType: "integer",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SongGenres");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SongGenres",
                table: "SongGenres",
                columns: new[] { "GenreId", "SongId" });

            migrationBuilder.UpdateData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 1,
                column: "Album",
                value: "Bio goes here");
        }
    }
}
