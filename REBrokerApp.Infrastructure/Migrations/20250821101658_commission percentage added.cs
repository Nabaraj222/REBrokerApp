using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REBrokerApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class commissionpercentageadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CommissionAmount",
                table: "BrokerCommissions",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CommissionPercentage",
                table: "BrokerCommissions",
                type: "decimal(3,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "TransactionDate",
                table: "BrokerCommissions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommissionAmount",
                table: "BrokerCommissions");

            migrationBuilder.DropColumn(
                name: "CommissionPercentage",
                table: "BrokerCommissions");

            migrationBuilder.DropColumn(
                name: "TransactionDate",
                table: "BrokerCommissions");
        }
    }
}
