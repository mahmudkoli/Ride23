using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ride23.customer.Infrastructure.Persistence.migrations
{
    /// <inheritdoc />
    public partial class CustomerIdentityUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentityGuid",
                schema: "Customer",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "IdentityId",
                schema: "Customer",
                table: "Customers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentityId",
                schema: "Customer",
                table: "Customers");

            migrationBuilder.AddColumn<Guid>(
                name: "IdentityGuid",
                schema: "Customer",
                table: "Customers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
