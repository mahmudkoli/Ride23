using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ride23.customer.Infrastructure.Persistence.migrations
{
    /// <inheritdoc />
    public partial class AddAddressPhoneNumberProfilePhotoToCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                schema: "Customer",
                table: "Customers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                schema: "Customer",
                table: "Customers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                schema: "Customer",
                table: "Customers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                schema: "Customer",
                table: "Customers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePhoto",
                schema: "Customer",
                table: "Customers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Street",
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
                name: "City",
                schema: "Customer",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Country",
                schema: "Customer",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                schema: "Customer",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                schema: "Customer",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ProfilePhoto",
                schema: "Customer",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Street",
                schema: "Customer",
                table: "Customers");
        }
    }
}
