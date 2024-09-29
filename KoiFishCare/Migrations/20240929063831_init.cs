using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KoiFishCare.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<bool>(type: "bit", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExperienceYears = table.Column<int>(type: "int", nullable: true),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePublicId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Distances",
                columns: table => new
                {
                    DistanceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Kilometer = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Distances", x => x.DistanceID);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Qrcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentID);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    ServiceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EstimatedDuration = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.ServiceID);
                });

            migrationBuilder.CreateTable(
                name: "Slots",
                columns: table => new
                {
                    SlotID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    WeekDate = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slots", x => x.SlotID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Staffs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ManagerID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsManager = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staffs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Staffs_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Veterinarians",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Veterinarians", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Veterinarians_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KoiOrPools",
                columns: table => new
                {
                    KoiOrPoolID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPool = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KoiOrPools", x => x.KoiOrPoolID);
                    table.ForeignKey(
                        name: "FK_KoiOrPools_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VetSlots",
                columns: table => new
                {
                    VetID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SlotID = table.Column<int>(type: "int", nullable: false),
                    isBooked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VetSlots", x => new { x.VetID, x.SlotID });
                    table.ForeignKey(
                        name: "FK_VetSlots_Slots_SlotID",
                        column: x => x.SlotID,
                        principalTable: "Slots",
                        principalColumn: "SlotID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VetSlots_Veterinarians_VetID",
                        column: x => x.VetID,
                        principalTable: "Veterinarians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BookingStatus = table.Column<int>(type: "int", nullable: false),
                    MeetURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentID = table.Column<int>(type: "int", nullable: false),
                    ServiceID = table.Column<int>(type: "int", nullable: false),
                    SlotID = table.Column<int>(type: "int", nullable: false),
                    CustomerID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VetID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DistanceID = table.Column<int>(type: "int", nullable: false),
                    KoiOrPoolID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookingID);
                    table.ForeignKey(
                        name: "FK_Bookings_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Distances_DistanceID",
                        column: x => x.DistanceID,
                        principalTable: "Distances",
                        principalColumn: "DistanceID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_KoiOrPools_KoiOrPoolID",
                        column: x => x.KoiOrPoolID,
                        principalTable: "KoiOrPools",
                        principalColumn: "KoiOrPoolID");
                    table.ForeignKey(
                        name: "FK_Bookings_Payments_PaymentID",
                        column: x => x.PaymentID,
                        principalTable: "Payments",
                        principalColumn: "PaymentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Services_ServiceID",
                        column: x => x.ServiceID,
                        principalTable: "Services",
                        principalColumn: "ServiceID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Slots_SlotID",
                        column: x => x.SlotID,
                        principalTable: "Slots",
                        principalColumn: "SlotID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Veterinarians_VetID",
                        column: x => x.VetID,
                        principalTable: "Veterinarians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    FeedbackID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rate = table.Column<int>(type: "int", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookingID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.FeedbackID);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Bookings_BookingID",
                        column: x => x.BookingID,
                        principalTable: "Bookings",
                        principalColumn: "BookingID");
                });

            migrationBuilder.CreateTable(
                name: "PrescriptionRecords",
                columns: table => new
                {
                    PrescriptionRecordID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiseaseName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Symptoms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Medication = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookingID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescriptionRecords", x => x.PrescriptionRecordID);
                    table.ForeignKey(
                        name: "FK_PrescriptionRecords_Bookings_BookingID",
                        column: x => x.BookingID,
                        principalTable: "Bookings",
                        principalColumn: "BookingID");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "38e7f48f-5a54-46af-9c5e-204ba4412831", null, "Customer", "CUSTOMER" },
                    { "b955216d-a59c-4caa-b7fe-fcc522e6393f", null, "Manager", "MANAGER" },
                    { "c89ff6a7-0c08-4e9a-91c5-88e090548240", null, "Staff", "STAFF" },
                    { "d3e48e17-a793-4636-ad58-66e10c0aa52f", null, "Vet", "VET" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "ExperienceYears", "FirstName", "Gender", "ImagePublicId", "ImageURL", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "c1", 0, "789 Customer Lane", "b2483a7d-7765-4511-95ec-ad021753c9ac", null, false, null, "Alice", false, "customer1_image_id", "https://example.com/customer1.jpg", "Johnson", false, null, null, "ALICU", null, null, false, "a3d451ba-7d0d-4019-8a40-3dad88116277", false, "alicu" },
                    { "c2", 0, "123 Customer Ave", "0b26ea0e-739b-4d68-9326-b5bfde69f47d", null, false, null, "Bob", true, "customer2_image_id", "https://example.com/customer2.jpg", "Williams", false, null, null, "BOOOOOB", null, null, false, "c4495f01-8212-443b-9ef2-401daa7e9d1e", false, "boooob" },
                    { "m2", 0, "123 Staff Ave", "457084d8-6c7f-4516-99e9-4d74e96bf6c2", null, false, null, "manager", true, "staff2_image_id", "https://example.com/staff2.jpg", "Williams", false, null, null, "MANAGER", null, null, false, "e4ca127f-bfb9-4dbc-ae28-967a0b77afb7", false, "manager" },
                    { "s1", 0, "789 Staff Lane", "c7acf0be-dc2a-40e8-a892-3216f7da8ab9", null, false, null, "staff1", false, "staff_image_id", "https://example.com/staff1.jpg", "Johnson", false, null, null, "SITAP", null, null, false, "1aca6ef1-99a6-4e9b-aa0e-e5352c248dca", false, "sitap" },
                    { "v1", 0, "123 Vet St.", "6612ac37-d6ec-404a-9e50-e3746203d712", null, false, 10, "John", true, "vet1_image_id", "https://example.com/vet1.jpg", "Doe", false, null, null, "JOHNDOE", null, null, false, "2a97e080-8426-4f46-86bd-de4e457659a4", false, "johndoe" },
                    { "v2", 0, "456 Vet St.", "ce70d49c-0a89-4bb1-aab5-ead1a2b64d46", null, false, 8, "Jane", false, "vet2_image_id", "https://example.com/vet2.jpg", "Smith", false, null, null, "JANESMITH", null, null, false, "748b8b45-700c-4ca0-bc2d-2bd0a6da78fd", false, "janesmith" },
                    { "v3", 0, "456 Vet St.", "59703c08-07de-40e1-a82d-93680bcaf837", null, false, 8, "vet3", false, "vet2_image_id", "https://example.com/vet2.jpg", "Smith", false, null, null, "VET3", null, null, false, "28b40ea4-2c25-4dec-b4c2-87d3c89e2e2a", false, "vet3" },
                    { "v4", 0, "456 Vet St.", "d12f256f-97ad-4676-b914-301cae8b1dc5", null, false, 8, "Vet 4", false, "vet2_image_id", "https://example.com/vet2.jpg", "Smith", false, null, null, "VETERIANARY", null, null, false, "d522c4d2-5af0-4b98-b857-c35256424449", false, "veterianary" },
                    { "v5", 0, "456 Vet St.", "db1b3f8d-2722-4320-bd50-3f6762996833", null, false, 8, "Vet 5", false, "vet2_image_id", "https://example.com/vet2.jpg", "Smith", false, null, null, "VET1000000", null, null, false, "440e892a-1072-48c3-a5d1-cf2e3b8db51f", false, "vet1000000" }
                });

            migrationBuilder.InsertData(
                table: "Distances",
                columns: new[] { "DistanceID", "Kilometer", "Price" },
                values: new object[,]
                {
                    { 1, 5.0m, 20.00m },
                    { 2, 15.0m, 50.00m }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "PaymentID", "Qrcode", "Type" },
                values: new object[,]
                {
                    { 1, "qrcode1", "Credit Card" },
                    { 2, "qrcode2", "PayPal" }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "ServiceID", "Description", "EstimatedDuration", "Price", "ServiceName" },
                values: new object[,]
                {
                    { 1, "A general health check for Koi fish.", 1.5, 150.00m, "Koi Health Check" },
                    { 2, "Comprehensive pool maintenance service.", 2.0, 250.00m, "Pool Maintenance" }
                });

            migrationBuilder.InsertData(
                table: "Slots",
                columns: new[] { "SlotID", "EndTime", "StartTime", "WeekDate" },
                values: new object[,]
                {
                    { 1, new TimeOnly(10, 0, 0), new TimeOnly(9, 0, 0), "Monday" },
                    { 2, new TimeOnly(12, 0, 0), new TimeOnly(11, 0, 0), "Monday" },
                    { 3, new TimeOnly(12, 0, 0), new TimeOnly(9, 0, 0), "Tuesday" },
                    { 4, new TimeOnly(12, 0, 0), new TimeOnly(11, 0, 0), "Tuesday" },
                    { 5, new TimeOnly(12, 0, 0), new TimeOnly(11, 0, 0), "Wednesday" }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                column: "Id",
                values: new object[]
                {
                    "c1",
                    "c2"
                });

            migrationBuilder.InsertData(
                table: "Staffs",
                columns: new[] { "Id", "IsManager", "ManagerID" },
                values: new object[,]
                {
                    { "m2", true, null },
                    { "s1", false, null }
                });

            migrationBuilder.InsertData(
                table: "Veterinarians",
                column: "Id",
                values: new object[]
                {
                    "v1",
                    "v2",
                    "v3",
                    "v4",
                    "v5"
                });

            migrationBuilder.InsertData(
                table: "KoiOrPools",
                columns: new[] { "KoiOrPoolID", "CustomerID", "Description", "IsPool", "Name" },
                values: new object[,]
                {
                    { 1, "c1", "A beautiful koi pond owned by John.", false, "John's Koi Pond" },
                    { 2, "c2", "A large swimming pool owned by the Smith family.", true, "Smith's Pool" }
                });

            migrationBuilder.InsertData(
                table: "VetSlots",
                columns: new[] { "SlotID", "VetID", "isBooked" },
                values: new object[,]
                {
                    { 1, "v1", false },
                    { 2, "v1", false },
                    { 3, "v1", false },
                    { 1, "v2", false },
                    { 4, "v2", false },
                    { 4, "v3", false },
                    { 5, "v3", false },
                    { 1, "v4", false },
                    { 3, "v4", false },
                    { 5, "v5", false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CustomerID",
                table: "Bookings",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_DistanceID",
                table: "Bookings",
                column: "DistanceID");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_KoiOrPoolID",
                table: "Bookings",
                column: "KoiOrPoolID");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_PaymentID",
                table: "Bookings",
                column: "PaymentID");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ServiceID",
                table: "Bookings",
                column: "ServiceID");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_SlotID",
                table: "Bookings",
                column: "SlotID");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_VetID",
                table: "Bookings",
                column: "VetID");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_BookingID",
                table: "Feedbacks",
                column: "BookingID");

            migrationBuilder.CreateIndex(
                name: "IX_KoiOrPools_CustomerID",
                table: "KoiOrPools",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionRecords_BookingID",
                table: "PrescriptionRecords",
                column: "BookingID");

            migrationBuilder.CreateIndex(
                name: "IX_VetSlots_SlotID",
                table: "VetSlots",
                column: "SlotID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "PrescriptionRecords");

            migrationBuilder.DropTable(
                name: "Staffs");

            migrationBuilder.DropTable(
                name: "VetSlots");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Distances");

            migrationBuilder.DropTable(
                name: "KoiOrPools");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Slots");

            migrationBuilder.DropTable(
                name: "Veterinarians");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
