using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ride23.Identity.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addedField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "Identity",
                table: "Users",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Identity",
                table: "Users");
        }
    }
}
