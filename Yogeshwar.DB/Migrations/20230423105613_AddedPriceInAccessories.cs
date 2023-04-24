using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yogeshwar.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddedPriceInAccessories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Accessories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Accessories");
        }
    }
}
