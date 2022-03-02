using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class CocktailsListsColumnRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FriendlyLink",
                table: "CocktailsLists");

            migrationBuilder.RenameColumn(
                name: "UniqueLink",
                table: "CocktailsLists",
                newName: "Slug");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Slug",
                table: "CocktailsLists",
                newName: "UniqueLink");

            migrationBuilder.AddColumn<string>(
                name: "FriendlyLink",
                table: "CocktailsLists",
                type: "text",
                nullable: true);
        }
    }
}
