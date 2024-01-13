using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionService.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Milage",
                table: "Items",
                newName: "Mileage");

            migrationBuilder.RenameColumn(
                name: "ReversePrice",
                table: "Auctions",
                newName: "ReservePrice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Mileage",
                table: "Items",
                newName: "Milage");

            migrationBuilder.RenameColumn(
                name: "ReservePrice",
                table: "Auctions",
                newName: "ReversePrice");
        }
    }
}
