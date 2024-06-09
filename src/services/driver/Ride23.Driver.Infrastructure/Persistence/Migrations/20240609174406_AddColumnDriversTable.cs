using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ride23.driver.Infrastructure.Persistence.migrations
{
    /// <inheritdoc />
    public partial class AddColumnDriversTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Driver",
                table: "Drivers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "City",
                schema: "Driver",
                table: "Drivers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                schema: "Driver",
                table: "Drivers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LicenseExpiryDate",
                schema: "Driver",
                table: "Drivers",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LicenseNo",
                schema: "Driver",
                table: "Drivers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NoOfRides",
                schema: "Driver",
                table: "Drivers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                schema: "Driver",
                table: "Drivers",
                type: "character(11)",
                fixedLength: true,
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                schema: "Driver",
                table: "Drivers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePhoto",
                schema: "Driver",
                table: "Drivers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "Driver",
                table: "Drivers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                schema: "Driver",
                table: "Drivers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "Country",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "LicenseExpiryDate",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "LicenseNo",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "NoOfRides",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "ProfilePhoto",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "Street",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Driver",
                table: "Drivers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }
    }
}
