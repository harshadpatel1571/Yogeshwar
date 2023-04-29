using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yogeshwar.DB.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAccessories_Accessories_AccessoriesId",
                table: "ProductAccessories");

            migrationBuilder.RenameColumn(
                name: "AccessoriesId",
                table: "ProductAccessories",
                newName: "AccessoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductAccessories_AccessoriesId",
                table: "ProductAccessories",
                newName: "IX_ProductAccessories_AccessoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAccessories_Accessories_AccessoryId",
                table: "ProductAccessories",
                column: "AccessoryId",
                principalTable: "Accessories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAccessories_Accessories_AccessoryId",
                table: "ProductAccessories");

            migrationBuilder.RenameColumn(
                name: "AccessoryId",
                table: "ProductAccessories",
                newName: "AccessoriesId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductAccessories_AccessoryId",
                table: "ProductAccessories",
                newName: "IX_ProductAccessories_AccessoriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAccessories_Accessories_AccessoriesId",
                table: "ProductAccessories",
                column: "AccessoriesId",
                principalTable: "Accessories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
