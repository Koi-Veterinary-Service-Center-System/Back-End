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

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Distance> Distances { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<KoiOrPool> KoiOrPools { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PrescriptionRecord> PrescriptionRecords { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Slot> Slots { get; set; }

    public virtual DbSet<Veterinarian> Vets { get; set; }

    public virtual DbSet<Staff> Staffs { get; set; }

    public virtual DbSet<VetSlot> VetSlots { get; set; }

    public virtual DbSet<Veterinarian> Veterinarians { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //add role
        List<IdentityRole> roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Name = "Customer",
                NormalizedName = "CUSTOMER"
            },
            new IdentityRole
            {
                Name = "Vet",
                NormalizedName = "VET"
            },
            new IdentityRole
            {
                Name = "Staff",
                NormalizedName = "STAFF"
            },
            new IdentityRole
            {
                Name = "Manager",
                NormalizedName = "MANAGER"
            },
        };
        modelBuilder.Entity<IdentityRole>().HasData(roles);

        //Define keys
        modelBuilder.Entity<Veterinarian>()
                .ToTable("Veterinarians")
                .HasBaseType<User>();

        modelBuilder.Entity<Customer>()
            .ToTable("Customers")
            .HasBaseType<User>();

        modelBuilder.Entity<Staff>()
            .ToTable("Staffs")
            .HasBaseType<User>();

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Veterinarian)
            .WithMany(v => v.Bookings)
            .HasForeignKey(b => b.VetID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<VetSlot>()
        .HasKey(vs => new { vs.VetID, vs.SlotID });

        modelBuilder.Entity<VetSlot>()
          .HasOne(vs => vs.Veterinarian)
          .WithMany(v => v.VetSlots)
          .HasForeignKey(vs => vs.VetID);

        modelBuilder.Entity<VetSlot>()
          .HasOne(vs => vs.Slot)
          .WithMany(s => s.VetSlots)
          .HasForeignKey(vs => vs.SlotID);

        //SEED DATA=================

        // Seed data for Veterinarian
        modelBuilder.Entity<Veterinarian>().HasData(
            new Veterinarian
            {
                Id = "v1", // Primary key inherited from IdentityUser (User)
                FirstName = "John",
                LastName = "Doe",
                UserName = "johndoe",
                NormalizedUserName = "JOHNDOE",
                Gender = true,
                ExperienceYears = 10,
                Address = "123 Vet St.",
                ImageURL = "https://example.com/vet1.jpg",
                ImagePublicId = "vet1_image_id"
            },
            new Veterinarian
            {
                Id = "v2",
                FirstName = "Jane",
                LastName = "Smith",
                UserName = "janesmith",
                NormalizedUserName = "JANESMITH",
                Gender = false,
                ExperienceYears = 8,
                Address = "456 Vet St.",
                ImageURL = "https://example.com/vet2.jpg",
                ImagePublicId = "vet2_image_id"
            },
            new Veterinarian
            {
                Id = "v3",
                FirstName = "vet3",
                LastName = "Smith",
                UserName = "vet3",
                NormalizedUserName = "VET3",
                Gender = false,
                ExperienceYears = 8,
                Address = "456 Vet St.",
                ImageURL = "https://example.com/vet2.jpg",
                ImagePublicId = "vet2_image_id"
            },
            new Veterinarian
            {
                Id = "v4",
                FirstName = "Vet 4",
                LastName = "Smith",
                UserName = "veterianary",
                NormalizedUserName = "VETERIANARY",
                Gender = false,
                ExperienceYears = 8,
                Address = "456 Vet St.",
                ImageURL = "https://example.com/vet2.jpg",
                ImagePublicId = "vet2_image_id"
            },
            new Veterinarian
            {
                Id = "v5",
                FirstName = "Vet 5",
                LastName = "Smith",
                UserName = "vet1000000",
                NormalizedUserName = "VET1000000",
                Gender = false,
                ExperienceYears = 8,
                Address = "456 Vet St.",
                ImageURL = "https://example.com/vet2.jpg",
                ImagePublicId = "vet2_image_id"
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
                VetID = "v4",
                SlotID = 1,
                isBooked = false
            },
            new VetSlot
            {
                VetID = "v5",
                SlotID = 5,
                isBooked = false
            },
            new VetSlot
            {
                VetID = "v4",
                SlotID = 3,
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
                Price = 150.00m,
                EstimatedDuration = 1.5,
                ImageURL = "https://example.com/service1.jpg"
            },
            new Service
            {
                ServiceID = 2,
                ServiceName = "Pool Maintenance",
                Description = "Comprehensive pool maintenance service.",
                Price = 250.00m,
                EstimatedDuration = 2.0,
                ImageURL = "https://example.com/service2.jpg"
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
                Qrcode = "qrcode2",
                Type = "VNPay"
            }
        );

        // Seed data for KoiOrPool
        modelBuilder.Entity<KoiOrPool>().HasData(
            new KoiOrPool
            {
                KoiOrPoolID = 1,
                Name = "John's Koi Pond",
                IsPool = false,
                Description = "A beautiful koi pond owned by John.",
                CustomerID = "c1" // Assuming customer with ID 'c1' exists
            },
            new KoiOrPool
            {
                KoiOrPoolID = 2,
                Name = "Smith's Pool",
                IsPool = true,
                Description = "A large swimming pool owned by the Smith family.",
                CustomerID = "c2" // Assuming customer with ID 'c2' exists
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
                Price = 92.500M,
            },

            new Distance
            {
                DistanceID = 2,
                District = "District 1",
                Area = "Ben Thanh Ward",
                Price = 98.500M,
            },

            new Distance
            {
                DistanceID = 3,
                District = "District 1",
                Area = "Co Giang Ward",
                Price = 97.000M,
            },

            new Distance
            {
                DistanceID = 4,
                District = "District 1",
                Area = "Cau Kho Ward",
                Price = 99.000M,
            },

            new Distance
            {
                DistanceID = 5,
                District = "District 1",
                Area = "Cau Ong Lanh Ward",
                Price = 95.500M,
            },

            new Distance
            {
                DistanceID = 6,
                District = "District 1",
                Area = "Da Kao Ward",
                Price = 93.000M,
            },

            new Distance
            {
                DistanceID = 7,
                District = "District 1",
                Area = "Nguyen Thai Binh Ward",
                Price = 94.000M,
            },

            new Distance
            {
                DistanceID = 8,
                District = "District 1",
                Area = "Nguyen Cu Trinh Ward",
                Price = 101.500M,
            },

            new Distance
            {
                DistanceID = 9,
                District = "District 1",
                Area = "Pham Ngu Lao Ward",
                Price = 101.000M,
            },

            new Distance
            {
                DistanceID = 10,
                District = "District 1",
                Area = "Tan Dinh Ward",
                Price = 105.000M,
            },

        // District 2: 11 area
            new Distance
            {
                DistanceID = 11,
                District = "District 2",
                Area = "An Khanh Ward",
                Price = 70.500M,
            },

            new Distance
            {
                DistanceID = 12,
                District = "District 2",
                Area = "An Loi Dong Ward",
                Price = 84.000M,
            },

            new Distance
            {
                DistanceID = 13,
                District = "District 2",
                Area = "An Phu Ward",
                Price = 69.500M,
            },

            new Distance
            {
                DistanceID = 14,
                District = "District 2",
                Area = "Binh An Ward",
                Price = 74.000M,
            },

            new Distance
            {
                DistanceID = 15,
                District = "District 2",
                Area = "Binh Khanh Ward",
                Price = 70.000M,
            },

            new Distance
            {
                DistanceID = 16,
                District = "District 2",
                Area = "Binh Trung Dong Ward",
                Price = 54.000M,
            },

            new Distance
            {
                DistanceID = 17,
                District = "District 2",
                Area = "Binh Trung Tay Ward",
                Price = 64.000M,
            },

            new Distance
            {
                DistanceID = 18,
                District = "District 2",
                Area = "Cat Lai Ward",
                Price = 62.000M,
            },

            new Distance
            {
                DistanceID = 19,
                District = "District 2",
                Area = "Thanh My Loi Ward",
                Price = 65.000M,
            },

            new Distance
            {
                DistanceID = 20,
                District = "District 2",
                Area = "Thao Dien Ward",
                Price = 80.500M,
            },

            new Distance
            {
                DistanceID = 21,
                District = "District 2",
                Area = "Thu Thiem Ward",
                Price = 82.000M,
            },

        // District 3: 14 area
            new Distance
            {
                DistanceID = 22,
                District = "District 2",
                Area = "Ward 1",
                Price = 112.500M,
            },

            new Distance
            {
                DistanceID = 23,
                District = "District 2",
                Area = "Ward 2",
                Price = 110.000M,
            },

            new Distance
            {
                DistanceID = 24,
                District = "District 2",
                Area = "Ward 3",
                Price = 108.000M,
            },

            new Distance
            {
                DistanceID = 25,
                District = "District 2",
                Area = "Ward 4",
                Price = 105.500M,
            },

            new Distance
            {
                DistanceID = 26,
                District = "District 2",
                Area = "Ward 5",
                Price = 104.000M,
            },

            new Distance
            {
                DistanceID = 27,
                District = "District 2",
                Area = "Ward 6",
                Price = 107.500M,
            },

            new Distance
            {
                DistanceID = 28,
                District = "District 2",
                Area = "Ward 7",
                Price = 108.000M,
            },

            new Distance
            {
                DistanceID = 29,
                District = "District 2",
                Area = "Ward 8",
                Price = 110.500M,
            },

            new Distance
            {
                DistanceID = 30,
                District = "District 2",
                Area = "Ward 9",
                Price = 113.000M,
            },

            new Distance
            {
                DistanceID = 31,
                District = "District 2",
                Area = "Ward 10",
                Price = 115.500M,
            },

            new Distance
            {
                DistanceID = 32,
                District = "District 2",
                Area = "Ward 11",
                Price = 124.500M,
            },

            new Distance
            {
                DistanceID = 33,
                District = "District 2",
                Area = "Ward 12",
                Price = 116.000M,
            },

            new Distance
            {
                DistanceID = 34,
                District = "District 2",
                Area = "Ward 13",
                Price = 114.500M,
            },

            new Distance
            {
                DistanceID = 35,
                District = "District 2",
                Area = "Ward 14",
                Price = 113.000M,
            },

        // District 4: 15 area
            new Distance
            {
                DistanceID = 36,
                District = "District 4",
                Area = "Ward 1",
                Price = 114.000M,
            },

            new Distance
            {
                DistanceID = 37,
                District = "District 4",
                Area = "Ward 2",
                Price = 113.000M,
            },

            new Distance
            {
                DistanceID = 38,
                District = "District 4",
                Area = "Ward 3",
                Price = 115.500M,
            },

            new Distance
            {
                DistanceID = 39,
                District = "District 4",
                Area = "Ward 4",
                Price = 111.500M,
            },

            new Distance
            {
                DistanceID = 40,
                District = "District 4",
                Area = "Ward 5",
                Price = 117.500M,
            },

            new Distance
            {
                DistanceID = 41,
                District = "District 4",
                Area = "Ward 6",
                Price = 113.500M,
            },

            new Distance
            {
                DistanceID = 42,
                District = "District 4",
                Area = "Ward 8",
                Price = 115.000M,
            },

            new Distance
            {
                DistanceID = 43,
                District = "District 4",
                Area = "Ward 9",
                Price = 119.000M,
            },

            new Distance
            {
                DistanceID = 44,
                District = "District 4",
                Area = "Ward 10",
                Price = 117.000M,
            },

            new Distance
            {
                DistanceID = 45,
                District = "District 4",
                Area = "Ward 12",
                Price = 114.500M,
            },

            new Distance
            {
                DistanceID = 46,
                District = "District 4",
                Area = "Ward 13",
                Price = 120.500M,
            },

            new Distance
            {
                DistanceID = 47,
                District = "District 4",
                Area = "Ward 14",
                Price = 118.500M,
            },

            new Distance
            {
                DistanceID = 48,
                District = "District 4",
                Area = "Ward 15",
                Price = 112.000M,
            },

            new Distance
            {
                DistanceID = 49,
                District = "District 4",
                Area = "Ward 16",
                Price = 108.500M,
            },

            new Distance
            {
                DistanceID = 50,
                District = "District 4",
                Area = "Ward 18",
                Price = 102.500M,
            },

        // District 5: 15 area 
            new Distance
            {
                DistanceID = 51,
                District = "District 5",
                Area = "Ward 1",
                Price = 107.500M
            },

            new Distance
            {
                DistanceID = 52,
                District = "District 5",
                Area = "Ward 2",
                Price = 105.500M
            },

            new Distance
            {
                DistanceID = 53,
                District = "District 5",
                Area = "Ward 3",
                Price = 106.500M,
            },

            new Distance
            {
                DistanceID = 54,
                District = "District 5",
                Area = "Ward 4",
                Price = 104.000M,
            },

            new Distance
            {
                DistanceID = 55,
                District = "District 5",
                Area = "Ward 5",
                Price = 103.000M,
            },

            new Distance
            {
                DistanceID = 56,
                District = "District 5",
                Area = "Ward 6",
                Price = 100.000M,
            },

            new Distance
            {
                DistanceID = 57,
                District = "District 5",
                Area = "Ward 7",
                Price = 99.500M,
            },

            new Distance
            {
                DistanceID = 58,
                District = "District 5",
                Area = "Ward 8",
                Price = 98.000M,
            },

            new Distance
            {
                DistanceID = 59,
                District = "District 5",
                Area = "Ward 9",
                Price = 96.000M,
            },

            new Distance
            {
                DistanceID = 60,
                District = "District 5",
                Area = "Ward 10",
                Price = 94.000M,
            },

            new Distance
            {
                DistanceID = 61,
                District = "District 5",
                Area = "Ward 11",
                Price = 93.000M,
            },

            new Distance
            {
                DistanceID = 62,
                District = "District 5",
                Area = "Ward 12",
                Price = 91.500M,
            },

            new Distance
            {
                DistanceID = 63,
                District = "District 5",
                Area = "Ward 13",
                Price = 90.500M,
            },

            new Distance
            {
                DistanceID = 64,
                District = "District 5",
                Area = "Ward 14",
                Price = 89.000M,
            },

            new Distance
            {
                DistanceID = 65,
                District = "District 5",
                Area = "Ward 15",
                Price = 88.500M,
            },

        // District 6: 14 area 
            new Distance
            {
                DistanceID = 66,
                District = "District 6",
                Area = "Ward 1",
                Price = 1250000m
            },

            new Distance
            {
                DistanceID = 67,
                District = "District 6",
                Area = "Ward 2",
                Price = 1300000m
            },

            new Distance
            {
                DistanceID = 68,
                District = "District 6",
                Area = "Ward 3",
                Price = 1350000m
            },

            new Distance
            {
                DistanceID = 69,
                District = "District 6",
                Area = "Ward 4",
                Price = 1400000m
            },

            new Distance
            {
                DistanceID = 70,
                District = "District 6",
                Area = "Ward 5",
                Price = 1450000m
            },

            new Distance
            {
                DistanceID = 71,
                District = "District 6",
                Area = "Ward 6",
                Price = 1500000m
            },

            new Distance
            {
                DistanceID = 72,
                District = "District 6",
                Area = "Ward 7",
                Price = 1550000m
            },

            new Distance
            {
                DistanceID = 73,
                District = "District 6",
                Area = "Ward 8",
                Price = 1600000m
            },

            new Distance
            {
                DistanceID = 74,
                District = "District 6",
                Area = "Ward 9",
                Price = 1650000m
            },

            new Distance
            {
                DistanceID = 75,
                District = "District 6",
                Area = "Ward 10",
                Price = 1700000m
            },

            new Distance
            {
                DistanceID = 76,
                District = "District 6",
                Area = "Ward 11",
                Price = 1750000m
            },

            new Distance
            {
                DistanceID = 77,
                District = "District 6",
                Area = "Ward 12",
                Price = 1800000m
            },

            new Distance
            {
                DistanceID = 78,
                District = "District 6",
                Area = "Ward 13",
                Price = 1850000m
            },

            new Distance
            {
                DistanceID = 79,
                District = "District 6",
                Area = "Ward 14",
                Price = 1900000m
            },

        // District 7: 10 area 
            new Distance
            {
                DistanceID = 80,
                District = "District 7",
                Area = "Binh Thuan Ward",
                Price = 2000000m
            },

            new Distance
            {
                DistanceID = 81,
                District = "District 7",
                Area = "Phu My Ward",
                Price = 2050000m
            },

            new Distance
            {
                DistanceID = 82,
                District = "District 7",
                Area = "Phu Thuan Ward",
                Price = 2100000m
            },

            new Distance
            {
                DistanceID = 83,
                District = "District 7",
                Area = "Tan Hung Ward",
                Price = 2150000m
            },

            new Distance
            {
                DistanceID = 84,
                District = "District 7",
                Area = "Tan Kieng Ward",
                Price = 2200000m
            },

            new Distance
            {
                DistanceID = 85,
                District = "District 7",
                Area = "Tan Phong Ward",
                Price = 2250000m
            },

            new Distance
            {
                DistanceID = 86,
                District = "District 7",
                Area = "Tan Phu Ward",
                Price = 2300000m
            },

            new Distance
            {
                DistanceID = 87,
                District = "District 7",
                Area = "Tan Quy Ward",
                Price = 2350000m
            },

            new Distance
            {
                DistanceID = 88,
                District = "District 7",
                Area = "Tan Thuan Dong Ward",
                Price = 2400000m
            },

            new Distance
            {
                DistanceID = 89,
                District = "District 7",
                Area = "Tan Thuan Tay Ward",
                Price = 2450000m
            },

        // District 8: 16 area  
            new Distance
            {
                DistanceID = 90,
                District = "District 8",
                Area = "Ward 1",
                Price = 2000000m
            },

            new Distance
            {
                DistanceID = 91,
                District = "District 8",
                Area = "Ward 2",
                Price = 2050000m
            },

            new Distance
            {
                DistanceID = 92,
                District = "District 8",
                Area = "Ward 3",
                Price = 2100000m
            },

            new Distance
            {
                DistanceID = 93,
                District = "District 8",
                Area = "Ward 4",
                Price = 2150000m
            },

            new Distance
            {
                DistanceID = 94,
                District = "District 8",
                Area = "Ward 5",
                Price = 2200000m
            },

            new Distance
            {
                DistanceID = 95,
                District = "District 8",
                Area = "Ward 6",
                Price = 2250000m
            },

            new Distance
            {
                DistanceID = 96,
                District = "District 8",
                Area = "Ward 7",
                Price = 2300000m
            },

            new Distance
            {
                DistanceID = 97,
                District = "District 8",
                Area = "Ward 8",
                Price = 2350000m
            },

            new Distance
            {
                DistanceID = 98,
                District = "District 8",
                Area = "Ward 9",
                Price = 2400000m
            },

            new Distance
            {
                DistanceID = 99,
                District = "District 8",
                Area = "Ward 10",
                Price = 2450000m
            },

            new Distance
            {
                DistanceID = 100,
                District = "District 8",
                Area = "Ward 11",
                Price = 2500000m
            },

            new Distance
            {
                DistanceID = 101,
                District = "District 8",
                Area = "Ward 12",
                Price = 2550000m
            },

            new Distance
            {
                DistanceID = 102,
                District = "District 8",
                Area = "Ward 13",
                Price = 2600000m
            },

            new Distance
            {
                DistanceID = 103,
                District = "District 8",
                Area = "Ward 14",
                Price = 2650000m
            },

            new Distance
            {
                DistanceID = 104,
                District = "District 8",
                Area = "Ward 15",
                Price = 2700000m
            },

            new Distance
            {
                DistanceID = 105,
                District = "District 8",
                Area = "Ward 16",
                Price = 2750000m
            },

        // District 9: 13 area  
            new Distance
            {
                DistanceID = 106,
                District = "District 9",
                Area = "Hiep Phu Ward",
                Price = 2000000m
            },

            new Distance
            {
                DistanceID = 107,
                District = "District 9",
                Area = "Long Binh Ward",
                Price = 2050000m
            },

            new Distance
            {
                DistanceID = 108,
                District = "District 9",
                Area = "Long Phuoc Ward",
                Price = 2100000m
            },

            new Distance
            {
                DistanceID = 109,
                District = "District 9",
                Area = "Long Thanh My Ward",
                Price = 2150000m
            },

            new Distance
            {
                DistanceID = 110,
                District = "District 9",
                Area = "Long Truong Ward",
                Price = 2200000m
            },

            new Distance
            {
                DistanceID = 111,
                District = "District 9",
                Area = "Phu Huu Ward",
                Price = 2250000m
            },

            new Distance
            {
                DistanceID = 112,
                District = "District 9",
                Area = "Phuoc Binh Ward",
                Price = 2300000m
            },

            new Distance
            {
                DistanceID = 113,
                District = "District 9",
                Area = "Phuoc Long A Ward",
                Price = 2350000m
            },

            new Distance
            {
                DistanceID = 114,
                District = "District 9",
                Area = "Phuoc Long B Ward",
                Price = 2400000m
            },

            new Distance
            {
                DistanceID = 115,
                District = "District 9",
                Area = "Tan Phu Ward",
                Price = 2450000m
            },

            new Distance
            {
                DistanceID = 116,
                District = "District 9",
                Area = "Tang Nhon Phu A Ward",
                Price = 2500000m
            },

            new Distance
            {
                DistanceID = 117,
                District = "District 9",
                Area = "Tang Nhon Phu B Ward",
                Price = 2550000m
            },

            new Distance
            {
                DistanceID = 118,
                District = "District 9",
                Area = "Truong Thanh Ward",
                Price = 2600000m
            },

        // District 10: 15 area 
            new Distance
            {
                DistanceID = 119,
                District = "District 10",
                Area = "Ward 1",
                Price = 2000000m
            },

            new Distance
            {
                DistanceID = 120,
                District = "District 10",
                Area = "Ward 2",
                Price = 2050000m
            },

            new Distance
            {
                DistanceID = 121,
                District = "District 10",
                Area = "Ward 3",
                Price = 2100000m
            },

            new Distance
            {
                DistanceID = 122,
                District = "District 10",
                Area = "Ward 4",
                Price = 2150000m
            },

            new Distance
            {
                DistanceID = 123,
                District = "District 10",
                Area = "Ward 5",
                Price = 2200000m
            },

            new Distance
            {
                DistanceID = 124,
                District = "District 10",
                Area = "Ward 6",
                Price = 2250000m
            },

            new Distance
            {
                DistanceID = 125,
                District = "District 10",
                Area = "Ward 7",
                Price = 2300000m
            },

            new Distance
            {
                DistanceID = 126,
                District = "District 10",
                Area = "Ward 8",
                Price = 2350000m
            },

            new Distance
            {
                DistanceID = 127,
                District = "District 10",
                Area = "Ward 9",
                Price = 2400000m
            },

            new Distance
            {
                DistanceID = 128,
                District = "District 10",
                Area = "Ward 10",
                Price = 2450000m
            },

            new Distance
            {
                DistanceID = 129,
                District = "District 10",
                Area = "Ward 11",
                Price = 2500000m
            },

            new Distance
            {
                DistanceID = 130,
                District = "District 10",
                Area = "Ward 12",
                Price = 2550000m
            },

            new Distance
            {
                DistanceID = 131,
                District = "District 10",
                Area = "Ward 13",
                Price = 2600000m
            },

            new Distance
            {
                DistanceID = 132,
                District = "District 10",
                Area = "Ward 14",
                Price = 2650000m
            },

            new Distance
            {
                DistanceID = 133,
                District = "District 10",
                Area = "Ward 15",
                Price = 2700000m
            },

        // District 11: 16 area
            new Distance
            {
                DistanceID = 134,
                District = "District 11",
                Area = "Ward 1",
                Price = 2000000m
            },

            new Distance
            {
                DistanceID = 135,
                District = "District 11",
                Area = "Ward 2",
                Price = 2050000m
            },

            new Distance
            {
                DistanceID = 136,
                District = "District 11",
                Area = "Ward 3",
                Price = 2100000m
            },

            new Distance
            {
                DistanceID = 137,
                District = "District 11",
                Area = "Ward 4",
                Price = 2150000m
            },

            new Distance
            {
                DistanceID = 138,
                District = "District 11",
                Area = "Ward 5",
                Price = 2200000m
            },

            new Distance
            {
                DistanceID = 139,
                District = "District 11",
                Area = "Ward 6",
                Price = 2250000m
            },

            new Distance
            {
                DistanceID = 140,
                District = "District 11",
                Area = "Ward 7",
                Price = 2300000m
            },

            new Distance
            {
                DistanceID = 141,
                District = "District 11",
                Area = "Ward 8",
                Price = 2350000m
            },

            new Distance
            {
                DistanceID = 142,
                District = "District 11",
                Area = "Ward 9",
                Price = 2400000m
            },

            new Distance
            {
                DistanceID = 143,
                District = "District 11",
                Area = "Ward 10",
                Price = 2450000m
            },

            new Distance
            {
                DistanceID = 144,
                District = "District 11",
                Area = "Ward 11",
                Price = 2500000m
            },

            new Distance
            {
                DistanceID = 145,
                District = "District 11",
                Area = "Ward 12",
                Price = 2550000m
            },

            new Distance
            {
                DistanceID = 146,
                District = "District 11",
                Area = "Ward 13",
                Price = 2600000m
            },

            new Distance
            {
                DistanceID = 147,
                District = "District 11",
                Area = "Ward 14",
                Price = 2650000m
            },

            new Distance
            {
                DistanceID = 148,
                District = "District 11",
                Area = "Ward 15",
                Price = 2700000m
            },

            new Distance
            {
                DistanceID = 149,
                District = "District 11",
                Area = "Ward 16",
                Price = 2750000m
            },

        // District 12: 11 area
            new Distance
            {
                DistanceID = 150,
                District = "District 12",
                Area = "An Phu Dong Ward",
                Price = 2000000m
            },

            new Distance
            {
                DistanceID = 151,
                District = "District 12",
                Area = "Dong Hung Thuan Ward",
                Price = 2050000m
            },

            new Distance
            {
                DistanceID = 152,
                District = "District 12",
                Area = "Hiep Thanh Ward",
                Price = 2100000m
            },

            new Distance
            {
                DistanceID = 153,
                District = "District 12",
                Area = "Tan Chanh Hiep Ward",
                Price = 2150000m
            },

            new Distance
            {
                DistanceID = 154,
                District = "District 12",
                Area = "Tan Hung Thuan Ward",
                Price = 2200000m
            },

            new Distance
            {
                DistanceID = 155,
                District = "District 12",
                Area = "Tan Thoi Hiep Ward",
                Price = 2250000m
            },

            new Distance
            {
                DistanceID = 156,
                District = "District 12",
                Area = "Tan Thoi Nhat Ward",
                Price = 2300000m
            },

            new Distance
            {
                DistanceID = 157,
                District = "District 12",
                Area = "Thanh Loc Ward",
                Price = 2350000m
            },

            new Distance
            {
                DistanceID = 158,
                District = "District 12",
                Area = "Thanh Xuan Ward",
                Price = 2400000m
            },

            new Distance
            {
                DistanceID = 159,
                District = "District 12",
                Area = "Thoi An Ward",
                Price = 2450000m
            },

            new Distance
            {
                DistanceID = 160,
                District = "District 12",
                Area = "Trung My Tay Ward",
                Price = 2500000m
            },

        // Go Vap District: 16 area
            new Distance
            {
                DistanceID = 161,
                District = "Go Vap District",
                Area = "Ward 1",
                Price = 2000000m
            },

            new Distance
            {
                DistanceID = 162,
                District = "Go Vap District",
                Area = "Ward 3",
                Price = 2050000m
            },

            new Distance
            {
                DistanceID = 163,
                District = "Go Vap District",
                Area = "Ward 4",
                Price = 2100000m
            },

            new Distance
            {
                DistanceID = 164,
                District = "Go Vap District",
                Area = "Ward 5",
                Price = 2150000m
            },

            new Distance
            {
                DistanceID = 165,
                District = "Go Vap District",
                Area = "Ward 6",
                Price = 2200000m
            },

            new Distance
            {
                DistanceID = 166,
                District = "Go Vap District",
                Area = "Ward 7",
                Price = 2250000m
            },

            new Distance
            {
                DistanceID = 167,
                District = "Go Vap District",
                Area = "Ward 8",
                Price = 2300000m
            },

            new Distance
            {
                DistanceID = 168,
                District = "Go Vap District",
                Area = "Ward 9",
                Price = 2350000m
            },

            new Distance
            {
                DistanceID = 169,
                District = "Go Vap District",
                Area = "Ward 10",
                Price = 2400000m
            },

            new Distance
            {
                DistanceID = 170,
                District = "Go Vap District",
                Area = "Ward 11",
                Price = 2450000m
            },

            new Distance
            {
                DistanceID = 171,
                District = "Go Vap District",
                Area = "Ward 12",
                Price = 2500000m
            },

            new Distance
            {
                DistanceID = 172,
                District = "Go Vap District",
                Area = "Ward 13",
                Price = 2550000m
            },

            new Distance
            {
                DistanceID = 173,
                District = "Go Vap District",
                Area = "Ward 14",
                Price = 2600000m
            },

            new Distance
            {
                DistanceID = 174,
                District = "Go Vap District",
                Area = "Ward 15",
                Price = 2650000m
            },

            new Distance
            {
                DistanceID = 175,
                District = "Go Vap District",
                Area = "Ward 16",
                Price = 2700000m
            },

            new Distance
            {
                DistanceID = 176,
                District = "Go Vap District",
                Area = "Ward 17",
                Price = 2750000m
            },

        // Tan Binh District: 15 area
            new Distance
            {
                DistanceID = 177,
                District = "Tan Binh District",
                Area = "Ward 1",
                Price = 2000000m
            },

            new Distance
            {
                DistanceID = 178,
                District = "Tan Binh District",
                Area = "Ward 2",
                Price = 2050000m
            },

            new Distance
            {
                DistanceID = 179,
                District = "Tan Binh District",
                Area = "Ward 3",
                Price = 2100000m
            },

            new Distance
            {
                DistanceID = 180,
                District = "Tan Binh District",
                Area = "Ward 4",
                Price = 2150000m
            },

            new Distance
            {
                DistanceID = 181,
                District = "Tan Binh District",
                Area = "Ward 5",
                Price = 2200000m
            },

            new Distance
            {
                DistanceID = 182,
                District = "Tan Binh District",
                Area = "Ward 6",
                Price = 2250000m
            },

            new Distance
            {
                DistanceID = 183,
                District = "Tan Binh District",
                Area = "Ward 7",
                Price = 2300000m
            },

            new Distance
            {
                DistanceID = 184,
                District = "Tan Binh District",
                Area = "Ward 8",
                Price = 2350000m
            },

            new Distance
            {
                DistanceID = 185,
                District = "Tan Binh District",
                Area = "Ward 9",
                Price = 2400000m
            },

            new Distance
            {
                DistanceID = 186,
                District = "Tan Binh District",
                Area = "Ward 10",
                Price = 2450000m
            },

            new Distance
            {
                DistanceID = 187,
                District = "Tan Binh District",
                Area = "Ward 11",
                Price = 2500000m
            },

            new Distance
            {
                DistanceID = 188,
                District = "Tan Binh District",
                Area = "Ward 12",
                Price = 2550000m
            },

            new Distance
            {
                DistanceID = 189,
                District = "Tan Binh District",
                Area = "Ward 13",
                Price = 2600000m
            },

            new Distance
            {
                DistanceID = 190,
                District = "Tan Binh District",
                Area = "Ward 14",
                Price = 2650000m
            },

            new Distance
            {
                DistanceID = 191,
                District = "Tan Binh District",
                Area = "Ward 15",
                Price = 2700000m
            },

        // Tan Phu District: 11 area
            new Distance
            {
                DistanceID = 192,
                District = "Tan Phu District",
                Area = "Hiep Tan Ward",
                Price = 2000000m
            },

            new Distance
            {
                DistanceID = 193,
                District = "Tan Phu District",
                Area = "Hoa Thanh Ward",
                Price = 2050000m
            },

            new Distance
            {
                DistanceID = 194,
                District = "Tan Phu District",
                Area = "Phu Tho Hoa Ward",
                Price = 2100000m
            },

            new Distance
            {
                DistanceID = 195,
                District = "Tan Phu District",
                Area = "Phu Thanh Ward",
                Price = 2150000m
            },

            new Distance
            {
                DistanceID = 196,
                District = "Tan Phu District",
                Area = "Phu Trung Ward",
                Price = 2200000m
            },

            new Distance
            {
                DistanceID = 197,
                District = "Tan Phu District",
                Area = "Tan Quy Ward",
                Price = 2250000m
            },

            new Distance
            {
                DistanceID = 198,
                District = "Tan Phu District",
                Area = "Tan Thanh Ward",
                Price = 2300000m
            },

            new Distance
            {
                DistanceID = 199,
                District = "Tan Phu District",
                Area = "Tan Son Nhi Ward",
                Price = 2350000m
            },

            new Distance
            {
                DistanceID = 200,
                District = "Tan Phu District",
                Area = "Tan Thoi Hoa Ward",
                Price = 2400000m
            },

            new Distance
            {
                DistanceID = 201,
                District = "Tan Phu District",
                Area = "Tay Thanh Ward",
                Price = 2450000m
            },

            new Distance
            {
                DistanceID = 202,
                District = "Tan Phu District",
                Area = "Son Ky Ward",
                Price = 2500000m
            },

        // District Binh Thanh: 20 area
            new Distance
            {
                DistanceID = 203,
                District = "Binh Thanh District",
                Area = "Ward 1",
                Price = 2000000m
            },

            new Distance
            {
                DistanceID = 204,
                District = "Binh Thanh District",
                Area = "Ward 2",
                Price = 2050000m
            },

            new Distance
            {
                DistanceID = 205,
                District = "Binh Thanh District",
                Area = "Ward 3",
                Price = 2100000m
            },

            new Distance
            {
                DistanceID = 206,
                District = "Binh Thanh District",
                Area = "Ward 5",
                Price = 2150000m
            },

            new Distance
            {
                DistanceID = 207,
                District = "Binh Thanh District",
                Area = "Ward 6",
                Price = 2200000m
            },

            new Distance
            {
                DistanceID = 208,
                District = "Binh Thanh District",
                Area = "Ward 7",
                Price = 2250000m
            },

            new Distance
            {
                DistanceID = 209,
                District = "Binh Thanh District",
                Area = "Ward 11",
                Price = 2300000m
            },

            new Distance
            {
                DistanceID = 210,
                District = "Binh Thanh District",
                Area = "Ward 12",
                Price = 2350000m
            },

            new Distance
            {
                DistanceID = 211,
                District = "Binh Thanh District",
                Area = "Ward 13",
                Price = 2400000m
            },

            new Distance
            {
                DistanceID = 212,
                District = "Binh Thanh District",
                Area = "Ward 14",
                Price = 2450000m
            },

            new Distance
            {
                DistanceID = 213,
                District = "Binh Thanh District",
                Area = "Ward 15",
                Price = 2500000m
            },

            new Distance
            {
                DistanceID = 214,
                District = "Binh Thanh District",
                Area = "Ward 17",
                Price = 2550000m
            },

            new Distance
            {
                DistanceID = 215,
                District = "Binh Thanh District",
                Area = "Ward 19",
                Price = 2600000m
            },

            new Distance
            {
                DistanceID = 216,
                District = "Binh Thanh District",
                Area = "Ward 21",
                Price = 2650000m
            },

            new Distance
            {
                DistanceID = 217,
                District = "Binh Thanh District",
                Area = "Ward 22",
                Price = 2700000m
            },

            new Distance
            {
                DistanceID = 218,
                District = "Binh Thanh District",
                Area = "Ward 24",
                Price = 2750000m
            },

            new Distance
            {
                DistanceID = 219,
                District = "Binh Thanh District",
                Area = "Ward 25",
                Price = 2800000m
            },

            new Distance
            {
                DistanceID = 220,
                District = "Binh Thanh District",
                Area = "Ward 26",
                Price = 2850000m
            },

            new Distance
            {
                DistanceID = 221,
                District = "Binh Thanh District",
                Area = "Ward 27",
                Price = 2900000m
            },

            new Distance
            {
                DistanceID = 222,
                District = "Binh Thanh District",
                Area = "Ward 28",
                Price = 2950000m
            },

        // Phu Nhuan District: 15 area
            new Distance
            {
                DistanceID = 223,
                District = "Phu Nhuan District",
                Area = "Ward 1",
                Price = 2000000m
            },

            new Distance
            {
                DistanceID = 224,
                District = "Phu Nhuan District",
                Area = "Ward 2",
                Price = 2050000m
            },

            new Distance
            {
                DistanceID = 225,
                District = "Phu Nhuan District",
                Area = "Ward 3",
                Price = 2100000m
            },

            new Distance
            {
                DistanceID = 226,
                District = "Phu Nhuan District",
                Area = "Ward 4",
                Price = 2150000m
            },

            new Distance
            {
                DistanceID = 227,
                District = "Phu Nhuan District",
                Area = "Ward 5",
                Price = 2200000m
            },

            new Distance
            {
                DistanceID = 228,
                District = "Phu Nhuan District",
                Area = "Ward 7",
                Price = 2250000m
            },

            new Distance
            {
                DistanceID = 229,
                District = "Phu Nhuan District",
                Area = "Ward 8",
                Price = 2300000m
            },

            new Distance
            {
                DistanceID = 230,
                District = "Phu Nhuan District",
                Area = "Ward 9",
                Price = 2350000m
            },

            new Distance
            {
                DistanceID = 231,
                District = "Phu Nhuan District",
                Area = "Ward 10",
                Price = 2400000m
            },

            new Distance
            {
                DistanceID = 232,
                District = "Phu Nhuan District",
                Area = "Ward 11",
                Price = 2450000m
            },

            new Distance
            {
                DistanceID = 233,
                District = "Phu Nhuan District",
                Area = "Ward 12",
                Price = 2500000m
            },

            new Distance
            {
                DistanceID = 234,
                District = "Phu Nhuan District",
                Area = "Ward 13",
                Price = 2550000m
            },

            new Distance
            {
                DistanceID = 235,
                District = "Phu Nhuan District",
                Area = "Ward 14",
                Price = 2600000m
            },

            new Distance
            {
                DistanceID = 236,
                District = "Phu Nhuan District",
                Area = "Ward 15",
                Price = 2650000m
            },

            new Distance
            {
                DistanceID = 237,
                District = "Phu Nhuan District",
                Area = "Ward 17",
                Price = 2700000m
            },

        // District Thu Duc: 12 area
            new Distance
            {
                DistanceID = 238,
                District = "Thu Duc District",
                Area = "Binh Chieu Ward",
                Price = 2000000m
            },

            new Distance
            {
                DistanceID = 239,
                District = "Thu Duc District",
                Area = "Binh Tho Ward",
                Price = 2050000m
            },

            new Distance
            {
                DistanceID = 240,
                District = "Thu Duc District",
                Area = "Hiep Binh Chanh Ward",
                Price = 2100000m
            },

            new Distance
            {
                DistanceID = 241,
                District = "Thu Duc District",
                Area = "Hiep Binh Phuoc Ward",
                Price = 2150000m
            },

            new Distance
            {
                DistanceID = 242,
                District = "Thu Duc District",
                Area = "Linh Chieu Ward",
                Price = 2200000m
            },

            new Distance
            {
                DistanceID = 243,
                District = "Thu Duc District",
                Area = "Linh Dong Ward",
                Price = 2250000m
            },

            new Distance
            {
                DistanceID = 244,
                District = "Thu Duc District",
                Area = "Linh Tay Ward",
                Price = 2300000m
            },

            new Distance
            {
                DistanceID = 245,
                District = "Thu Duc District",
                Area = "Linh Trung Ward",
                Price = 2350000m
            },

            new Distance
            {
                DistanceID = 246,
                District = "Thu Duc District",
                Area = "Linh Xuan Ward",
                Price = 2400000m
            },

            new Distance
            {
                DistanceID = 247,
                District = "Thu Duc District",
                Area = "Tam Binh Ward",
                Price = 2450000m
            },

            new Distance
            {
                DistanceID = 248,
                District = "Thu Duc District",
                Area = "Tam Phu Ward",
                Price = 2500000m
            },

            new Distance
            {
                DistanceID = 249,
                District = "Thu Duc District",
                Area = "Truong Tho Ward",
                Price = 2550000m
            },

        // District Binh Tan: 10 area
            new Distance
            {
                DistanceID = 250,
                District = "Binh Tan District",
                Area = "Binh Hung Hoa Ward",
                Price = 2000000m
            },

            new Distance
            {
                DistanceID = 251,
                District = "Binh Tan District",
                Area = "Binh Hung Hoa A Ward",
                Price = 2050000m
            },

            new Distance
            {
                DistanceID = 252,
                District = "Binh Tan District",
                Area = "Binh Hung Hoa B Ward",
                Price = 2100000m
            },

            new Distance
            {
                DistanceID = 253,
                District = "Binh Tan District",
                Area = "Binh Tri Dong Ward",
                Price = 2150000m
            },

            new Distance
            {
                DistanceID = 254,
                District = "Binh Tan District",
                Area = "Binh Tri Dong A Ward",
                Price = 2200000m
            },

            new Distance
            {
                DistanceID = 255,
                District = "Binh Tan District",
                Area = "Binh Tri Dong B Ward",
                Price = 2250000m
            },

            new Distance
            {
                DistanceID = 256,
                District = "Binh Tan District",
                Area = "Tan Tao Ward",
                Price = 2300000m
            },

            new Distance
            {
                DistanceID = 257,
                District = "Binh Tan District",
                Area = "Tan Tao A Ward",
                Price = 2350000m
            },

            new Distance
            {
                DistanceID = 258,
                District = "Binh Tan District",
                Area = "An Lac Ward",
                Price = 2400000m
            },

            new Distance
            {
                DistanceID = 259,
                District = "Binh Tan District",
                Area = "An Lac A Ward",
                Price = 2450000m
            },

            // Cu Chi District: 1 town and 20 Commune
            new Distance
            {
                DistanceID = 260,
                District = "Cu Chi District",
                Area = "Cu Chi Town",
                Price = 2000000m
            },

            new Distance
            {
                DistanceID = 261,
                District = "Cu Chi District",
                Area = "Phu My Hung Commune",
                Price = 2050000m
            },

            new Distance
            {
                DistanceID = 262,
                District = "Cu Chi District",
                Area = "An Phu Commune",
                Price = 2100000m
            },

            new Distance
            {
                DistanceID = 263,
                District = "Cu Chi District",
                Area = "Trung Lap Thuong Commune",
                Price = 2150000m
            },

            new Distance
            {
                DistanceID = 264,
                District = "Cu Chi District",
                Area = "An Nhon Tay Commune",
                Price = 2200000m
            },

            new Distance
            {
                DistanceID = 265,
                District = "Cu Chi District",
                Area = "Nhuan Duc Commune",
                Price = 2250000m
            },
            new Distance
            {
                DistanceID = 266,
                District = "Cu Chi District",
                Area = "Pham Van Co Commune",
                Price = 2300000m
            },

            new Distance
            {
                DistanceID = 267,
                District = "Cu Chi District",
                Area = "Phu Hoa Dong Commune",
                Price = 2350000m
            },

            new Distance
            {
                DistanceID = 268,
                District = "Cu Chi District",
                Area = "Trung Lap Ha Commune",
                Price = 2400000m
            },

            new Distance
            {
                DistanceID = 269,
                District = "Cu Chi District",
                Area = "Trung An Commune",
                Price = 2450000m
            },

            new Distance
            {
                DistanceID = 270,
                District = "Cu Chi District",
                Area = "Phuoc Thanh Commune",
                Price = 2500000m
            },

            new Distance
            {
                DistanceID = 271,
                District = "Cu Chi District",
                Area = "Phuoc Hiep Commune",
                Price = 2550000m
            },

            new Distance
            {
                DistanceID = 272,
                District = "Cu Chi District",
                Area = "Tan An Hoi Commune",
                Price = 2600000m
            },

            new Distance
            {
                DistanceID = 273,
                District = "Cu Chi District",
                Area = "Phuoc Vinh An Commune",
                Price = 2650000m
            },

            new Distance
            {
                DistanceID = 274,
                District = "Cu Chi District",
                Area = "Thai My Commune",
                Price = 2700000m
            },

            new Distance
            {
                DistanceID = 275,
                District = "Cu Chi District",
                Area = "Tan Thanh Tay Commune",
                Price = 2750000m
            },

            new Distance
            {
                DistanceID = 276,
                District = "Cu Chi District",
                Area = "Hoa Phu Commune",
                Price = 2800000m
            },

            new Distance
            {
                DistanceID = 277,
                District = "Cu Chi District",
                Area = "Tan Thanh Dong Commune",
                Price = 2850000m
            },

            new Distance
            {
                DistanceID = 278,
                District = "Cu Chi District",
                Area = "Binh My Commune",
                Price = 2900000m
            },

            new Distance
            {
                DistanceID = 279,
                District = "Cu Chi District",
                Area = "Tan Phu Trung Commune",
                Price = 2950000m
            },

            new Distance
            {
                DistanceID = 280,
                District = "Cu Chi District",
                Area = "Tan Thong Hoi Commune",
                Price = 3000000m
            },

            // Hoc Mon District: 1 town and 11 commune
            new Distance
            {
                DistanceID = 281,
                District = "Hoc Mon District",
                Area = "Hoc Mon Town",
                Price = 2000000m
            },

            new Distance
            {
                DistanceID = 282,
                District = "Hoc Mon District",
                Area = "Ba Diem Commune",
                Price = 2050000m
            },

            new Distance
            {
                DistanceID = 283,
                District = "Hoc Mon District",
                Area = "Dong Thanh Commune",
                Price = 2100000m
            },

            new Distance
            {
                DistanceID = 284,
                District = "Hoc Mon District",
                Area = "Nhi Binh Commune",
                Price = 2150000m
            },

            new Distance
            {
                DistanceID = 285,
                District = "Hoc Mon District",
                Area = "Tan Hiep Commune",
                Price = 2200000m
            },

            new Distance
            {
                DistanceID = 286,
                District = "Hoc Mon District",
                Area = "Tan Thoi Nhi Commune",
                Price = 2250000m
            },

            new Distance
            {
                DistanceID = 287,
                District = "Hoc Mon District",
                Area = "Tan Xuan Commune",
                Price = 2300000m
            },

            new Distance
            {
                DistanceID = 288,
                District = "Hoc Mon District",
                Area = "Thoi Tam Thon Commune",
                Price = 2350000m
            },

            new Distance
            {
                DistanceID = 289,
                District = "Hoc Mon District",
                Area = "Trung Chanh Commune",
                Price = 2400000m
            },

            new Distance
            {
                DistanceID = 290,
                District = "Hoc Mon District",
                Area = "Xuan Thoi Dong Commune",
                Price = 2450000m
            },

            new Distance
            {
                DistanceID = 291,
                District = "Hoc Mon District",
                Area = "Xuan Thoi Son Commune",
                Price = 2500000m
            },

            new Distance
            {
                DistanceID = 292,
                District = "Hoc Mon District",
                Area = "Xuan Thoi Thuong Commune",
                Price = 2550000m
            },
            // Binh Chanh District: 1 town and 16 commune
            new Distance
            {
                DistanceID = 293,
                District = "Binh Chanh District",
                Area = "Tan Tuc Town",
                Price = 2000000m
            },

            new Distance
            {
                DistanceID = 294,
                District = "Binh Chanh District",
                Area = "Tan Kien Commune",
                Price = 2050000m
            },

            new Distance
            {
                DistanceID = 295,
                District = "Binh Chanh District",
                Area = "Tan Nhat Commune",
                Price = 2100000m
            },

            new Distance
            {
                DistanceID = 296,
                District = "Binh Chanh District",
                Area = "An Phu Tay Commune",
                Price = 2150000m
            },

            new Distance
            {
                DistanceID = 297,
                District = "Binh Chanh District",
                Area = "Tan Quy Tay Commune",
                Price = 2200000m
            },

            new Distance
            {
                DistanceID = 298,
                District = "Binh Chanh District",
                Area = "Hung Long Commune",
                Price = 2250000m
            },

            new Distance
            {
                DistanceID = 299,
                District = "Binh Chanh District",
                Area = "Qui Duc Commune",
                Price = 2300000m
            },

            new Distance
            {
                DistanceID = 300,
                District = "Binh Chanh District",
                Area = "Binh Chanh Commune",
                Price = 2350000m
            },

            new Distance
            {
                DistanceID = 301,
                District = "Binh Chanh District",
                Area = "Le Minh Xuan Commune",
                Price = 2400000m
            },

            new Distance
            {
                DistanceID = 302,
                District = "Binh Chanh District",
                Area = "Pham Van Hai Commune",
                Price = 2450000m
            },

            new Distance
            {
                DistanceID = 303,
                District = "Binh Chanh District",
                Area = "Binh Hung Commune",
                Price = 2500000m
            },

            new Distance
            {
                DistanceID = 304,
                District = "Binh Chanh District",
                Area = "Binh Loi Commune",
                Price = 2550000m
            },

            new Distance
            {
                DistanceID = 305,
                District = "Binh Chanh District",
                Area = "Da Phuoc Commune",
                Price = 2600000m
            },

            new Distance
            {
                DistanceID = 306,
                District = "Binh Chanh District",
                Area = "Phong Phu Commune",
                Price = 2650000m
            },

            new Distance
            {
                DistanceID = 307,
                District = "Binh Chanh District",
                Area = "Vinh Loc A Commune",
                Price = 2700000m
            },

            new Distance
            {
                DistanceID = 308,
                District = "Binh Chanh District",
                Area = "Vinh Loc B Commune",
                Price = 2750000m
            },

            // Nha Be District: 1 town and 6 commune
            new Distance
            {
                DistanceID = 309,
                District = "Nha Be District",
                Area = "Nha Be Town",
                Price = 2000000m
            },

            new Distance
            {
                DistanceID = 310,
                District = "Nha Be District",
                Area = "Hiep Phuoc Commune",
                Price = 2050000m
            },

            new Distance
            {
                DistanceID = 311,
                District = "Nha Be District",
                Area = "Long Thoi Commune",
                Price = 2100000m
            },

            new Distance
            {
                DistanceID = 312,
                District = "Nha Be District",
                Area = "Nhon Duc Commune",
                Price = 2150000m
            },

            new Distance
            {
                DistanceID = 313,
                District = "Nha Be District",
                Area = "Phu Xuan Commune",
                Price = 2200000m
            },

            new Distance
            {
                DistanceID = 314,
                District = "Nha Be District",
                Area = "Phuoc Kien Commune",
                Price = 2250000m
            },

            new Distance
            {
                DistanceID = 315,
                District = "Nha Be District",
                Area = "Phuoc Loc Commune",
                Price = 2300000m
            },

            // Can Gio District: 1 town and 6 commune
            new Distance
            {
                DistanceID = 316,
                District = "Can Gio District",
                Area = "Can Thanh Town",
                Price = 2000000m
            },

            new Distance
            {
                DistanceID = 317,
                District = "Can Gio District",
                Area = "Binh Khanh Commune",
                Price = 2050000m
            },

            new Distance
            {
                DistanceID = 318,
                District = "Can Gio District",
                Area = "An Thoi Dong Commune",
                Price = 2100000m
            },

            new Distance
            {
                DistanceID = 319,
                District = "Can Gio District",
                Area = "Tam Thon Hiep Commune",
                Price = 2150000m
            },

            new Distance
            {
                DistanceID = 320,
                District = "Can Gio District",
                Area = "Thanh An Commune",
                Price = 2200000m
            },

            new Distance
            {
                DistanceID = 321,
                District = "Can Gio District",
                Area = "Ly Nhon Commune",
                Price = 2250000m
            },

            new Distance
            {
                DistanceID = 322,
                District = "Can Gio District",
                Area = "Long Hoa Commune",
                Price = 2300000m
            }
        );

        // Seed data for Customers
        modelBuilder.Entity<Customer>().HasData(
            new Customer
            {
                Id = "c1",
                FirstName = "Alice",
                LastName = "Johnson",
                UserName = "alice",
                NormalizedUserName = "ALICE",
                Gender = false,
                Address = "789 Customer Lane",
                ImageURL = "https://example.com/customer1.jpg",
                ImagePublicId = "customer1_image_id"
            },
            new Customer
            {
                Id = "c2",
                FirstName = "Bob",
                LastName = "Williams",
                UserName = "boooob",
                NormalizedUserName = "BOOOOOB",
                Gender = true,
                Address = "123 Customer Ave",
                ImageURL = "https://example.com/customer2.jpg",
                ImagePublicId = "customer2_image_id"
            }
        );

        // Seed data for staff
        // modelBuilder.Entity<Staff>().HasData(
            // new Staff
            // {
            //     Id = "s1",
            //     FirstName = "staff1",
            //     LastName = "Johnson",
            //     UserName = "sitap",
            //     NormalizedUserName = "SITAP",
            //     Gender = false,
            //     Address = "789 Staff Lane",
            //     ImageURL = "https://example.com/staff1.jpg",
            //     ImagePublicId = "staff_image_id",
            //     IsManager = false
            // },
            // new Staff
            // {
            //     Id = "m2",
            //     FirstName = "manager",
            //     LastName = "Williams",
            //     UserName = "manager",
            //     NormalizedUserName = "MANAGER",
            //     Gender = true,
            //     Address = "123 Staff Ave",
            //     ImageURL = "https://example.com/staff2.jpg",
            //     ImagePublicId = "staff2_image_id",
            //     IsManager = true

            // }
        // );
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
