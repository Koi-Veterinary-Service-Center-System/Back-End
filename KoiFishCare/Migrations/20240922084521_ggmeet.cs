using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoiFishCare.Migrations
{
    /// <inheritdoc />
    public partial class ggmeet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MeetURL",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeetURL",
                table: "Bookings");
        }
    }
}
