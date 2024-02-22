using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelenMebel.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFurnitureTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CategoryId",
                table: "Furniture",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Furniture_CategoryId",
                table: "Furniture",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Furniture_Category_CategoryId",
                table: "Furniture",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Furniture_Category_CategoryId",
                table: "Furniture");

            migrationBuilder.DropIndex(
                name: "IX_Furniture_CategoryId",
                table: "Furniture");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Furniture");
        }
    }
}
