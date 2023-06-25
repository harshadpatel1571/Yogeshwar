using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yogeshwar.DB.Migrations
{
    /// <inheritdoc />
    public partial class Add_Column_Amount_In_Order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Order",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Order");
        }
    }
}
