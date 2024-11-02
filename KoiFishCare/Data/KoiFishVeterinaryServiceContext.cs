using System;
using System.Collections.Generic;
using KoiFishCare.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace KoiFishCare.Data;

public partial class KoiFishVeterinaryServiceContext : IdentityDbContext<User>
{

    public KoiFishVeterinaryServiceContext(DbContextOptions<KoiFishVeterinaryServiceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Distance> Distances { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PrescriptionRecord> PrescriptionRecords { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Slot> Slots { get; set; }

    public virtual DbSet<VetSlot> VetSlots { get; set; }

    public virtual DbSet<BookingRecord> BookingRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ---- Payment Relationship -----------------------------------------------------------------------
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Payment)
            .WithMany(p => p.Bookings)  // Assuming Payment has a Bookings collection
            .HasForeignKey(b => b.PaymentID)
            .OnDelete(DeleteBehavior.Restrict);  // No cascading delete to avoid cycles

        // ---- Service Relationship -----------------------------------------------------------------------
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Service)
            .WithMany(s => s.Bookings)  // Assuming Service has a Bookings collection
            .HasForeignKey(b => b.ServiceID)
            .OnDelete(DeleteBehavior.Restrict);  // No cascading delete

        // ---- Slot Relationship -----------------------------------------------------------------------
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Slot)
            .WithMany(s => s.Bookings)  // Assuming Slot has a Bookings collection
            .HasForeignKey(b => b.SlotID)
            .OnDelete(DeleteBehavior.Restrict);  // No cascading delete

