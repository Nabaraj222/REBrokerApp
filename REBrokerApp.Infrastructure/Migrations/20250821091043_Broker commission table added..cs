using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REBrokerApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Brokercommissiontableadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PropertyLocation_PropertyId",
                table: "PropertyLocation");

            migrationBuilder.DropIndex(
                name: "IX_PropertyFeature_PropertyId",
                table: "PropertyFeature");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Properties",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "BrokerName",
                table: "Properties",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BrokerPhone",
                table: "Properties",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyLocation_PropertyId",
                table: "PropertyLocation",
                column: "PropertyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropertyFeature_PropertyId",
                table: "PropertyFeature",
                column: "PropertyId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PropertyLocation_PropertyId",
                table: "PropertyLocation");

            migrationBuilder.DropIndex(
                name: "IX_PropertyFeature_PropertyId",
                table: "PropertyFeature");

            migrationBuilder.DropColumn(
                name: "BrokerName",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "BrokerPhone",
                table: "Properties");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Properties",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyLocation_PropertyId",
                table: "PropertyLocation",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyFeature_PropertyId",
                table: "PropertyFeature",
                column: "PropertyId");
        }
    }
}
