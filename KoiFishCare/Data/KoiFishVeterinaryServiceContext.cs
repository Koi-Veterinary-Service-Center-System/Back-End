using System;
using System.Collections.Generic;
using KoiFishCare.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using WebApplication1.Models;

namespace WebApplication1.Data;

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
            .HasOne(b => b.Vet)
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
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
