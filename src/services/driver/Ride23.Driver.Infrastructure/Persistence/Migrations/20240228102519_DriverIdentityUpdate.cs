using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ride23.driver.Infrastructure.Persistence.migrations
{
    /// <inheritdoc />
    public partial class DriverIdentityUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentityGuid",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.AddColumn<string>(
                name: "IdentityId",
                schema: "Driver",
                table: "Drivers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentityId",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.AddColumn<Guid>(
                name: "IdentityGuid",
                schema: "Driver",
                table: "Drivers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
