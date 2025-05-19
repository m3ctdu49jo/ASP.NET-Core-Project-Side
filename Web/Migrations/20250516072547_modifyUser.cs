using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingMall.Migrations
{
    /// <inheritdoc />
    public partial class modifyUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ShippedDate",
                table: "Orders",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datatime",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Users");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ShippedDate",
                table: "Orders",
                type: "datatime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);
        }
    }
}
