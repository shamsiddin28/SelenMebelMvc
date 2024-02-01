using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelenMebelMvcUI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTypeOfFurniture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CategoryId",
                table: "TypeOfFurniture",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "CartDetail",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_TypeOfFurniture_CategoryId",
                table: "TypeOfFurniture",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_TypeOfFurniture_Category_CategoryId",
                table: "TypeOfFurniture",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TypeOfFurniture_Category_CategoryId",
                table: "TypeOfFurniture");

            migrationBuilder.DropIndex(
                name: "IX_TypeOfFurniture_CategoryId",
                table: "TypeOfFurniture");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "TypeOfFurniture");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "CartDetail");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "Order",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
