using Microsoft.EntityFrameworkCore.Migrations;

namespace MouratoAirport.Migrations
{
    public partial class flights : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Business",
                table: "Flights",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Deluxe",
                table: "Flights",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Economic",
                table: "Flights",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "Flights",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Business",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "Deluxe",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "Economic",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Flights");
        }
    }
}
