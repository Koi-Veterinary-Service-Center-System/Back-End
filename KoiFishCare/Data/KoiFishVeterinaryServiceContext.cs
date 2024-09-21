using System;
using System.Collections.Generic;
using KoiFishCare.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public partial class KoiFishVeterinaryServiceContext : DbContext
{
    public KoiFishVeterinaryServiceContext()
    {
    }

    public KoiFishVeterinaryServiceContext(DbContextOptions<KoiFishVeterinaryServiceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Distance> Distances { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<KoiOrPool> KoiOrPools { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PrescriptionRecord> PrescriptionRecords { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Slot> Slots { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Veterinarian> Veterinarians { get; set; }

    public virtual DbSet<VetSlot> VetSlot { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__349DA586AD67902E");

            entity.ToTable("Account");

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Booking__73951ACD523EDE51");

            entity.ToTable("Booking");

            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.CustomerId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CustomerID");
            entity.Property(e => e.DistanceId).HasColumnName("DistanceID");
            entity.Property(e => e.KoiOrPoolId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("KoiOrPoolID");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Note).HasColumnType("text");
            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.SlotId).HasColumnName("SlotID");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.VetId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VetID");

            entity.HasOne(d => d.Customer).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Booking__Custome__5AEE82B9");

            entity.HasOne(d => d.Distance).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.DistanceId)
                .HasConstraintName("FK__Booking__Distanc__5CD6CB2B");

            entity.HasOne(d => d.KoiOrPool).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.KoiOrPoolId)
                .HasConstraintName("FK__Booking__KoiOrPo__5DCAEF64");

            entity.HasOne(d => d.Payment).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.PaymentId)
                .HasConstraintName("FK__Booking__Payment__59063A47");

            entity.HasOne(d => d.Service).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__Booking__Service__5812160E");

            entity.HasOne(d => d.Slot).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.SlotId)
                .HasConstraintName("FK__Booking__SlotID__59FA5E80");

            entity.HasOne(d => d.Vet).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.VetId)
                .HasConstraintName("FK__Booking__VetID__5BE2A6F2");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B8E0E2996A");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.AccountId, "UQ__Customer__349DA5876005DF31").IsUnique();

            entity.Property(e => e.CustomerId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("('C'+left(newid(),(8)))")
                .HasColumnName("CustomerID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.DefaultAddress)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ImageURL");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithOne(p => p.Customer)
                .HasForeignKey<Customer>(d => d.AccountId)
                .HasConstraintName("FK__Customer__Accoun__412EB0B6");
        });

        modelBuilder.Entity<Distance>(entity =>
        {
            entity.HasKey(e => e.DistanceId).HasName("PK__Distance__A24E2A1C7F790CA5");

            entity.ToTable("Distance");

            entity.Property(e => e.DistanceId).HasColumnName("DistanceID");
            entity.Property(e => e.Kilometer).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__6A4BEDF67764D7CE");

            entity.ToTable("Feedback");

            entity.Property(e => e.FeedbackId).HasColumnName("FeedbackID");
            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.Comments).HasColumnType("text");

            entity.HasOne(d => d.Booking).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("FK__Feedback__Bookin__60A75C0F");
        });

        modelBuilder.Entity<KoiOrPool>(entity =>
        {
            entity.HasKey(e => e.KoiOrPoolId).HasName("PK__KoiOrPoo__0612556F20091BA1");

            entity.ToTable("KoiOrPool");

            entity.Property(e => e.KoiOrPoolId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("('K'+left(newid(),(8)))")
                .HasColumnName("KoiOrPoolID");
            entity.Property(e => e.CustomerId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CustomerID");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Customer).WithMany(p => p.KoiOrPools)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__KoiOrPool__Custo__5535A963");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__9B556A58D9830F1F");

            entity.ToTable("Payment");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.Qrcode)
                .HasColumnType("text")
                .HasColumnName("QRCode");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PrescriptionRecord>(entity =>
        {
            entity.HasKey(e => e.PrescriptionRecordId).HasName("PK__Prescrip__1D4AC3184A0205C0");

            entity.ToTable("PrescriptionRecord");

            entity.Property(e => e.PrescriptionRecordId).HasColumnName("PrescriptionRecordID");
            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.DiseaseName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Medication).HasColumnType("text");
            entity.Property(e => e.Note).HasColumnType("text");
            entity.Property(e => e.Symptoms).HasColumnType("text");

            entity.HasOne(d => d.Booking).WithMany(p => p.PrescriptionRecords)
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("FK__Prescript__Booki__6383C8BA");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Service__C51BB0EA33D1CD3F");

            entity.ToTable("Service");

            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.EstimatedDuration).HasColumnName("Estimated_duration");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ServiceName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Slot>(entity =>
        {
            entity.HasKey(e => e.SlotId).HasName("PK__Slot__0A124A4F64E36AD6");

            entity.ToTable("Slot");

            entity.Property(e => e.SlotId).HasColumnName("SlotID");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__Staff__96D4AAF72FA07439");

            entity.HasIndex(e => e.AccountId, "UQ__Staff__349DA587A55D4CB8").IsUnique();

            entity.Property(e => e.StaffId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("('S'+left(newid(),(8)))")
                .HasColumnName("StaffID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ImageURL");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithOne(p => p.Staff)
                .HasForeignKey<Staff>(d => d.AccountId)
                .HasConstraintName("FK__Staff__AccountID__3B75D760");
        });

        modelBuilder.Entity<Veterinarian>(entity =>
        {
            entity.HasKey(e => e.VetId).HasName("PK__Veterina__2556B80E6CF88BB2");

            entity.ToTable("Veterinarian");

            entity.HasIndex(e => e.AccountId, "UQ__Veterina__349DA5875B7204C7").IsUnique();

            entity.Property(e => e.VetId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("('V'+left(newid(),(8)))")
                .HasColumnName("VetID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.ExperienceYears).HasColumnName("Experience_years");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ImageURL");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithOne(p => p.Veterinarian)
                .HasForeignKey<Veterinarian>(d => d.AccountId)
                .HasConstraintName("FK__Veterinar__Accou__45F365D3");

            entity.HasMany(d => d.Slots).WithMany(p => p.Vets)
                .UsingEntity<Dictionary<string, object>>(
                    "Having",
                    r => r.HasOne<Slot>().WithMany()
                        .HasForeignKey("SlotId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Having__SlotID__4D94879B"),
                    l => l.HasOne<Veterinarian>().WithMany()
                        .HasForeignKey("VetId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Having__VetID__4CA06362"),
                    j =>
                    {
                        j.HasKey("VetId", "SlotId").HasName("PK__Having__C5F79CAAFED7EB1E");
                        j.ToTable("Having");
                        j.IndexerProperty<string>("VetId")
                            .HasMaxLength(50)
                            .IsUnicode(false)
                            .HasColumnName("VetID");
                        j.IndexerProperty<int>("SlotId").HasColumnName("SlotID");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
