using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Infrastructure.Data.Migrations
{
    public partial class AddCocktailsLists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CocktailsLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UniqueLink = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    AuthorId = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CocktailsLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CocktailsLists_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CocktailCocktailsList",
                columns: table => new
                {
                    CocktailsId = table.Column<int>(type: "integer", nullable: false),
                    CocktailsListsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CocktailCocktailsList", x => new { x.CocktailsId, x.CocktailsListsId });
                    table.ForeignKey(
                        name: "FK_CocktailCocktailsList_Cocktails_CocktailsId",
                        column: x => x.CocktailsId,
                        principalTable: "Cocktails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CocktailCocktailsList_CocktailsLists_CocktailsListsId",
                        column: x => x.CocktailsListsId,
                        principalTable: "CocktailsLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CocktailCocktailsList_CocktailsListsId",
                table: "CocktailCocktailsList",
                column: "CocktailsListsId");

            migrationBuilder.CreateIndex(
                name: "IX_CocktailsLists_AuthorId",
                table: "CocktailsLists",
                column: "AuthorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CocktailCocktailsList");

            migrationBuilder.DropTable(
                name: "CocktailsLists");
        }
    }
}
