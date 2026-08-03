using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingMall.Web.Migrations
{
    /// <inheritdoc />
    public partial class OrderAddStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "payment",
                table: "Orders",
                newName: "Payment");

            migrationBuilder.AlterColumn<short>(
                name: "Payment",
                table: "Orders",
                type: "smallint",
                maxLength: 1,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldMaxLength: 1);

            migrationBuilder.AddColumn<short>(
                name: "Status",
                table: "Orders",
                type: "smallint",
                maxLength: 1,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Payment",
                table: "Orders",
                newName: "payment");

            migrationBuilder.AlterColumn<string>(
                name: "payment",
                table: "Orders",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldMaxLength: 1,
                oldNullable: true);
        }
    }
}