        // ---- Customer Relationship -----------------------------------------------------------------------
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Customer)
            .WithMany(c => c.CustomerBookings)  // Assuming Customer has a Bookings collection
            .HasForeignKey(b => b.CustomerID)
            .OnDelete(DeleteBehavior.Restrict);  // Cascade delete when Customer is deleted

        // ---- Veterinarian Relationship -----------------------------------------------------------------------
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Veterinarian)
            .WithMany(v => v.VetBookings)  // Assuming Veterinarian has a Bookings collection
            .HasForeignKey(b => b.VetID)
            .OnDelete(DeleteBehavior.Restrict);  // No cascading delete

        // ---- VetSlot Relationship -----------------------------------------------------------------------
        modelBuilder.Entity<VetSlot>()
        .HasKey(vs => new { vs.VetID, vs.SlotID });

        // ---- Veterinarian Relationship -----------------------------------------------------------------------
        modelBuilder.Entity<VetSlot>()
          .HasOne(vs => vs.Veterinarian)
          .WithMany(v => v.VetSlots)
          .HasForeignKey(vs => vs.VetID);

        // ---- Slot Relationship -----------------------------------------------------------------------
        modelBuilder.Entity<VetSlot>()
          .HasOne(vs => vs.Slot)
          .WithMany(s => s.VetSlots)
          .HasForeignKey(vs => vs.SlotID);

        //---------- Add Role -----------------------------------------------------------------
        var managerRole = new IdentityRole
        {
            Id = "4b73e212-5d38-4711-8336-f299801120b7",
            Name = "Manager",
            NormalizedName = "MANAGER"
        };

        var staffRole = new IdentityRole
        {
            Id = "40ff3214-4004-4c03-ac3f-806b99feb7dd",
            Name = "Staff",
            NormalizedName = "STAFF"
        };

        var vetRole = new IdentityRole
        {
            Id = "b2c5b5c7-5578-4ca8-8fd4-a8ca10b81a4f",
            Name = "Vet",
            NormalizedName = "VET"
        };

        var customerRole = new IdentityRole
        {
            Id = "1e0930e3-64c0-4691-be42-3d4030eaac7c",
            Name = "Customer",
            NormalizedName = "CUSTOMER"
        };

        modelBuilder.Entity<IdentityRole>().HasData(managerRole, staffRole, vetRole, customerRole);

        //SEED DATA=================

        // Seed data for User
        var hasher = new PasswordHasher<User>();
        var users = new List<User>()
        {
        //---------- Manager -----------------------------------------------------------------
            new User {
                Id = "m1",
                IsManager = true,
                UserName = "Tphung_2004",
                NormalizedUserName = "TPHUNG_2004",
                FirstName = "Anh",
                LastName = "Phung",
                Gender = false,
                Email = "tphung1435@gmail.com",
                NormalizedEmail = "TPHUNG1435@GMAIL.COM",
                Address = "Hem Nho",
                ImageURL = "https://firebasestorage.googleapis.com/v0/b/swp391veterinary.appspot.com/o/profile_pictures%2F6d2fcfc3-eb91-47ae-8324-9864beda4ff3.jpeg?alt=media&token=77e68195-a40b-44ea-af8f-87bb077b0a89",
                ImagePublicId = null,
                PhoneNumber = "0786542387",
                ExperienceYears = null,
            },

        //---------- Staff -----------------------------------------------------------------
                new User {
                Id = "s1",
                IsManager = false,
                ManagerID = "m1",
                UserName = "Thanhdc_1229",
                NormalizedUserName = "THANHDC_1229",
                FirstName = "Cong",
                LastName = "Thanh",
                Gender = true,
                Email = "thanhdc1229@gmail.com",
                NormalizedEmail = "THANHDC1229@GMAIL.COM",
                Address = "VinCom Le Van Viet",
                ImageURL = "https://firebasestorage.googleapis.com/v0/b/swp391veterinary.appspot.com/o/profile_pictures%2Fz5986693920790_205b874ed5326f1810ec835d7b3cb2b8.jpg?alt=media&token=8ec317f5-d8b0-4d2b-a599-bc16aaca03ff",
                ImagePublicId = null,
                PhoneNumber = "0799981696",
                ExperienceYears = null,
            },

        //---------- Customer -----------------------------------------------------------------
            new User {
                Id = "c1",
                IsManager = false,
                UserName = "Dkhoa_Happy",
                NormalizedUserName = "DKHOA_HAPPY",
                FirstName = "Dang",
                LastName = "Khoa",
                Gender = true,
                Email = "dangkhoa13978@gmail.com",
                NormalizedEmail = "DANGKHOA13978@GMAIL.COM",
                Address = "Bui Minh Truc",
                ImageURL = "https://firebasestorage.googleapis.com/v0/b/swp391veterinary.appspot.com/o/profile_pictures%2F7720cf1b-bf61-466a-b82a-7a553a9da5cc.jpeg?alt=media&token=ce545418-5224-4b57-81bb-f431d4752d67",
                ImagePublicId = null,
                PhoneNumber = "0972513978",
                ExperienceYears = null,
            },

        //---------- Veterinarian -----------------------------------------------------------------
            new User {
                Id = "v1",
                IsManager = false,
                ManagerID = "m1",
                UserName = "NhatNguyen_1229",
                NormalizedUserName = "NHATNGUYEN_1229",
                FirstName = "Nhat",
                LastName = "Nguyen",
                Gender = true,
                Email = "nhatnguyense186475@fpt.edu.vn",
                NormalizedEmail = "NHATNGUYENSE186475@FPT.EDU.VN",
                Address = "Phu My Hung",
                ImageURL = "https://firebasestorage.googleapis.com/v0/b/swp391veterinary.appspot.com/o/profile_pictures%2FnhatNguyen.png?alt=media&token=dd5ce883-e50a-49e8-9393-7739f188389e",
                ImagePublicId = null,
                PhoneNumber = "0873457689",
                ExperienceYears = 10,
            },

            new User {
                Id = "v2",
                IsManager = false,
                ManagerID = "m1",
                UserName = "MinhLu_2004",
                NormalizedUserName = "MINHLU_2004",
                FirstName = "Minh",
                LastName = "E",
                Gender = true,
                Email = "minhlu1476@gmail.com",
                NormalizedEmail = "MINHLU1476@GMAIL.COM",
                Address = "Le Van Viet",
                ImageURL = "https://firebasestorage.googleapis.com/v0/b/swp391veterinary.appspot.com/o/profile_pictures%2F346077162_980458369794854_6154481088254840308_n.jpg?alt=media&token=11e2d80f-b7f0-463c-8531-246172cc214f",
                ImagePublicId = null,
                PhoneNumber = "0762431687",
                ExperienceYears = 5,
            },

            new User {
                Id = "v3",
                IsManager = false,
                ManagerID = "m1",
                UserName = "Chau_2004",
                NormalizedUserName = "CHAU_2004",
                FirstName = "Chau",
                LastName = "Lee",
                Gender = true,
                Email = "ChauLee123@gmail.com",
                NormalizedEmail = "CHAULEE123@GMAIL.COM",
                Address = "Lam Van Ben",
                ImageURL = null,
                ImagePublicId = null,
                PhoneNumber = "0123456789",
                ExperienceYears = 8,
            },

            new User {
                Id = "v4",
                IsManager = false,
                ManagerID = "m1",
                UserName = "AnNTP_1904",
                NormalizedUserName = "ANNTP_1904",
                FirstName = "An",
                LastName = "Nguyen",
                Gender = false,
                Email = "AnNTP1904@gmail.com",
                NormalizedEmail = "AnNTP1904@GMAIL.COM",
                Address = "Dong Nai",
                ImageURL = null,
                ImagePublicId = null,
                PhoneNumber = "0967812345",
                ExperienceYears = 7,
            },

            new User {
                Id = "v5",
                IsManager = false,
                ManagerID = "m1",
                UserName = "AnhPham_2909",
                NormalizedUserName = "ANHPHAM_2909",
                FirstName = "Anh",
                LastName = "Pham",
                Gender = true,
                Email = "AnhPham2909@gmail.com",
                NormalizedEmail = "ANHPHAM2909@GMAIL.COM",
                Address = "Thao Dien",
                ImageURL = null,
                ImagePublicId = null,
                PhoneNumber = "0987654321",
                ExperienceYears = 9,
            }
        };

        foreach (var user in users)
        {
            user.PasswordHash = hasher.HashPassword(user, "String123@");
        }

        modelBuilder.Entity<User>().HasData(users);

        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                UserId = "m1",
                RoleId = "4b73e212-5d38-4711-8336-f299801120b7",
            },

            new IdentityUserRole<string>
            {
                UserId = "s1",
                RoleId = "40ff3214-4004-4c03-ac3f-806b99feb7dd",
            },

            new IdentityUserRole<string>
            {
                UserId = "c1",
                RoleId = "1e0930e3-64c0-4691-be42-3d4030eaac7c",
            },

            new IdentityUserRole<string>
            {
                UserId = "v1",
                RoleId = "b2c5b5c7-5578-4ca8-8fd4-a8ca10b81a4f",
            },

            new IdentityUserRole<string>
            {
                UserId = "v2",
                RoleId = "b2c5b5c7-5578-4ca8-8fd4-a8ca10b81a4f",
            },

            new IdentityUserRole<string>
            {
                UserId = "v3",
                RoleId = "b2c5b5c7-5578-4ca8-8fd4-a8ca10b81a4f",
            },

            new IdentityUserRole<string>
            {
                UserId = "v4",
                RoleId = "b2c5b5c7-5578-4ca8-8fd4-a8ca10b81a4f",
            },

            new IdentityUserRole<string>
            {
                UserId = "v5",
                RoleId = "b2c5b5c7-5578-4ca8-8fd4-a8ca10b81a4f",
            }
        );

        // Seed data for Slots
        modelBuilder.Entity<Slot>().HasData(
            new Slot
            {
                SlotID = 1,
                StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(10, 0),
                WeekDate = Models.Enum.DayOfWeek.Monday
            },
            new Slot
            {
                SlotID = 2,
                StartTime = new TimeOnly(11, 0),
                EndTime = new TimeOnly(12, 0),
                WeekDate = Models.Enum.DayOfWeek.Monday
            },
            new Slot
            {
                SlotID = 3,
                StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(12, 0),
                WeekDate = Models.Enum.DayOfWeek.Tuesday
            },
            new Slot
            {
                SlotID = 4,
                StartTime = new TimeOnly(11, 0),
                EndTime = new TimeOnly(12, 0),
                WeekDate = Models.Enum.DayOfWeek.Tuesday
            },
            new Slot
            {
                SlotID = 5,
                StartTime = new TimeOnly(11, 0),
                EndTime = new TimeOnly(12, 0),
                WeekDate = Models.Enum.DayOfWeek.Wednesday
            }
        );

        // Seed data for VetSlot (Assuming that the relationship between Vet and Slot is many-to-many)
        modelBuilder.Entity<VetSlot>().HasData(
            new VetSlot
            {
                VetID = "v1",
                SlotID = 1,
                isBooked = false
            },
            new VetSlot
            {
                VetID = "v2",
                SlotID = 1,
                isBooked = false
            },
            new VetSlot
            {
                VetID = "v4",
                SlotID = 1,
                isBooked = false
            },
            new VetSlot
            {
                VetID = "v1",
                SlotID = 2,
                isBooked = false
            },
            new VetSlot
            {
                VetID = "v1",
                SlotID = 3,
                isBooked = false
            },
            new VetSlot
            {
                VetID = "v4",
                SlotID = 3,
                isBooked = false
            },
            new VetSlot
            {
                VetID = "v2",
                SlotID = 4,
                isBooked = false
            },
            new VetSlot
            {
                VetID = "v3",
                SlotID = 4,
                isBooked = false
            },
            new VetSlot
            {
                VetID = "v3",
                SlotID = 5,
                isBooked = false
            },

            new VetSlot
            {
                VetID = "v5",
                SlotID = 5,
                isBooked = false
            }
        );

        // Seed data for Services
        modelBuilder.Entity<Service>().HasData(
            new Service
            {
                ServiceID = 1,
                ServiceName = "Koi Health Check",
                Description = "A general health check for Koi fish.",
                Price = 150000M,
                QuantityPrice = 25000M,
                EstimatedDuration = 1.5,
                ImageURL = "https://cafishvet.com/wp-content/uploads/2024/09/Water-Treatment-Jessie-Sanders-Fish-Vetranarian-1024x683.jpg"
            },
            new Service
            {
                ServiceID = 2,
                ServiceName = "Pool Maintenance",
                Description = "Comprehensive pool maintenance service.",
                Price = 99000M,
                QuantityPrice = 20000M,
                EstimatedDuration = 2.0,
                ImageURL = "https://i.pinimg.com/564x/7b/cc/71/7bcc716d63ec9bc682c019d2aa5090b8.jpg"
            },
            new Service
            {
                ServiceID = 3,
                ServiceName = "Water Quality Testing",
                Description = "Testing the water quality to ensure it is optimal for Koi health.",
                Price = 79000M,
                QuantityPrice = 20000M,
                EstimatedDuration = 1.0,
                ImageURL = "https://cafishvet.com/wp-content/uploads/2020/10/good-water-quality-in-fish-tank-1024x536.jpg"
            },
            new Service
            {
                ServiceID = 4,
                ServiceName = "Disease Treatment",
                Description = "Diagnosing and treating diseases in Koi fish.",
                Price = 199000M,
                QuantityPrice = 25000M,
                EstimatedDuration = 2.5,
                ImageURL = "https://wonkywheels.com/wp-content/uploads/2021/08/koidiseasemat.jpeg"
            },
            new Service
            {
                ServiceID = 5,
                ServiceName = "Koi Pond Inspection",
                Description = "A detailed inspection of your Koi pond to identify any issues or potential improvements.",
                Price = 69000M,
                QuantityPrice = 15000M,
                EstimatedDuration = 1.75,
                ImageURL = "https://youraquariumguide.com/wp-content/uploads/2022/05/Maintenance-Considerations-For-Koi-Pond.jpg"
            },
            new Service
            {
                ServiceID = 6,
                ServiceName = "Koi Breeding Consultation",
                Description = "Expert advice and consultation on breeding healthy Koi fish.",
                Price = 100000M,
                QuantityPrice = 0,
                EstimatedDuration = 3.0,
                ImageURL = "https://i.pinimg.com/564x/10/50/09/105009e291593ad674bc60faed37a5e8.jpg"
            }
        );

        // Seed data for Payments
        modelBuilder.Entity<Payment>().HasData(
            new Payment
            {
                PaymentID = 1,
                Type = "In Cash"
            },
            new Payment
            {
                PaymentID = 2,
                Type = "VNPay"
            }
        );

        // Seed data for Distance
        modelBuilder.Entity<Distance>().HasData(
        // District 1: 10 area
            new Distance
            {
                DistanceID = 1,
                District = "District 1",
                Area = "Ben Nghe Ward",
                Price = 92500M,
            },

            new Distance
            {
                DistanceID = 2,
                District = "District 1",
                Area = "Ben Thanh Ward",
                Price = 98500M,
            },

            new Distance
            {
                DistanceID = 3,
                District = "District 1",
                Area = "Co Giang Ward",
                Price = 97000M,
            },

            new Distance
            {
                DistanceID = 4,
                District = "District 1",
                Area = "Cau Kho Ward",
                Price = 99000M,
            },

            new Distance
            {
                DistanceID = 5,
                District = "District 1",
                Area = "Cau Ong Lanh Ward",
                Price = 95500M,
            },

            new Distance
            {
                DistanceID = 6,
                District = "District 1",
                Area = "Da Kao Ward",
                Price = 93000M,
            },

            new Distance
            {
                DistanceID = 7,
                District = "District 1",
                Area = "Nguyen Thai Binh Ward",
                Price = 94000M,
            },

            new Distance
            {
                DistanceID = 8,
                District = "District 1",
                Area = "Nguyen Cu Trinh Ward",
                Price = 101500M,
            },

            new Distance
            {
                DistanceID = 9,
                District = "District 1",
                Area = "Pham Ngu Lao Ward",
                Price = 101000M,
            },

            new Distance
            {
                DistanceID = 10,
                District = "District 1",
                Area = "Tan Dinh Ward",
                Price = 105000M,
            },

        // District 2: 11 area
            new Distance
            {
                DistanceID = 11,
                District = "District 2",
                Area = "An Khanh Ward",
                Price = 70500M,
            },

            new Distance
            {
                DistanceID = 12,
                District = "District 2",
                Area = "An Loi Dong Ward",
                Price = 84000M,
            },

            new Distance
            {
                DistanceID = 13,
                District = "District 2",
                Area = "An Phu Ward",
                Price = 69500M,
            },

            new Distance
            {
                DistanceID = 14,
                District = "District 2",
                Area = "Binh An Ward",
                Price = 74000M,
            },

            new Distance
            {
                DistanceID = 15,
                District = "District 2",
                Area = "Binh Khanh Ward",
                Price = 70000M,
            },

            new Distance
            {
                DistanceID = 16,
                District = "District 2",
                Area = "Binh Trung Dong Ward",
                Price = 54000M,
            },

            new Distance
            {
                DistanceID = 17,
                District = "District 2",
                Area = "Binh Trung Tay Ward",
                Price = 64000M,
            },

            new Distance
            {
                DistanceID = 18,
                District = "District 2",
                Area = "Cat Lai Ward",
                Price = 62000M,
            },

            new Distance
            {
                DistanceID = 19,
                District = "District 2",
                Area = "Thanh My Loi Ward",
                Price = 65000M,
            },

            new Distance
            {
                DistanceID = 20,
                District = "District 2",
                Area = "Thao Dien Ward",
                Price = 80500M,
            },

            new Distance
            {
                DistanceID = 21,
                District = "District 2",
                Area = "Thu Thiem Ward",
                Price = 82000M,
            },

        // District 3: 14 area
            new Distance
            {
                DistanceID = 22,
                District = "District 2",
                Area = "Ward 1",
                Price = 112500M,
            },

            new Distance
            {
                DistanceID = 23,
                District = "District 2",
                Area = "Ward 2",
                Price = 110000M,
            },

            new Distance
            {
                DistanceID = 24,
                District = "District 2",
                Area = "Ward 3",
                Price = 108000M,
            },

            new Distance
            {
                DistanceID = 25,
                District = "District 2",
                Area = "Ward 4",
                Price = 105500M,
            },

            new Distance
            {
                DistanceID = 26,
                District = "District 2",
                Area = "Ward 5",
                Price = 104000M,
            },

            new Distance
            {
                DistanceID = 27,
                District = "District 2",
                Area = "Ward 6",
                Price = 107500M,
            },

            new Distance
            {
                DistanceID = 28,
                District = "District 2",
                Area = "Ward 7",
                Price = 108000M,
            },

            new Distance
            {
                DistanceID = 29,
                District = "District 2",
                Area = "Ward 8",
                Price = 110500M,
            },

            new Distance
            {
                DistanceID = 30,
                District = "District 2",
                Area = "Ward 9",
                Price = 113000M,
            },

            new Distance
            {
                DistanceID = 31,
                District = "District 2",
                Area = "Ward 10",
                Price = 115500M,
            },

            new Distance
            {
                DistanceID = 32,
                District = "District 2",
                Area = "Ward 11",
                Price = 124500M,
            },

            new Distance
            {
                DistanceID = 33,
                District = "District 2",
                Area = "Ward 12",
                Price = 116000M,
            },

            new Distance
            {
                DistanceID = 34,
                District = "District 2",
                Area = "Ward 13",
                Price = 114500M,
            },

            new Distance
            {
                DistanceID = 35,
                District = "District 2",
                Area = "Ward 14",
                Price = 113000M,
            },

        // District 4: 15 area
            new Distance
            {
                DistanceID = 36,
                District = "District 4",
                Area = "Ward 1",
                Price = 114000M,
            },

            new Distance
            {
                DistanceID = 37,
                District = "District 4",
                Area = "Ward 2",
                Price = 113000M,
            },

            new Distance
            {
                DistanceID = 38,
                District = "District 4",
                Area = "Ward 3",
                Price = 115500M,
            },

            new Distance
            {
                DistanceID = 39,
                District = "District 4",
                Area = "Ward 4",
                Price = 111500M,
            },

            new Distance
            {
                DistanceID = 40,
                District = "District 4",
                Area = "Ward 5",
                Price = 117500M,
            },

            new Distance
            {
                DistanceID = 41,
                District = "District 4",
                Area = "Ward 6",
                Price = 113500M,
            },

            new Distance
            {
                DistanceID = 42,
                District = "District 4",
                Area = "Ward 8",
                Price = 115000M,
            },

            new Distance
            {
                DistanceID = 43,
                District = "District 4",
                Area = "Ward 9",
                Price = 119000M,
            },

            new Distance
            {
                DistanceID = 44,
                District = "District 4",
                Area = "Ward 10",
                Price = 117000M,
            },

            new Distance
            {
                DistanceID = 45,
                District = "District 4",
                Area = "Ward 12",
                Price = 114500M,
            },

            new Distance
            {
                DistanceID = 46,
                District = "District 4",
                Area = "Ward 13",
                Price = 120500M,
            },

            new Distance
            {
                DistanceID = 47,
                District = "District 4",
                Area = "Ward 14",
                Price = 118500M,
            },

            new Distance
            {
                DistanceID = 48,
                District = "District 4",
                Area = "Ward 15",
                Price = 112000M,
            },

            new Distance
            {
                DistanceID = 49,
                District = "District 4",
                Area = "Ward 16",
                Price = 108500M,
            },

            new Distance
            {
                DistanceID = 50,
                District = "District 4",
                Area = "Ward 18",
                Price = 102500M,
            },

        // District 5: 15 area 
            new Distance
            {
                DistanceID = 51,
                District = "District 5",
                Area = "Ward 1",
                Price = 107500M
            },

            new Distance
            {
                DistanceID = 52,
                District = "District 5",
                Area = "Ward 2",
                Price = 105500M
            },

            new Distance
            {
                DistanceID = 53,
                District = "District 5",
                Area = "Ward 3",
                Price = 106500M,
            },

            new Distance
            {
                DistanceID = 54,
                District = "District 5",
                Area = "Ward 4",
                Price = 104000M,
            },

            new Distance
            {
                DistanceID = 55,
                District = "District 5",
                Area = "Ward 5",
                Price = 103000M,
            },

            new Distance
            {
                DistanceID = 56,
                District = "District 5",
                Area = "Ward 6",
                Price = 100000M,
            },

            new Distance
            {
                DistanceID = 57,
                District = "District 5",
                Area = "Ward 7",
                Price = 99500M,
            },

            new Distance
            {
                DistanceID = 58,
                District = "District 5",
                Area = "Ward 8",
                Price = 98000M,
            },

            new Distance
            {
                DistanceID = 59,
                District = "District 5",
                Area = "Ward 9",
                Price = 96000M,
            },

            new Distance
            {
                DistanceID = 60,
                District = "District 5",
                Area = "Ward 10",
                Price = 94000M,
            },

            new Distance
            {
                DistanceID = 61,
                District = "District 5",
                Area = "Ward 11",
                Price = 93000M,
            },

            new Distance
            {
                DistanceID = 62,
                District = "District 5",
                Area = "Ward 12",
                Price = 91500M,
            },

            new Distance
            {
                DistanceID = 63,
                District = "District 5",
                Area = "Ward 13",
                Price = 90500M,
            },

            new Distance
            {
                DistanceID = 64,
                District = "District 5",
                Area = "Ward 14",
                Price = 89000M,
            },

            new Distance
            {
                DistanceID = 65,
                District = "District 5",
                Area = "Ward 15",
                Price = 88500M,
            },

        // District 6: 14 area 
            new Distance
            {
                DistanceID = 66,
                District = "District 6",
                Area = "Ward 1",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 67,
                District = "District 6",
                Area = "Ward 2",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 68,
                District = "District 6",
                Area = "Ward 3",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 69,
                District = "District 6",
                Area = "Ward 4",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 70,
                District = "District 6",
                Area = "Ward 5",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 71,
                District = "District 6",
                Area = "Ward 6",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 72,
                District = "District 6",
                Area = "Ward 7",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 73,
                District = "District 6",
                Area = "Ward 8",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 74,
                District = "District 6",
                Area = "Ward 9",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 75,
                District = "District 6",
                Area = "Ward 10",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 76,
                District = "District 6",
                Area = "Ward 11",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 77,
                District = "District 6",
                Area = "Ward 12",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 78,
                District = "District 6",
                Area = "Ward 13",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 79,
                District = "District 6",
                Area = "Ward 14",
                Price = 95000M
            },

        // District 7: 10 area 
            new Distance
            {
                DistanceID = 80,
                District = "District 7",
                Area = "Binh Thuan Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 81,
                District = "District 7",
                Area = "Phu My Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 82,
                District = "District 7",
                Area = "Phu Thuan Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 83,
                District = "District 7",
                Area = "Tan Hung Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 84,
                District = "District 7",
                Area = "Tan Kieng Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 85,
                District = "District 7",
                Area = "Tan Phong Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 86,
                District = "District 7",
                Area = "Tan Phu Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 87,
                District = "District 7",
                Area = "Tan Quy Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 88,
                District = "District 7",
                Area = "Tan Thuan Dong Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 89,
                District = "District 7",
                Area = "Tan Thuan Tay Ward",
                Price = 95000M
            },

        // District 8: 16 area  
            new Distance
            {
                DistanceID = 90,
                District = "District 8",
                Area = "Ward 1",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 91,
                District = "District 8",
                Area = "Ward 2",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 92,
                District = "District 8",
                Area = "Ward 3",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 93,
                District = "District 8",
                Area = "Ward 4",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 94,
                District = "District 8",
                Area = "Ward 5",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 95,
                District = "District 8",
                Area = "Ward 6",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 96,
                District = "District 8",
                Area = "Ward 7",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 97,
                District = "District 8",
                Area = "Ward 8",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 98,
                District = "District 8",
                Area = "Ward 9",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 99,
                District = "District 8",
                Area = "Ward 10",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 100,
                District = "District 8",
                Area = "Ward 11",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 101,
                District = "District 8",
                Area = "Ward 12",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 102,
                District = "District 8",
                Area = "Ward 13",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 103,
                District = "District 8",
                Area = "Ward 14",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 104,
                District = "District 8",
                Area = "Ward 15",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 105,
                District = "District 8",
                Area = "Ward 16",
                Price = 95000M
            },

        // District 9: 13 area  
            new Distance
            {
                DistanceID = 106,
                District = "District 9",
                Area = "Hiep Phu Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 107,
                District = "District 9",
                Area = "Long Binh Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 108,
                District = "District 9",
                Area = "Long Phuoc Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 109,
                District = "District 9",
                Area = "Long Thanh My Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 110,
                District = "District 9",
                Area = "Long Truong Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 111,
                District = "District 9",
                Area = "Phu Huu Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 112,
                District = "District 9",
                Area = "Phuoc Binh Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 113,
                District = "District 9",
                Area = "Phuoc Long A Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 114,
                District = "District 9",
                Area = "Phuoc Long B Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 115,
                District = "District 9",
                Area = "Tan Phu Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 116,
                District = "District 9",
                Area = "Tang Nhon Phu A Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 117,
                District = "District 9",
                Area = "Tang Nhon Phu B Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 118,
                District = "District 9",
                Area = "Truong Thanh Ward",
                Price = 95000M
            },

        // District 10: 15 area 
            new Distance
            {
                DistanceID = 119,
                District = "District 10",
                Area = "Ward 1",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 120,
                District = "District 10",
                Area = "Ward 2",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 121,
                District = "District 10",
                Area = "Ward 3",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 122,
                District = "District 10",
                Area = "Ward 4",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 123,
                District = "District 10",
                Area = "Ward 5",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 124,
                District = "District 10",
                Area = "Ward 6",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 125,
                District = "District 10",
                Area = "Ward 7",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 126,
                District = "District 10",
                Area = "Ward 8",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 127,
                District = "District 10",
                Area = "Ward 9",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 128,
                District = "District 10",
                Area = "Ward 10",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 129,
                District = "District 10",
                Area = "Ward 11",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 130,
                District = "District 10",
                Area = "Ward 12",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 131,
                District = "District 10",
                Area = "Ward 13",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 132,
                District = "District 10",
                Area = "Ward 14",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 133,
                District = "District 10",
                Area = "Ward 15",
                Price = 95000M
            },

        // District 11: 16 area
            new Distance
            {
                DistanceID = 134,
                District = "District 11",
                Area = "Ward 1",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 135,
                District = "District 11",
                Area = "Ward 2",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 136,
                District = "District 11",
                Area = "Ward 3",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 137,
                District = "District 11",
                Area = "Ward 4",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 138,
                District = "District 11",
                Area = "Ward 5",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 139,
                District = "District 11",
                Area = "Ward 6",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 140,
                District = "District 11",
                Area = "Ward 7",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 141,
                District = "District 11",
                Area = "Ward 8",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 142,
                District = "District 11",
                Area = "Ward 9",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 143,
                District = "District 11",
                Area = "Ward 10",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 144,
                District = "District 11",
                Area = "Ward 11",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 145,
                District = "District 11",
                Area = "Ward 12",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 146,
                District = "District 11",
                Area = "Ward 13",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 147,
                District = "District 11",
                Area = "Ward 14",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 148,
                District = "District 11",
                Area = "Ward 15",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 149,
                District = "District 11",
                Area = "Ward 16",
                Price = 95000M
            },

        // District 12: 11 area
            new Distance
            {
                DistanceID = 150,
                District = "District 12",
                Area = "An Phu Dong Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 151,
                District = "District 12",
                Area = "Dong Hung Thuan Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 152,
                District = "District 12",
                Area = "Hiep Thanh Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 153,
                District = "District 12",
                Area = "Tan Chanh Hiep Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 154,
                District = "District 12",
                Area = "Tan Hung Thuan Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 155,
                District = "District 12",
                Area = "Tan Thoi Hiep Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 156,
                District = "District 12",
                Area = "Tan Thoi Nhat Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 157,
                District = "District 12",
                Area = "Thanh Loc Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 158,
                District = "District 12",
                Area = "Thanh Xuan Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 159,
                District = "District 12",
                Area = "Thoi An Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 160,
                District = "District 12",
                Area = "Trung My Tay Ward",
                Price = 95000M
            },

        // Go Vap District: 16 area
            new Distance
            {
                DistanceID = 161,
                District = "Go Vap District",
                Area = "Ward 1",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 162,
                District = "Go Vap District",
                Area = "Ward 3",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 163,
                District = "Go Vap District",
                Area = "Ward 4",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 164,
                District = "Go Vap District",
                Area = "Ward 5",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 165,
                District = "Go Vap District",
                Area = "Ward 6",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 166,
                District = "Go Vap District",
                Area = "Ward 7",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 167,
                District = "Go Vap District",
                Area = "Ward 8",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 168,
                District = "Go Vap District",
                Area = "Ward 9",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 169,
                District = "Go Vap District",
                Area = "Ward 10",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 170,
                District = "Go Vap District",
                Area = "Ward 11",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 171,
                District = "Go Vap District",
                Area = "Ward 12",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 172,
                District = "Go Vap District",
                Area = "Ward 13",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 173,
                District = "Go Vap District",
                Area = "Ward 14",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 174,
                District = "Go Vap District",
                Area = "Ward 15",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 175,
                District = "Go Vap District",
                Area = "Ward 16",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 176,
                District = "Go Vap District",
                Area = "Ward 17",
                Price = 95000M
            },

        // Tan Binh District: 15 area
            new Distance
            {
                DistanceID = 177,
                District = "Tan Binh District",
                Area = "Ward 1",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 178,
                District = "Tan Binh District",
                Area = "Ward 2",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 179,
                District = "Tan Binh District",
                Area = "Ward 3",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 180,
                District = "Tan Binh District",
                Area = "Ward 4",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 181,
                District = "Tan Binh District",
                Area = "Ward 5",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 182,
                District = "Tan Binh District",
                Area = "Ward 6",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 183,
                District = "Tan Binh District",
                Area = "Ward 7",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 184,
                District = "Tan Binh District",
                Area = "Ward 8",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 185,
                District = "Tan Binh District",
                Area = "Ward 9",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 186,
                District = "Tan Binh District",
                Area = "Ward 10",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 187,
                District = "Tan Binh District",
                Area = "Ward 11",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 188,
                District = "Tan Binh District",
                Area = "Ward 12",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 189,
                District = "Tan Binh District",
                Area = "Ward 13",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 190,
                District = "Tan Binh District",
                Area = "Ward 14",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 191,
                District = "Tan Binh District",
                Area = "Ward 15",
                Price = 95000M
            },

        // Tan Phu District: 11 area
            new Distance
            {
                DistanceID = 192,
                District = "Tan Phu District",
                Area = "Hiep Tan Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 193,
                District = "Tan Phu District",
                Area = "Hoa Thanh Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 194,
                District = "Tan Phu District",
                Area = "Phu Tho Hoa Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 195,
                District = "Tan Phu District",
                Area = "Phu Thanh Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 196,
                District = "Tan Phu District",
                Area = "Phu Trung Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 197,
                District = "Tan Phu District",
                Area = "Tan Quy Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 198,
                District = "Tan Phu District",
                Area = "Tan Thanh Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 199,
                District = "Tan Phu District",
                Area = "Tan Son Nhi Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 200,
                District = "Tan Phu District",
                Area = "Tan Thoi Hoa Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 201,
                District = "Tan Phu District",
                Area = "Tay Thanh Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 202,
                District = "Tan Phu District",
                Area = "Son Ky Ward",
                Price = 95000M
            },

        // District Binh Thanh: 20 area
            new Distance
            {
                DistanceID = 203,
                District = "Binh Thanh District",
                Area = "Ward 1",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 204,
                District = "Binh Thanh District",
                Area = "Ward 2",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 205,
                District = "Binh Thanh District",
                Area = "Ward 3",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 206,
                District = "Binh Thanh District",
                Area = "Ward 5",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 207,
                District = "Binh Thanh District",
                Area = "Ward 6",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 208,
                District = "Binh Thanh District",
                Area = "Ward 7",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 209,
                District = "Binh Thanh District",
                Area = "Ward 11",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 210,
                District = "Binh Thanh District",
                Area = "Ward 12",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 211,
                District = "Binh Thanh District",
                Area = "Ward 13",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 212,
                District = "Binh Thanh District",
                Area = "Ward 14",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 213,
                District = "Binh Thanh District",
                Area = "Ward 15",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 214,
                District = "Binh Thanh District",
                Area = "Ward 17",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 215,
                District = "Binh Thanh District",
                Area = "Ward 19",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 216,
                District = "Binh Thanh District",
                Area = "Ward 21",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 217,
                District = "Binh Thanh District",
                Area = "Ward 22",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 218,
                District = "Binh Thanh District",
                Area = "Ward 24",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 219,
                District = "Binh Thanh District",
                Area = "Ward 25",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 220,
                District = "Binh Thanh District",
                Area = "Ward 26",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 221,
                District = "Binh Thanh District",
                Area = "Ward 27",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 222,
                District = "Binh Thanh District",
                Area = "Ward 28",
                Price = 95000M
            },

        // Phu Nhuan District: 15 area
            new Distance
            {
                DistanceID = 223,
                District = "Phu Nhuan District",
                Area = "Ward 1",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 224,
                District = "Phu Nhuan District",
                Area = "Ward 2",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 225,
                District = "Phu Nhuan District",
                Area = "Ward 3",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 226,
                District = "Phu Nhuan District",
                Area = "Ward 4",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 227,
                District = "Phu Nhuan District",
                Area = "Ward 5",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 228,
                District = "Phu Nhuan District",
                Area = "Ward 7",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 229,
                District = "Phu Nhuan District",
                Area = "Ward 8",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 230,
                District = "Phu Nhuan District",
                Area = "Ward 9",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 231,
                District = "Phu Nhuan District",
                Area = "Ward 10",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 232,
                District = "Phu Nhuan District",
                Area = "Ward 11",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 233,
                District = "Phu Nhuan District",
                Area = "Ward 12",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 234,
                District = "Phu Nhuan District",
                Area = "Ward 13",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 235,
                District = "Phu Nhuan District",
                Area = "Ward 14",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 236,
                District = "Phu Nhuan District",
                Area = "Ward 15",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 237,
                District = "Phu Nhuan District",
                Area = "Ward 17",
                Price = 95000M
            },

        // District Thu Duc: 12 area
            new Distance
            {
                DistanceID = 238,
                District = "Thu Duc District",
                Area = "Binh Chieu Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 239,
                District = "Thu Duc District",
                Area = "Binh Tho Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 240,
                District = "Thu Duc District",
                Area = "Hiep Binh Chanh Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 241,
                District = "Thu Duc District",
                Area = "Hiep Binh Phuoc Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 242,
                District = "Thu Duc District",
                Area = "Linh Chieu Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 243,
                District = "Thu Duc District",
                Area = "Linh Dong Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 244,
                District = "Thu Duc District",
                Area = "Linh Tay Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 245,
                District = "Thu Duc District",
                Area = "Linh Trung Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 246,
                District = "Thu Duc District",
                Area = "Linh Xuan Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 247,
                District = "Thu Duc District",
                Area = "Tam Binh Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 248,
                District = "Thu Duc District",
                Area = "Tam Phu Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 249,
                District = "Thu Duc District",
                Area = "Truong Tho Ward",
                Price = 95000M
            },

        // District Binh Tan: 10 area
            new Distance
            {
                DistanceID = 250,
                District = "Binh Tan District",
                Area = "Binh Hung Hoa Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 251,
                District = "Binh Tan District",
                Area = "Binh Hung Hoa A Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 252,
                District = "Binh Tan District",
                Area = "Binh Hung Hoa B Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 253,
                District = "Binh Tan District",
                Area = "Binh Tri Dong Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 254,
                District = "Binh Tan District",
                Area = "Binh Tri Dong A Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 255,
                District = "Binh Tan District",
                Area = "Binh Tri Dong B Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 256,
                District = "Binh Tan District",
                Area = "Tan Tao Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 257,
                District = "Binh Tan District",
                Area = "Tan Tao A Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 258,
                District = "Binh Tan District",
                Area = "An Lac Ward",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 259,
                District = "Binh Tan District",
                Area = "An Lac A Ward",
                Price = 95000M
            },

            // Cu Chi District: 1 town and 20 Commune
            new Distance
            {
                DistanceID = 260,
                District = "Cu Chi District",
                Area = "Cu Chi Town",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 261,
                District = "Cu Chi District",
                Area = "Phu My Hung Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 262,
                District = "Cu Chi District",
                Area = "An Phu Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 263,
                District = "Cu Chi District",
                Area = "Trung Lap Thuong Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 264,
                District = "Cu Chi District",
                Area = "An Nhon Tay Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 265,
                District = "Cu Chi District",
                Area = "Nhuan Duc Commune",
                Price = 95000M
            },
            new Distance
            {
                DistanceID = 266,
                District = "Cu Chi District",
                Area = "Pham Van Co Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 267,
                District = "Cu Chi District",
                Area = "Phu Hoa Dong Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 268,
                District = "Cu Chi District",
                Area = "Trung Lap Ha Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 269,
                District = "Cu Chi District",
                Area = "Trung An Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 270,
                District = "Cu Chi District",
                Area = "Phuoc Thanh Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 271,
                District = "Cu Chi District",
                Area = "Phuoc Hiep Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 272,
                District = "Cu Chi District",
                Area = "Tan An Hoi Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 273,
                District = "Cu Chi District",
                Area = "Phuoc Vinh An Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 274,
                District = "Cu Chi District",
                Area = "Thai My Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 275,
                District = "Cu Chi District",
                Area = "Tan Thanh Tay Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 276,
                District = "Cu Chi District",
                Area = "Hoa Phu Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 277,
                District = "Cu Chi District",
                Area = "Tan Thanh Dong Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 278,
                District = "Cu Chi District",
                Area = "Binh My Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 279,
                District = "Cu Chi District",
                Area = "Tan Phu Trung Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 280,
                District = "Cu Chi District",
                Area = "Tan Thong Hoi Commune",
                Price = 95000M
            },

            // Hoc Mon District: 1 town and 11 commune
            new Distance
            {
                DistanceID = 281,
                District = "Hoc Mon District",
                Area = "Hoc Mon Town",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 282,
                District = "Hoc Mon District",
                Area = "Ba Diem Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 283,
                District = "Hoc Mon District",
                Area = "Dong Thanh Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 284,
                District = "Hoc Mon District",
                Area = "Nhi Binh Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 285,
                District = "Hoc Mon District",
                Area = "Tan Hiep Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 286,
                District = "Hoc Mon District",
                Area = "Tan Thoi Nhi Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 287,
                District = "Hoc Mon District",
                Area = "Tan Xuan Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 288,
                District = "Hoc Mon District",
                Area = "Thoi Tam Thon Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 289,
                District = "Hoc Mon District",
                Area = "Trung Chanh Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 290,
                District = "Hoc Mon District",
                Area = "Xuan Thoi Dong Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 291,
                District = "Hoc Mon District",
                Area = "Xuan Thoi Son Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 292,
                District = "Hoc Mon District",
                Area = "Xuan Thoi Thuong Commune",
                Price = 95000M
            },
            // Binh Chanh District: 1 town and 16 commune
            new Distance
            {
                DistanceID = 293,
                District = "Binh Chanh District",
                Area = "Tan Tuc Town",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 294,
                District = "Binh Chanh District",
                Area = "Tan Kien Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 295,
                District = "Binh Chanh District",
                Area = "Tan Nhat Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 296,
                District = "Binh Chanh District",
                Area = "An Phu Tay Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 297,
                District = "Binh Chanh District",
                Area = "Tan Quy Tay Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 298,
                District = "Binh Chanh District",
                Area = "Hung Long Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 299,
                District = "Binh Chanh District",
                Area = "Qui Duc Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 300,
                District = "Binh Chanh District",
                Area = "Binh Chanh Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 301,
                District = "Binh Chanh District",
                Area = "Le Minh Xuan Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 302,
                District = "Binh Chanh District",
                Area = "Pham Van Hai Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 303,
                District = "Binh Chanh District",
                Area = "Binh Hung Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 304,
                District = "Binh Chanh District",
                Area = "Binh Loi Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 305,
                District = "Binh Chanh District",
                Area = "Da Phuoc Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 306,
                District = "Binh Chanh District",
                Area = "Phong Phu Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 307,
                District = "Binh Chanh District",
                Area = "Vinh Loc A Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 308,
                District = "Binh Chanh District",
                Area = "Vinh Loc B Commune",
                Price = 95000M
            },

            // Nha Be District: 1 town and 6 commune
            new Distance
            {
                DistanceID = 309,
                District = "Nha Be District",
                Area = "Nha Be Town",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 310,
                District = "Nha Be District",
                Area = "Hiep Phuoc Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 311,
                District = "Nha Be District",
                Area = "Long Thoi Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 312,
                District = "Nha Be District",
                Area = "Nhon Duc Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 313,
                District = "Nha Be District",
                Area = "Phu Xuan Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 314,
                District = "Nha Be District",
                Area = "Phuoc Kien Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 315,
                District = "Nha Be District",
                Area = "Phuoc Loc Commune",
                Price = 95000M
            },

            // Can Gio District: 1 town and 6 commune
            new Distance
            {
                DistanceID = 316,
                District = "Can Gio District",
                Area = "Can Thanh Town",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 317,
                District = "Can Gio District",
                Area = "Binh Khanh Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 318,
                District = "Can Gio District",
                Area = "An Thoi Dong Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 319,
                District = "Can Gio District",
                Area = "Tam Thon Hiep Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 320,
                District = "Can Gio District",
                Area = "Thanh An Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 321,
                District = "Can Gio District",
                Area = "Ly Nhon Commune",
                Price = 95000M
            },

            new Distance
            {
                DistanceID = 322,
                District = "Can Gio District",
                Area = "Long Hoa Commune",
                Price = 95000M
            }
        );

    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
