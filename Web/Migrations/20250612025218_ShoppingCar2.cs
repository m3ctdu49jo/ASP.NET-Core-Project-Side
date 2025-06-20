using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingMall.Web.Migrations
{
    /// <inheritdoc />
    public partial class ShoppingCar2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCars_Products_ProductId",
                table: "ShoppingCars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCars",
                table: "ShoppingCars");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ShoppingCars",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "ShoppingCars",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ShoppingCars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCars",
                table: "ShoppingCars",
                columns: new[] { "Id", "UserName" });

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCars_Products_ProductId",
                table: "ShoppingCars",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCars_Products_ProductId",
                table: "ShoppingCars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCars",
                table: "ShoppingCars");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ShoppingCars");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ShoppingCars",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "ShoppingCars",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCars",
                table: "ShoppingCars",
                column: "UserName");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCars_Products_ProductId",
                table: "ShoppingCars",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductID");
        }
    }
}
