using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Yogeshwar.DB.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedImageSizeLimit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "ProductImages",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Customer",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyLogo",
                table: "Configuration",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Categories",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Accessories",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "ProductImages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Customer",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyLogo",
                table: "Configuration",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Categories",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Accessories",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "HsnNo", "Image", "IsActive", "IsDeleted", "ModifiedBy", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2023, 4, 19, 9, 22, 48, 463, DateTimeKind.Local).AddTicks(1898), "HSN4547JD7", null, true, false, null, null, "Machine" },
                    { 2, 1, new DateTime(2023, 4, 19, 9, 22, 48, 463, DateTimeKind.Local).AddTicks(1900), "HSNSP54475", null, true, false, null, null, "Spare Part" },
                    { 3, 1, new DateTime(2023, 4, 19, 9, 22, 48, 463, DateTimeKind.Local).AddTicks(1901), "HSN5445ELC", null, true, false, null, null, "Electronic" },
                    { 4, 1, new DateTime(2023, 4, 19, 9, 22, 48, 463, DateTimeKind.Local).AddTicks(1903), "HSN7887IRN", null, true, false, null, null, "Iron" },
                    { 5, 1, new DateTime(2023, 4, 19, 9, 22, 48, 463, DateTimeKind.Local).AddTicks(1904), "HSN87707BN", null, true, false, null, null, "Bolt & Nuts" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "Email", "ModifiedDate", "Name", "Password", "PhoneNo", "UserType", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 4, 19, 9, 22, 48, 463, DateTimeKind.Local).AddTicks(1652), "sohampipaliyapatel@gmail.com", null, "Soham Patel", "xE1lAwv+UqE7GX5q6MWJZA==", "8128195769", (byte)1, "Soham Patel" },
                    { 2, new DateTime(2023, 4, 19, 9, 22, 48, 463, DateTimeKind.Local).AddTicks(1663), "harshadpatel1571@gmail.com", null, "Harshad Patel", "+e2tW/Ybi3njCdaCY5kG3g==", "8128382487", (byte)1, "yogeshwar" }
                });
        }
    }
}
