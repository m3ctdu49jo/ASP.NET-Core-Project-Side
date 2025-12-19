using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingMall.Web.Migrations
{
    /// <inheritdoc />
    public partial class productCollectionModify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductCollections",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCollections", x => new { x.ProductID, x.UserName });
                    table.ForeignKey(
                        name: "FK_ProductCollections_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCollections_Users_UserName",
                        column: x => x.UserName,
                        principalTable: "Users",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_UserName",
                table: "ShoppingCarts",
                column: "UserName");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCollections_UserName",
                table: "ProductCollections",
                column: "UserName");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_Users_UserName",
                table: "ShoppingCarts",
                column: "UserName",
                principalTable: "Users",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_Users_UserName",
                table: "ShoppingCarts");

            migrationBuilder.DropTable(
                name: "ProductCollections");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCarts_UserName",
                table: "ShoppingCarts");
        }
    }
}
