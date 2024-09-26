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

    public virtual DbSet<Staff> Staff { get; set; }

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
            }
        );

        // Seed data for Slots
        modelBuilder.Entity<Slot>().HasData(
            new Slot
            {
                SlotID = 1,
                StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(10, 0),
                WeekDate = "Monday"
            },
            new Slot
            {
                SlotID = 2,
                StartTime = new TimeOnly(11, 0),
                EndTime = new TimeOnly(12, 0),
                WeekDate = "Monday"
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
                SlotID = 2,
                isBooked = true
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
                EstimatedDuration = 1.5
            },
            new Service
            {
                ServiceID = 2,
                ServiceName = "Pool Maintenance",
                Description = "Comprehensive pool maintenance service.",
                Price = 250.00m,
                EstimatedDuration = 2.0
            }
        );

        // Seed data for Payments
        modelBuilder.Entity<Payment>().HasData(
            new Payment
            {
                PaymentID = 1,
                Qrcode = "qrcode1",
                Type = "Credit Card"
            },
            new Payment
            {
                PaymentID = 2,
                Qrcode = "qrcode2",
                Type = "PayPal"
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
                CustomerId = "c1" // Assuming customer with ID 'c1' exists
            },
            new KoiOrPool
            {
                KoiOrPoolID = 2,
                Name = "Smith's Pool",
                IsPool = true,
                Description = "A large swimming pool owned by the Smith family.",
                CustomerId = "c2" // Assuming customer with ID 'c2' exists
            }
        );

        // Seed data for Distance
        modelBuilder.Entity<Distance>().HasData(
            new Distance
            {
                DistanceID = 1,
                Price = 20.00m,
                Kilometer = 5.0m
            },
            new Distance
            {
                DistanceID = 2,
                Price = 50.00m,
                Kilometer = 15.0m
            }
        );

        // Seed data for Customers
        modelBuilder.Entity<Customer>().HasData(
            new Customer
            {
                Id = "c1",
                FirstName = "Alice",
                LastName = "Johnson",
                UserName = "alicu",
                NormalizedUserName = "ALICU",
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

        //Seed data for staff
        modelBuilder.Entity<Staff>().HasData(
            new Staff
            {
                Id = "s1",
                FirstName = "staff1",
                LastName = "Johnson",
                UserName = "sitap",
                NormalizedUserName = "SITAP",
                Gender = false,
                Address = "789 Staff Lane",
                ImageURL = "https://example.com/staff1.jpg",
                ImagePublicId = "staff_image_id",
                IsManager = false
            },
            new Staff
            {
                Id = "m2",
                FirstName = "manager",
                LastName = "Williams",
                UserName = "manager",
                NormalizedUserName = "MANAGER",
                Gender = true,
                Address = "123 Staff Ave",
                ImageURL = "https://example.com/staff2.jpg",
                ImagePublicId = "staff2_image_id",
                IsManager = true
            }
        );
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
