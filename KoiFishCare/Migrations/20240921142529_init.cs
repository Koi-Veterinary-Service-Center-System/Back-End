using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoiFishCare.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Account__349DA586AD67902E", x => x.AccountID);
                });

            migrationBuilder.CreateTable(
                name: "Distance",
                columns: table => new
                {
                    DistanceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Kilometer = table.Column<decimal>(type: "decimal(5,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Distance__A24E2A1C7F790CA5", x => x.DistanceID);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    PaymentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QRCode = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Payment__9B556A58D9830F1F", x => x.PaymentID);
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    ServiceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Estimated_duration = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Service__C51BB0EA33D1CD3F", x => x.ServiceID);
                });

            migrationBuilder.CreateTable(
                name: "Slot",
                columns: table => new
                {
                    SlotID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    WeekDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Slot__0A124A4F64E36AD6", x => x.SlotID);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValueSql: "('C'+left(newid(),(8)))"),
                    AccountID = table.Column<int>(type: "int", nullable: true),
                    FirstName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Gender = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    DefaultAddress = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    ImageURL = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Customer__A4AE64B8E0E2996A", x => x.CustomerID);
                    table.ForeignKey(
                        name: "FK__Customer__Accoun__412EB0B6",
                        column: x => x.AccountID,
                        principalTable: "Account",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    StaffID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValueSql: "('S'+left(newid(),(8)))"),
                    AccountID = table.Column<int>(type: "int", nullable: true),
                    FirstName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Gender = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    ImageURL = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    IsManager = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Staff__96D4AAF72FA07439", x => x.StaffID);
                    table.ForeignKey(
                        name: "FK__Staff__AccountID__3B75D760",
                        column: x => x.AccountID,
                        principalTable: "Account",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateTable(
                name: "Veterinarian",
                columns: table => new
                {
                    VetID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValueSql: "('V'+left(newid(),(8)))"),
                    AccountID = table.Column<int>(type: "int", nullable: true),
                    FirstName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Gender = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    Experience_years = table.Column<int>(type: "int", nullable: true),
                    ImageURL = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Veterina__2556B80E6CF88BB2", x => x.VetID);
                    table.ForeignKey(
                        name: "FK__Veterinar__Accou__45F365D3",
                        column: x => x.AccountID,
                        principalTable: "Account",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateTable(
                name: "KoiOrPool",
                columns: table => new
                {
                    KoiOrPoolID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValueSql: "('K'+left(newid(),(8)))"),
                    CustomerID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    IsPool = table.Column<bool>(type: "bit", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__KoiOrPoo__0612556F20091BA1", x => x.KoiOrPoolID);
                    table.ForeignKey(
                        name: "FK__KoiOrPool__Custo__5535A963",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID");
                });

            migrationBuilder.CreateTable(
                name: "Having",
                columns: table => new
                {
                    VetID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    SlotID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Having__C5F79CAAFED7EB1E", x => new { x.VetID, x.SlotID });
                    table.ForeignKey(
                        name: "FK__Having__SlotID__4D94879B",
                        column: x => x.SlotID,
                        principalTable: "Slot",
                        principalColumn: "SlotID");
                    table.ForeignKey(
                        name: "FK__Having__VetID__4CA06362",
                        column: x => x.VetID,
                        principalTable: "Veterinarian",
                        principalColumn: "VetID");
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    BookingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceID = table.Column<int>(type: "int", nullable: true),
                    PaymentID = table.Column<int>(type: "int", nullable: true),
                    SlotID = table.Column<int>(type: "int", nullable: true),
                    CustomerID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    VetID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    DistanceID = table.Column<int>(type: "int", nullable: true),
                    KoiOrPoolID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    BookingDate = table.Column<DateOnly>(type: "date", nullable: true),
                    BookingTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    Location = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Booking__73951ACD523EDE51", x => x.BookingID);
                    table.ForeignKey(
                        name: "FK__Booking__Custome__5AEE82B9",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID");
                    table.ForeignKey(
                        name: "FK__Booking__Distanc__5CD6CB2B",
                        column: x => x.DistanceID,
                        principalTable: "Distance",
                        principalColumn: "DistanceID");
                    table.ForeignKey(
                        name: "FK__Booking__KoiOrPo__5DCAEF64",
                        column: x => x.KoiOrPoolID,
                        principalTable: "KoiOrPool",
                        principalColumn: "KoiOrPoolID");
                    table.ForeignKey(
                        name: "FK__Booking__Payment__59063A47",
                        column: x => x.PaymentID,
                        principalTable: "Payment",
                        principalColumn: "PaymentID");
                    table.ForeignKey(
                        name: "FK__Booking__Service__5812160E",
                        column: x => x.ServiceID,
                        principalTable: "Service",
                        principalColumn: "ServiceID");
                    table.ForeignKey(
                        name: "FK__Booking__SlotID__59FA5E80",
                        column: x => x.SlotID,
                        principalTable: "Slot",
                        principalColumn: "SlotID");
                    table.ForeignKey(
                        name: "FK__Booking__VetID__5BE2A6F2",
                        column: x => x.VetID,
                        principalTable: "Veterinarian",
                        principalColumn: "VetID");
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    FeedbackID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingID = table.Column<int>(type: "int", nullable: true),
                    Rate = table.Column<int>(type: "int", nullable: true),
                    Comments = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Feedback__6A4BEDF67764D7CE", x => x.FeedbackID);
                    table.ForeignKey(
                        name: "FK__Feedback__Bookin__60A75C0F",
                        column: x => x.BookingID,
                        principalTable: "Booking",
                        principalColumn: "BookingID");
                });

            migrationBuilder.CreateTable(
                name: "PrescriptionRecord",
                columns: table => new
                {
                    PrescriptionRecordID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingID = table.Column<int>(type: "int", nullable: true),
                    DiseaseName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Symptoms = table.Column<string>(type: "text", nullable: true),
                    Medication = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Prescrip__1D4AC3184A0205C0", x => x.PrescriptionRecordID);
                    table.ForeignKey(
                        name: "FK__Prescript__Booki__6383C8BA",
                        column: x => x.BookingID,
                        principalTable: "Booking",
                        principalColumn: "BookingID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_CustomerID",
                table: "Booking",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_DistanceID",
                table: "Booking",
                column: "DistanceID");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_KoiOrPoolID",
                table: "Booking",
                column: "KoiOrPoolID");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_PaymentID",
                table: "Booking",
                column: "PaymentID");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_ServiceID",
                table: "Booking",
                column: "ServiceID");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_SlotID",
                table: "Booking",
                column: "SlotID");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_VetID",
                table: "Booking",
                column: "VetID");

            migrationBuilder.CreateIndex(
                name: "UQ__Customer__349DA5876005DF31",
                table: "Customer",
                column: "AccountID",
                unique: true,
                filter: "[AccountID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_BookingID",
                table: "Feedback",
                column: "BookingID");

            migrationBuilder.CreateIndex(
                name: "IX_Having_SlotID",
                table: "Having",
                column: "SlotID");

            migrationBuilder.CreateIndex(
                name: "IX_KoiOrPool_CustomerID",
                table: "KoiOrPool",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionRecord_BookingID",
                table: "PrescriptionRecord",
                column: "BookingID");

            migrationBuilder.CreateIndex(
                name: "UQ__Staff__349DA587A55D4CB8",
                table: "Staff",
                column: "AccountID",
                unique: true,
                filter: "[AccountID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ__Veterina__349DA5875B7204C7",
                table: "Veterinarian",
                column: "AccountID",
                unique: true,
                filter: "[AccountID] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "Having");

            migrationBuilder.DropTable(
                name: "PrescriptionRecord");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "Distance");

            migrationBuilder.DropTable(
                name: "KoiOrPool");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "Slot");

            migrationBuilder.DropTable(
                name: "Veterinarian");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
