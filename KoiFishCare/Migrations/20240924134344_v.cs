using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KoiFishCare.Migrations
{
    /// <inheritdoc />
    public partial class v : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "012aff71-df12-4405-8678-2e7e302d69a6", null, "Staff", "STAFF" },
                    { "8cd90b97-35ee-41ca-88d6-858fa500448b", null, "Manager", "MANAGER" },
                    { "92ad9291-14d1-410f-a771-4013c76ef346", null, "Vet", "VET" },
                    { "e3c670f1-f429-4fb8-8ab5-1c44d8bb41a9", null, "Customer", "CUSTOMER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "012aff71-df12-4405-8678-2e7e302d69a6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8cd90b97-35ee-41ca-88d6-858fa500448b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "92ad9291-14d1-410f-a771-4013c76ef346");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e3c670f1-f429-4fb8-8ab5-1c44d8bb41a9");
        }
    }
}
