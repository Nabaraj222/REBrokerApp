using Microsoft.EntityFrameworkCore.Migrations;
using REBrokerApp.Domain.Entities;

#nullable disable

namespace REBrokerApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Brokercommissionsetuptableadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BrokerCommisionSetups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    MaxPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(3,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrokerCommisionSetups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BrokerCommissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrokerCommissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrokerCommissions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrokerCommissions_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrokerCommissions_PropertyId",
                table: "BrokerCommissions",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_BrokerCommissions_UserId",
                table: "BrokerCommissions",
                column: "UserId");

            migrationBuilder.Sql(@"INSERT INTO BrokerCommisionSetups (MinPrice, MaxPrice, Percentage)
                VALUES 
                (0.00, 4999999.99, 2.00),
                (5000000.00, 10000000.00, 1.75),
                (10000000.01, 99999999.99, 1.50);");
                }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrokerCommisionSetups");

            migrationBuilder.DropTable(
                name: "BrokerCommissions");
        }
    }
}
