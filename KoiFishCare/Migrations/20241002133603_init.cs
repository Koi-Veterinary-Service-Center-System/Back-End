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
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
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
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    ManagerID = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    IsPool = table.Column<bool>(type: "bit", nullable: true),
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
                    isBooked = table.Column<bool>(type: "bit", nullable: true)
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
                    KoiOrPoolID = table.Column<int>(type: "int", nullable: true),
                    DistanceID = table.Column<int>(type: "int", nullable: true)
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
                        principalColumn: "DistanceID");
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
                    RefundMoney = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RefundPercent = table.Column<int>(type: "int", nullable: true),
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
                    { "40de094e-0978-49eb-a909-039f130fa9e2", null, "Customer", "CUSTOMER" },
                    { "cfef6496-ddde-47cf-bf40-fc58cfd358f3", null, "Manager", "MANAGER" },
                    { "ebb25477-d386-489a-bd44-5694193697b0", null, "Staff", "STAFF" },
                    { "fcc69476-6e31-4c36-848d-294757f20f2c", null, "Vet", "VET" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "ExperienceYears", "FirstName", "Gender", "ImagePublicId", "ImageURL", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "c1", 0, "789 Customer Lane", "440be96a-d2e5-4560-b5b8-d51073cf418f", null, false, null, "Alice", false, "customer1_image_id", "https://example.com/customer1.jpg", "Johnson", false, null, null, "ALICE", null, null, false, "de67e3cc-7b54-4877-91ad-b67ee222e5d5", false, "alice" },
                    { "c2", 0, "123 Customer Ave", "a51e50dd-ece3-466b-b54d-279e3b57c69e", null, false, null, "Bob", true, "customer2_image_id", "https://example.com/customer2.jpg", "Williams", false, null, null, "BOOOOOB", null, null, false, "2fe0a0f9-10d9-4c62-8512-d1874e61b109", false, "boooob" },
                    { "v1", 0, "123 Vet St.", "42f92c9d-1a58-4228-9e74-01d9a97fc2c4", null, false, 10, "John", true, "vet1_image_id", "https://example.com/vet1.jpg", "Doe", false, null, null, "JOHNDOE", null, null, false, "e22e0584-d8e4-4cc1-93ec-8ddb79618168", false, "johndoe" },
                    { "v2", 0, "456 Vet St.", "efc6c3a4-f2dc-41b0-927f-dd0a5636f8e8", null, false, 8, "Jane", false, "vet2_image_id", "https://example.com/vet2.jpg", "Smith", false, null, null, "JANESMITH", null, null, false, "6f546379-c61f-4306-8639-8d979a8d7af2", false, "janesmith" },
                    { "v3", 0, "456 Vet St.", "afbf8c81-b7f9-43f7-a575-495572f42e87", null, false, 8, "vet3", false, "vet2_image_id", "https://example.com/vet2.jpg", "Smith", false, null, null, "VET3", null, null, false, "14a3d161-b43d-446f-9382-393f98cb73df", false, "vet3" },
                    { "v4", 0, "456 Vet St.", "e67d1c61-e31f-455c-bc5e-dc0074fccf1a", null, false, 8, "Vet 4", false, "vet2_image_id", "https://example.com/vet2.jpg", "Smith", false, null, null, "VETERIANARY", null, null, false, "8798b443-b0a0-4153-938a-a41da7cca082", false, "veterianary" },
                    { "v5", 0, "456 Vet St.", "deb71ae3-7af5-47d2-87d0-82be6baf6b33", null, false, 8, "Vet 5", false, "vet2_image_id", "https://example.com/vet2.jpg", "Smith", false, null, null, "VET1000000", null, null, false, "e426147d-e148-486e-999e-241490bbe865", false, "vet1000000" }
                });

            migrationBuilder.InsertData(
                table: "Distances",
                columns: new[] { "DistanceID", "Area", "District", "Price" },
                values: new object[,]
                {
                    { 1, "Ben Nghe Ward", "District 1", 92.500m },
                    { 2, "Ben Thanh Ward", "District 1", 98.500m },
                    { 3, "Co Giang Ward", "District 1", 97.000m },
                    { 4, "Cau Kho Ward", "District 1", 99.000m },
                    { 5, "Cau Ong Lanh Ward", "District 1", 95.500m },
                    { 6, "Da Kao Ward", "District 1", 93.000m },
                    { 7, "Nguyen Thai Binh Ward", "District 1", 94.000m },
                    { 8, "Nguyen Cu Trinh Ward", "District 1", 101.500m },
                    { 9, "Pham Ngu Lao Ward", "District 1", 101.000m },
                    { 10, "Tan Dinh Ward", "District 1", 105.000m },
                    { 11, "An Khanh Ward", "District 2", 70.500m },
                    { 12, "An Loi Dong Ward", "District 2", 84.000m },
                    { 13, "An Phu Ward", "District 2", 69.500m },
                    { 14, "Binh An Ward", "District 2", 74.000m },
                    { 15, "Binh Khanh Ward", "District 2", 70.000m },
                    { 16, "Binh Trung Dong Ward", "District 2", 54.000m },
                    { 17, "Binh Trung Tay Ward", "District 2", 64.000m },
                    { 18, "Cat Lai Ward", "District 2", 62.000m },
                    { 19, "Thanh My Loi Ward", "District 2", 65.000m },
                    { 20, "Thao Dien Ward", "District 2", 80.500m },
                    { 21, "Thu Thiem Ward", "District 2", 82.000m },
                    { 22, "Ward 1", "District 2", 112.500m },
                    { 23, "Ward 2", "District 2", 110.000m },
                    { 24, "Ward 3", "District 2", 108.000m },
                    { 25, "Ward 4", "District 2", 105.500m },
                    { 26, "Ward 5", "District 2", 104.000m },
                    { 27, "Ward 6", "District 2", 107.500m },
                    { 28, "Ward 7", "District 2", 108.000m },
                    { 29, "Ward 8", "District 2", 110.500m },
                    { 30, "Ward 9", "District 2", 113.000m },
                    { 31, "Ward 10", "District 2", 115.500m },
                    { 32, "Ward 11", "District 2", 124.500m },
                    { 33, "Ward 12", "District 2", 116.000m },
                    { 34, "Ward 13", "District 2", 114.500m },
                    { 35, "Ward 14", "District 2", 113.000m },
                    { 36, "Ward 1", "District 4", 114.000m },
                    { 37, "Ward 2", "District 4", 113.000m },
                    { 38, "Ward 3", "District 4", 115.500m },
                    { 39, "Ward 4", "District 4", 111.500m },
                    { 40, "Ward 5", "District 4", 117.500m },
                    { 41, "Ward 6", "District 4", 113.500m },
                    { 42, "Ward 8", "District 4", 115.000m },
                    { 43, "Ward 9", "District 4", 119.000m },
                    { 44, "Ward 10", "District 4", 117.000m },
                    { 45, "Ward 12", "District 4", 114.500m },
                    { 46, "Ward 13", "District 4", 120.500m },
                    { 47, "Ward 14", "District 4", 118.500m },
                    { 48, "Ward 15", "District 4", 112.000m },
                    { 49, "Ward 16", "District 4", 108.500m },
                    { 50, "Ward 18", "District 4", 102.500m },
                    { 51, "Ward 1", "District 5", 107.500m },
                    { 52, "Ward 2", "District 5", 105.500m },
                    { 53, "Ward 3", "District 5", 106.500m },
                    { 54, "Ward 4", "District 5", 104.000m },
                    { 55, "Ward 5", "District 5", 103.000m },
                    { 56, "Ward 6", "District 5", 100.000m },
                    { 57, "Ward 7", "District 5", 99.500m },
                    { 58, "Ward 8", "District 5", 98.000m },
                    { 59, "Ward 9", "District 5", 96.000m },
                    { 60, "Ward 10", "District 5", 94.000m },
                    { 61, "Ward 11", "District 5", 93.000m },
                    { 62, "Ward 12", "District 5", 91.500m },
                    { 63, "Ward 13", "District 5", 90.500m },
                    { 64, "Ward 14", "District 5", 89.000m },
                    { 65, "Ward 15", "District 5", 88.500m },
                    { 66, "Ward 1", "District 6", 1250000m },
                    { 67, "Ward 2", "District 6", 1300000m },
                    { 68, "Ward 3", "District 6", 1350000m },
                    { 69, "Ward 4", "District 6", 1400000m },
                    { 70, "Ward 5", "District 6", 1450000m },
                    { 71, "Ward 6", "District 6", 1500000m },
                    { 72, "Ward 7", "District 6", 1550000m },
                    { 73, "Ward 8", "District 6", 1600000m },
                    { 74, "Ward 9", "District 6", 1650000m },
                    { 75, "Ward 10", "District 6", 1700000m },
                    { 76, "Ward 11", "District 6", 1750000m },
                    { 77, "Ward 12", "District 6", 1800000m },
                    { 78, "Ward 13", "District 6", 1850000m },
                    { 79, "Ward 14", "District 6", 1900000m },
                    { 80, "Binh Thuan Ward", "District 7", 2000000m },
                    { 81, "Phu My Ward", "District 7", 2050000m },
                    { 82, "Phu Thuan Ward", "District 7", 2100000m },
                    { 83, "Tan Hung Ward", "District 7", 2150000m },
                    { 84, "Tan Kieng Ward", "District 7", 2200000m },
                    { 85, "Tan Phong Ward", "District 7", 2250000m },
                    { 86, "Tan Phu Ward", "District 7", 2300000m },
                    { 87, "Tan Quy Ward", "District 7", 2350000m },
                    { 88, "Tan Thuan Dong Ward", "District 7", 2400000m },
                    { 89, "Tan Thuan Tay Ward", "District 7", 2450000m },
                    { 90, "Ward 1", "District 8", 2000000m },
                    { 91, "Ward 2", "District 8", 2050000m },
                    { 92, "Ward 3", "District 8", 2100000m },
                    { 93, "Ward 4", "District 8", 2150000m },
                    { 94, "Ward 5", "District 8", 2200000m },
                    { 95, "Ward 6", "District 8", 2250000m },
                    { 96, "Ward 7", "District 8", 2300000m },
                    { 97, "Ward 8", "District 8", 2350000m },
                    { 98, "Ward 9", "District 8", 2400000m },
                    { 99, "Ward 10", "District 8", 2450000m },
                    { 100, "Ward 11", "District 8", 2500000m },
                    { 101, "Ward 12", "District 8", 2550000m },
                    { 102, "Ward 13", "District 8", 2600000m },
                    { 103, "Ward 14", "District 8", 2650000m },
                    { 104, "Ward 15", "District 8", 2700000m },
                    { 105, "Ward 16", "District 8", 2750000m },
                    { 106, "Hiep Phu Ward", "District 9", 2000000m },
                    { 107, "Long Binh Ward", "District 9", 2050000m },
                    { 108, "Long Phuoc Ward", "District 9", 2100000m },
                    { 109, "Long Thanh My Ward", "District 9", 2150000m },
                    { 110, "Long Truong Ward", "District 9", 2200000m },
                    { 111, "Phu Huu Ward", "District 9", 2250000m },
                    { 112, "Phuoc Binh Ward", "District 9", 2300000m },
                    { 113, "Phuoc Long A Ward", "District 9", 2350000m },
                    { 114, "Phuoc Long B Ward", "District 9", 2400000m },
                    { 115, "Tan Phu Ward", "District 9", 2450000m },
                    { 116, "Tang Nhon Phu A Ward", "District 9", 2500000m },
                    { 117, "Tang Nhon Phu B Ward", "District 9", 2550000m },
                    { 118, "Truong Thanh Ward", "District 9", 2600000m },
                    { 119, "Ward 1", "District 10", 2000000m },
                    { 120, "Ward 2", "District 10", 2050000m },
                    { 121, "Ward 3", "District 10", 2100000m },
                    { 122, "Ward 4", "District 10", 2150000m },
                    { 123, "Ward 5", "District 10", 2200000m },
                    { 124, "Ward 6", "District 10", 2250000m },
                    { 125, "Ward 7", "District 10", 2300000m },
                    { 126, "Ward 8", "District 10", 2350000m },
                    { 127, "Ward 9", "District 10", 2400000m },
                    { 128, "Ward 10", "District 10", 2450000m },
                    { 129, "Ward 11", "District 10", 2500000m },
                    { 130, "Ward 12", "District 10", 2550000m },
                    { 131, "Ward 13", "District 10", 2600000m },
                    { 132, "Ward 14", "District 10", 2650000m },
                    { 133, "Ward 15", "District 10", 2700000m },
                    { 134, "Ward 1", "District 11", 2000000m },
                    { 135, "Ward 2", "District 11", 2050000m },
                    { 136, "Ward 3", "District 11", 2100000m },
                    { 137, "Ward 4", "District 11", 2150000m },
                    { 138, "Ward 5", "District 11", 2200000m },
                    { 139, "Ward 6", "District 11", 2250000m },
                    { 140, "Ward 7", "District 11", 2300000m },
                    { 141, "Ward 8", "District 11", 2350000m },
                    { 142, "Ward 9", "District 11", 2400000m },
                    { 143, "Ward 10", "District 11", 2450000m },
                    { 144, "Ward 11", "District 11", 2500000m },
                    { 145, "Ward 12", "District 11", 2550000m },
                    { 146, "Ward 13", "District 11", 2600000m },
                    { 147, "Ward 14", "District 11", 2650000m },
                    { 148, "Ward 15", "District 11", 2700000m },
                    { 149, "Ward 16", "District 11", 2750000m },
                    { 150, "An Phu Dong Ward", "District 12", 2000000m },
                    { 151, "Dong Hung Thuan Ward", "District 12", 2050000m },
                    { 152, "Hiep Thanh Ward", "District 12", 2100000m },
                    { 153, "Tan Chanh Hiep Ward", "District 12", 2150000m },
                    { 154, "Tan Hung Thuan Ward", "District 12", 2200000m },
                    { 155, "Tan Thoi Hiep Ward", "District 12", 2250000m },
                    { 156, "Tan Thoi Nhat Ward", "District 12", 2300000m },
                    { 157, "Thanh Loc Ward", "District 12", 2350000m },
                    { 158, "Thanh Xuan Ward", "District 12", 2400000m },
                    { 159, "Thoi An Ward", "District 12", 2450000m },
                    { 160, "Trung My Tay Ward", "District 12", 2500000m },
                    { 161, "Ward 1", "Go Vap District", 2000000m },
                    { 162, "Ward 3", "Go Vap District", 2050000m },
                    { 163, "Ward 4", "Go Vap District", 2100000m },
                    { 164, "Ward 5", "Go Vap District", 2150000m },
                    { 165, "Ward 6", "Go Vap District", 2200000m },
                    { 166, "Ward 7", "Go Vap District", 2250000m },
                    { 167, "Ward 8", "Go Vap District", 2300000m },
                    { 168, "Ward 9", "Go Vap District", 2350000m },
                    { 169, "Ward 10", "Go Vap District", 2400000m },
                    { 170, "Ward 11", "Go Vap District", 2450000m },
                    { 171, "Ward 12", "Go Vap District", 2500000m },
                    { 172, "Ward 13", "Go Vap District", 2550000m },
                    { 173, "Ward 14", "Go Vap District", 2600000m },
                    { 174, "Ward 15", "Go Vap District", 2650000m },
                    { 175, "Ward 16", "Go Vap District", 2700000m },
                    { 176, "Ward 17", "Go Vap District", 2750000m },
                    { 177, "Ward 1", "Tan Binh District", 2000000m },
                    { 178, "Ward 2", "Tan Binh District", 2050000m },
                    { 179, "Ward 3", "Tan Binh District", 2100000m },
                    { 180, "Ward 4", "Tan Binh District", 2150000m },
                    { 181, "Ward 5", "Tan Binh District", 2200000m },
                    { 182, "Ward 6", "Tan Binh District", 2250000m },
                    { 183, "Ward 7", "Tan Binh District", 2300000m },
                    { 184, "Ward 8", "Tan Binh District", 2350000m },
                    { 185, "Ward 9", "Tan Binh District", 2400000m },
                    { 186, "Ward 10", "Tan Binh District", 2450000m },
                    { 187, "Ward 11", "Tan Binh District", 2500000m },
                    { 188, "Ward 12", "Tan Binh District", 2550000m },
                    { 189, "Ward 13", "Tan Binh District", 2600000m },
                    { 190, "Ward 14", "Tan Binh District", 2650000m },
                    { 191, "Ward 15", "Tan Binh District", 2700000m },
                    { 192, "Hiep Tan Ward", "Tan Phu District", 2000000m },
                    { 193, "Hoa Thanh Ward", "Tan Phu District", 2050000m },
                    { 194, "Phu Tho Hoa Ward", "Tan Phu District", 2100000m },
                    { 195, "Phu Thanh Ward", "Tan Phu District", 2150000m },
                    { 196, "Phu Trung Ward", "Tan Phu District", 2200000m },
                    { 197, "Tan Quy Ward", "Tan Phu District", 2250000m },
                    { 198, "Tan Thanh Ward", "Tan Phu District", 2300000m },
                    { 199, "Tan Son Nhi Ward", "Tan Phu District", 2350000m },
                    { 200, "Tan Thoi Hoa Ward", "Tan Phu District", 2400000m },
                    { 201, "Tay Thanh Ward", "Tan Phu District", 2450000m },
                    { 202, "Son Ky Ward", "Tan Phu District", 2500000m },
                    { 203, "Ward 1", "Binh Thanh District", 2000000m },
                    { 204, "Ward 2", "Binh Thanh District", 2050000m },
                    { 205, "Ward 3", "Binh Thanh District", 2100000m },
                    { 206, "Ward 5", "Binh Thanh District", 2150000m },
                    { 207, "Ward 6", "Binh Thanh District", 2200000m },
                    { 208, "Ward 7", "Binh Thanh District", 2250000m },
                    { 209, "Ward 11", "Binh Thanh District", 2300000m },
                    { 210, "Ward 12", "Binh Thanh District", 2350000m },
                    { 211, "Ward 13", "Binh Thanh District", 2400000m },
                    { 212, "Ward 14", "Binh Thanh District", 2450000m },
                    { 213, "Ward 15", "Binh Thanh District", 2500000m },
                    { 214, "Ward 17", "Binh Thanh District", 2550000m },
                    { 215, "Ward 19", "Binh Thanh District", 2600000m },
                    { 216, "Ward 21", "Binh Thanh District", 2650000m },
                    { 217, "Ward 22", "Binh Thanh District", 2700000m },
                    { 218, "Ward 24", "Binh Thanh District", 2750000m },
                    { 219, "Ward 25", "Binh Thanh District", 2800000m },
                    { 220, "Ward 26", "Binh Thanh District", 2850000m },
                    { 221, "Ward 27", "Binh Thanh District", 2900000m },
                    { 222, "Ward 28", "Binh Thanh District", 2950000m },
                    { 223, "Ward 1", "Phu Nhuan District", 2000000m },
                    { 224, "Ward 2", "Phu Nhuan District", 2050000m },
                    { 225, "Ward 3", "Phu Nhuan District", 2100000m },
                    { 226, "Ward 4", "Phu Nhuan District", 2150000m },
                    { 227, "Ward 5", "Phu Nhuan District", 2200000m },
                    { 228, "Ward 7", "Phu Nhuan District", 2250000m },
                    { 229, "Ward 8", "Phu Nhuan District", 2300000m },
                    { 230, "Ward 9", "Phu Nhuan District", 2350000m },
                    { 231, "Ward 10", "Phu Nhuan District", 2400000m },
                    { 232, "Ward 11", "Phu Nhuan District", 2450000m },
                    { 233, "Ward 12", "Phu Nhuan District", 2500000m },
                    { 234, "Ward 13", "Phu Nhuan District", 2550000m },
                    { 235, "Ward 14", "Phu Nhuan District", 2600000m },
                    { 236, "Ward 15", "Phu Nhuan District", 2650000m },
                    { 237, "Ward 17", "Phu Nhuan District", 2700000m },
                    { 238, "Binh Chieu Ward", "Thu Duc District", 2000000m },
                    { 239, "Binh Tho Ward", "Thu Duc District", 2050000m },
                    { 240, "Hiep Binh Chanh Ward", "Thu Duc District", 2100000m },
                    { 241, "Hiep Binh Phuoc Ward", "Thu Duc District", 2150000m },
                    { 242, "Linh Chieu Ward", "Thu Duc District", 2200000m },
                    { 243, "Linh Dong Ward", "Thu Duc District", 2250000m },
                    { 244, "Linh Tay Ward", "Thu Duc District", 2300000m },
                    { 245, "Linh Trung Ward", "Thu Duc District", 2350000m },
                    { 246, "Linh Xuan Ward", "Thu Duc District", 2400000m },
                    { 247, "Tam Binh Ward", "Thu Duc District", 2450000m },
                    { 248, "Tam Phu Ward", "Thu Duc District", 2500000m },
                    { 249, "Truong Tho Ward", "Thu Duc District", 2550000m },
                    { 250, "Binh Hung Hoa Ward", "Binh Tan District", 2000000m },
                    { 251, "Binh Hung Hoa A Ward", "Binh Tan District", 2050000m },
                    { 252, "Binh Hung Hoa B Ward", "Binh Tan District", 2100000m },
                    { 253, "Binh Tri Dong Ward", "Binh Tan District", 2150000m },
                    { 254, "Binh Tri Dong A Ward", "Binh Tan District", 2200000m },
                    { 255, "Binh Tri Dong B Ward", "Binh Tan District", 2250000m },
                    { 256, "Tan Tao Ward", "Binh Tan District", 2300000m },
                    { 257, "Tan Tao A Ward", "Binh Tan District", 2350000m },
                    { 258, "An Lac Ward", "Binh Tan District", 2400000m },
                    { 259, "An Lac A Ward", "Binh Tan District", 2450000m },
                    { 260, "Cu Chi Town", "Cu Chi District", 2000000m },
                    { 261, "Phu My Hung Commune", "Cu Chi District", 2050000m },
                    { 262, "An Phu Commune", "Cu Chi District", 2100000m },
                    { 263, "Trung Lap Thuong Commune", "Cu Chi District", 2150000m },
                    { 264, "An Nhon Tay Commune", "Cu Chi District", 2200000m },
                    { 265, "Nhuan Duc Commune", "Cu Chi District", 2250000m },
                    { 266, "Pham Van Co Commune", "Cu Chi District", 2300000m },
                    { 267, "Phu Hoa Dong Commune", "Cu Chi District", 2350000m },
                    { 268, "Trung Lap Ha Commune", "Cu Chi District", 2400000m },
                    { 269, "Trung An Commune", "Cu Chi District", 2450000m },
                    { 270, "Phuoc Thanh Commune", "Cu Chi District", 2500000m },
                    { 271, "Phuoc Hiep Commune", "Cu Chi District", 2550000m },
                    { 272, "Tan An Hoi Commune", "Cu Chi District", 2600000m },
                    { 273, "Phuoc Vinh An Commune", "Cu Chi District", 2650000m },
                    { 274, "Thai My Commune", "Cu Chi District", 2700000m },
                    { 275, "Tan Thanh Tay Commune", "Cu Chi District", 2750000m },
                    { 276, "Hoa Phu Commune", "Cu Chi District", 2800000m },
                    { 277, "Tan Thanh Dong Commune", "Cu Chi District", 2850000m },
                    { 278, "Binh My Commune", "Cu Chi District", 2900000m },
                    { 279, "Tan Phu Trung Commune", "Cu Chi District", 2950000m },
                    { 280, "Tan Thong Hoi Commune", "Cu Chi District", 3000000m },
                    { 281, "Hoc Mon Town", "Hoc Mon District", 2000000m },
                    { 282, "Ba Diem Commune", "Hoc Mon District", 2050000m },
                    { 283, "Dong Thanh Commune", "Hoc Mon District", 2100000m },
                    { 284, "Nhi Binh Commune", "Hoc Mon District", 2150000m },
                    { 285, "Tan Hiep Commune", "Hoc Mon District", 2200000m },
                    { 286, "Tan Thoi Nhi Commune", "Hoc Mon District", 2250000m },
                    { 287, "Tan Xuan Commune", "Hoc Mon District", 2300000m },
                    { 288, "Thoi Tam Thon Commune", "Hoc Mon District", 2350000m },
                    { 289, "Trung Chanh Commune", "Hoc Mon District", 2400000m },
                    { 290, "Xuan Thoi Dong Commune", "Hoc Mon District", 2450000m },
                    { 291, "Xuan Thoi Son Commune", "Hoc Mon District", 2500000m },
                    { 292, "Xuan Thoi Thuong Commune", "Hoc Mon District", 2550000m },
                    { 293, "Tan Tuc Town", "Binh Chanh District", 2000000m },
                    { 294, "Tan Kien Commune", "Binh Chanh District", 2050000m },
                    { 295, "Tan Nhat Commune", "Binh Chanh District", 2100000m },
                    { 296, "An Phu Tay Commune", "Binh Chanh District", 2150000m },
                    { 297, "Tan Quy Tay Commune", "Binh Chanh District", 2200000m },
                    { 298, "Hung Long Commune", "Binh Chanh District", 2250000m },
                    { 299, "Qui Duc Commune", "Binh Chanh District", 2300000m },
                    { 300, "Binh Chanh Commune", "Binh Chanh District", 2350000m },
                    { 301, "Le Minh Xuan Commune", "Binh Chanh District", 2400000m },
                    { 302, "Pham Van Hai Commune", "Binh Chanh District", 2450000m },
                    { 303, "Binh Hung Commune", "Binh Chanh District", 2500000m },
                    { 304, "Binh Loi Commune", "Binh Chanh District", 2550000m },
                    { 305, "Da Phuoc Commune", "Binh Chanh District", 2600000m },
                    { 306, "Phong Phu Commune", "Binh Chanh District", 2650000m },
                    { 307, "Vinh Loc A Commune", "Binh Chanh District", 2700000m },
                    { 308, "Vinh Loc B Commune", "Binh Chanh District", 2750000m },
                    { 309, "Nha Be Town", "Nha Be District", 2000000m },
                    { 310, "Hiep Phuoc Commune", "Nha Be District", 2050000m },
                    { 311, "Long Thoi Commune", "Nha Be District", 2100000m },
                    { 312, "Nhon Duc Commune", "Nha Be District", 2150000m },
                    { 313, "Phu Xuan Commune", "Nha Be District", 2200000m },
                    { 314, "Phuoc Kien Commune", "Nha Be District", 2250000m },
                    { 315, "Phuoc Loc Commune", "Nha Be District", 2300000m },
                    { 316, "Can Thanh Town", "Can Gio District", 2000000m },
                    { 317, "Binh Khanh Commune", "Can Gio District", 2050000m },
                    { 318, "An Thoi Dong Commune", "Can Gio District", 2100000m },
                    { 319, "Tam Thon Hiep Commune", "Can Gio District", 2150000m },
                    { 320, "Thanh An Commune", "Can Gio District", 2200000m },
                    { 321, "Ly Nhon Commune", "Can Gio District", 2250000m },
                    { 322, "Long Hoa Commune", "Can Gio District", 2300000m }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "PaymentID", "Qrcode", "Type" },
                values: new object[,]
                {
                    { 1, null, "In Cash" },
                    { 2, "qrcode2", "VNPay" }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "ServiceID", "Description", "EstimatedDuration", "ImageURL", "Price", "ServiceName" },
                values: new object[,]
                {
                    { 1, "A general health check for Koi fish.", 1.5, null, 150.00m, "Koi Health Check" },
                    { 2, "Comprehensive pool maintenance service.", 2.0, null, 250.00m, "Pool Maintenance" }
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
