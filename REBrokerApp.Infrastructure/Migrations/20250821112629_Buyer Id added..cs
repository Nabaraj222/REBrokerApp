using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REBrokerApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BuyerIdadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BuyerId",
                table: "Properties",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_BuyerId",
                table: "Properties",
                column: "BuyerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_AspNetUsers_BuyerId",
                table: "Properties",
                column: "BuyerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_AspNetUsers_BuyerId",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_BuyerId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "Properties");
        }
    }
}
