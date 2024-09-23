﻿// <auto-generated />
using System;
using KoiFishCare.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace KoiFishCare.Migrations
{
    [DbContext(typeof(KoiFishVeterinaryServiceContext))]
    partial class KoiFishVeterinaryServiceContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("KoiFishCare.Models.Booking", b =>
                {
                    b.Property<int>("BookingID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookingID"));

                    b.Property<DateOnly?>("BookingDate")
                        .HasColumnType("date");

                    b.Property<int>("BookingStatus")
                        .HasColumnType("int");

                    b.Property<string>("CustomerID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("DistanceID")
                        .HasColumnType("int");

                    b.Property<int?>("KoiOrPoolID")
                        .HasColumnType("int");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MeetURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PaymentID")
                        .HasColumnType("int");

                    b.Property<int?>("ServiceID")
                        .HasColumnType("int");

                    b.Property<int?>("SlotID")
                        .HasColumnType("int");

                    b.Property<decimal?>("TotalAmount")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("VetID")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("BookingID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("DistanceID");

                    b.HasIndex("KoiOrPoolID");

                    b.HasIndex("PaymentID");

                    b.HasIndex("ServiceID");

                    b.HasIndex("SlotID");

                    b.HasIndex("VetID");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("KoiFishCare.Models.Distance", b =>
                {
                    b.Property<int>("DistanceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DistanceID"));

                    b.Property<decimal>("Kilometer")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("DistanceID");

                    b.ToTable("Distances");
                });

            modelBuilder.Entity("KoiFishCare.Models.Feedback", b =>
                {
                    b.Property<int>("FeedbackID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FeedbackID"));

                    b.Property<int?>("BookingID")
                        .HasColumnType("int");

                    b.Property<string>("Comments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Rate")
                        .HasColumnType("int");

                    b.HasKey("FeedbackID");

                    b.HasIndex("BookingID");

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("KoiFishCare.Models.KoiOrPool", b =>
                {
                    b.Property<int>("KoiOrPoolID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("KoiOrPoolID"));

                    b.Property<string>("CustomerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsPool")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("KoiOrPoolID");

                    b.HasIndex("CustomerId");

                    b.ToTable("KoiOrPools");
                });

            modelBuilder.Entity("KoiFishCare.Models.Payment", b =>
                {
                    b.Property<int>("PaymentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentID"));

                    b.Property<string>("Qrcode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PaymentID");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("KoiFishCare.Models.PrescriptionRecord", b =>
                {
                    b.Property<int?>("PrescriptionRecordID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("PrescriptionRecordID"));

                    b.Property<int?>("BookingID")
                        .HasColumnType("int");

                    b.Property<string>("DiseaseName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Medication")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Symptoms")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PrescriptionRecordID");

                    b.HasIndex("BookingID");

                    b.ToTable("PrescriptionRecords");
                });

            modelBuilder.Entity("KoiFishCare.Models.Service", b =>
                {
                    b.Property<int>("ServiceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ServiceID"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("EstimatedDuration")
                        .HasColumnType("float");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ServiceID");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("KoiFishCare.Models.Slot", b =>
                {
                    b.Property<int>("SlotID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SlotID"));

                    b.Property<TimeOnly>("EndTime")
                        .HasColumnType("time");

                    b.Property<TimeOnly>("StartTime")
                        .HasColumnType("time");

                    b.Property<DateOnly>("WeekDate")
                        .HasColumnType("date");

                    b.HasKey("SlotID");

                    b.ToTable("Slots");
                });

            modelBuilder.Entity("KoiFishCare.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Gender")
                        .HasColumnType("bit");

                    b.Property<string>("ImagePublicId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("KoiFishCare.Models.VetSlot", b =>
                {
                    b.Property<string>("VetID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("SlotID")
                        .HasColumnType("int");

                    b.HasKey("VetID", "SlotID");

                    b.HasIndex("SlotID");

                    b.ToTable("VetSlots");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("KoiFishCare.Models.Customer", b =>
                {
                    b.HasBaseType("KoiFishCare.Models.User");

                    b.Property<string>("DefaultAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Customers", (string)null);
                });

            modelBuilder.Entity("KoiFishCare.Models.Staff", b =>
                {
                    b.HasBaseType("KoiFishCare.Models.User");

                    b.Property<bool?>("IsManager")
                        .HasColumnType("bit");

                    b.Property<string>("ManagerID")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Staffs", (string)null);
                });

            modelBuilder.Entity("KoiFishCare.Models.Veterinarian", b =>
                {
                    b.HasBaseType("KoiFishCare.Models.User");

                    b.Property<int>("ExperienceYears")
                        .HasColumnType("int");

                    b.ToTable("Veterinarians", (string)null);
                });

            modelBuilder.Entity("KoiFishCare.Models.Booking", b =>
                {
                    b.HasOne("KoiFishCare.Models.Customer", "Customer")
                        .WithMany("Bookings")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KoiFishCare.Models.Distance", "Distance")
                        .WithMany("Bookings")
                        .HasForeignKey("DistanceID");

                    b.HasOne("KoiFishCare.Models.KoiOrPool", "KoiOrPool")
                        .WithMany("Bookings")
                        .HasForeignKey("KoiOrPoolID");

                    b.HasOne("KoiFishCare.Models.Payment", "Payment")
                        .WithMany("Bookings")
                        .HasForeignKey("PaymentID");

                    b.HasOne("KoiFishCare.Models.Service", "Service")
                        .WithMany("Bookings")
                        .HasForeignKey("ServiceID");

                    b.HasOne("KoiFishCare.Models.Slot", "Slot")
                        .WithMany("Bookings")
                        .HasForeignKey("SlotID");

                    b.HasOne("KoiFishCare.Models.Veterinarian", "Vet")
                        .WithMany("Bookings")
                        .HasForeignKey("VetID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Customer");

                    b.Navigation("Distance");

                    b.Navigation("KoiOrPool");

                    b.Navigation("Payment");

                    b.Navigation("Service");

                    b.Navigation("Slot");

                    b.Navigation("Vet");
                });

            modelBuilder.Entity("KoiFishCare.Models.Feedback", b =>
                {
                    b.HasOne("KoiFishCare.Models.Booking", "Booking")
                        .WithMany()
                        .HasForeignKey("BookingID");

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("KoiFishCare.Models.KoiOrPool", b =>
                {
                    b.HasOne("KoiFishCare.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("KoiFishCare.Models.PrescriptionRecord", b =>
                {
                    b.HasOne("KoiFishCare.Models.Booking", "Booking")
                        .WithMany()
                        .HasForeignKey("BookingID");

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("KoiFishCare.Models.VetSlot", b =>
                {
                    b.HasOne("KoiFishCare.Models.Slot", "Slot")
                        .WithMany("VetSlots")
                        .HasForeignKey("SlotID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KoiFishCare.Models.Veterinarian", "Veterinarian")
                        .WithMany("VetSlots")
                        .HasForeignKey("VetID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Slot");

                    b.Navigation("Veterinarian");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("KoiFishCare.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("KoiFishCare.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KoiFishCare.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("KoiFishCare.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("KoiFishCare.Models.Customer", b =>
                {
                    b.HasOne("KoiFishCare.Models.User", null)
                        .WithOne()
                        .HasForeignKey("KoiFishCare.Models.Customer", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("KoiFishCare.Models.Staff", b =>
                {
                    b.HasOne("KoiFishCare.Models.User", null)
                        .WithOne()
                        .HasForeignKey("KoiFishCare.Models.Staff", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("KoiFishCare.Models.Veterinarian", b =>
                {
                    b.HasOne("KoiFishCare.Models.User", null)
                        .WithOne()
                        .HasForeignKey("KoiFishCare.Models.Veterinarian", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("KoiFishCare.Models.Distance", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("KoiFishCare.Models.KoiOrPool", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("KoiFishCare.Models.Payment", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("KoiFishCare.Models.Service", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("KoiFishCare.Models.Slot", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("VetSlots");
                });

            modelBuilder.Entity("KoiFishCare.Models.Customer", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("KoiFishCare.Models.Veterinarian", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("VetSlots");
                });
#pragma warning restore 612, 618
        }
    }
}
