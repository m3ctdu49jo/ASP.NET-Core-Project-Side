using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingMall.Web.Migrations
{
    /// <inheritdoc />
    public partial class userKeyModify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            // 1. 新增暫存欄位
            migrationBuilder.AddColumn<int>(
                name: "Id_temp",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // 2. 複製 Id 資料到暫存欄位
            migrationBuilder.Sql("UPDATE Users SET Id_temp = Id");

            // 3. 刪除原本的 Id 欄位
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Users");

            // 4. 重新命名暫存欄位為 Id
            migrationBuilder.RenameColumn(
                name: "Id_temp",
                table: "Users",
                newName: "Id");

            // 5. 建立複合主鍵
            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                columns: new[] { "Id", "UserName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            // 1. 新增 Id_temp 欄位，帶回 IDENTITY
            migrationBuilder.AddColumn<int>(
                name: "Id_temp",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            // 2. 複製 Id 資料回 Id_temp
            migrationBuilder.Sql("UPDATE Users SET Id_temp = Id");

            // 3. 刪除 Id 欄位
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Users");

            // 4. 重新命名 Id_temp 為 Id
            migrationBuilder.RenameColumn(
                name: "Id_temp",
                table: "Users",
                newName: "Id");

            // 5. 恢復單一主鍵
            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");
        }
    }
}
