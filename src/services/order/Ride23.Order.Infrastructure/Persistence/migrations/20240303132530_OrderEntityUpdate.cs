using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ride23.order.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class OrderEntityUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "Order",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                schema: "Order",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                schema: "Order",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "Order",
                table: "Orders",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                schema: "Order",
                table: "Orders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                schema: "Order",
                table: "Orders",
                type: "timestamp without time zone",
                nullable: true);
        }
    }
}
