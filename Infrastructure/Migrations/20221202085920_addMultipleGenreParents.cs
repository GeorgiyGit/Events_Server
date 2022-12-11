using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addMultipleGenreParents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Genres_Genres_ParentId",
                table: "Genres");

            migrationBuilder.DropIndex(
                name: "IX_Genres_ParentId",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Genres");

            migrationBuilder.CreateTable(
                name: "GenreGenre",
                columns: table => new
                {
                    ParentsId = table.Column<int>(type: "int", nullable: false),
                    SubTypesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenreGenre", x => new { x.ParentsId, x.SubTypesId });
                    table.ForeignKey(
                        name: "FK_GenreGenre_Genres_ParentsId",
                        column: x => x.ParentsId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GenreGenre_Genres_SubTypesId",
                        column: x => x.SubTypesId,
                        principalTable: "Genres",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GenreGenre_SubTypesId",
                table: "GenreGenre",
                column: "SubTypesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenreGenre");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Genres",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Genres_ParentId",
                table: "Genres",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Genres_Genres_ParentId",
                table: "Genres",
                column: "ParentId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
