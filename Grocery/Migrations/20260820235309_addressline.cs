using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grocery.Migrations
{
    /// <inheritdoc />
    public partial class addressline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressLine",
                table: "Addresss",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeliveryInstructions",
                table: "Addresss",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressLine",
                table: "Addresss");

            migrationBuilder.DropColumn(
                name: "DeliveryInstructions",
                table: "Addresss");
        }
    }
}
