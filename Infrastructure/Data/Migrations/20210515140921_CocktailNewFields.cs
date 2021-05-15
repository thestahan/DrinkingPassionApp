using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class CocktailNewFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BaseProductId",
                table: "Cocktails",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IngredientsCount",
                table: "Cocktails",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Cocktails_BaseProductId",
                table: "Cocktails",
                column: "BaseProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cocktails_Products_BaseProductId",
                table: "Cocktails",
                column: "BaseProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cocktails_Products_BaseProductId",
                table: "Cocktails");

            migrationBuilder.DropIndex(
                name: "IX_Cocktails_BaseProductId",
                table: "Cocktails");

            migrationBuilder.DropColumn(
                name: "BaseProductId",
                table: "Cocktails");

            migrationBuilder.DropColumn(
                name: "IngredientsCount",
                table: "Cocktails");
        }
    }
}
