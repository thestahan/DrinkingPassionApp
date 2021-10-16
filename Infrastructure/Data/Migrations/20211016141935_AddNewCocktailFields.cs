using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class AddNewCocktailFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Cocktails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "Cocktails",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Cocktails_AuthorId",
                table: "Cocktails",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cocktails_AspNetUsers_AuthorId",
                table: "Cocktails",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cocktails_AspNetUsers_AuthorId",
                table: "Cocktails");

            migrationBuilder.DropIndex(
                name: "IX_Cocktails_AuthorId",
                table: "Cocktails");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Cocktails");

            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "Cocktails");
        }
    }
}
