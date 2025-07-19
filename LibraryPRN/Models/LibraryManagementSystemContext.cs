using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LibraryPRN.Models;

public partial class LibraryManagementSystemContext : DbContext
{
    public LibraryManagementSystemContext()
    {
    }

    public LibraryManagementSystemContext(DbContextOptions<LibraryManagementSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Checkout> Checkouts { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Return> Returns { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var ConnectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(ConnectionString);
        }

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.AuthorId).HasName("PK__Authors__55B9F6DF8D40203E");

            entity.Property(e => e.AuthorId)
                .ValueGeneratedOnAdd()
                .HasColumnName("Author_ID");
            entity.Property(e => e.BirthYear).HasColumnName("Birth_Year");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Nationality)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__Books__C223F3947394DB12");

            entity.Property(e => e.BookId)
                .ValueGeneratedOnAdd()
                .HasColumnName("Book_ID");
            entity.Property(e => e.AuthorId).HasColumnName("Author_ID");
            entity.Property(e => e.Genre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("ISBN");
            entity.Property(e => e.PublicationYear).HasColumnName("Publication_Year");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Author).WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("FK__Books__Author_ID__4BAC3F29");
        });

        modelBuilder.Entity<Checkout>(entity =>
        {
            entity.HasKey(e => e.CheckoutId).HasName("PK__Checkout__9ABE9045F1A560B0");

            entity.Property(e => e.CheckoutId)
                .ValueGeneratedOnAdd()
                .HasColumnName("Checkout_ID");
            entity.Property(e => e.BookId).HasColumnName("Book_ID");
            entity.Property(e => e.CheckoutDate).HasColumnName("Checkout_Date");
            entity.Property(e => e.DueDate).HasColumnName("Due_Date");
            entity.Property(e => e.MemberId).HasColumnName("Member_ID");

            entity.HasOne(d => d.Book).WithMany(p => p.Checkouts)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK__Checkouts__Book___5165187F");

            entity.HasOne(d => d.Member).WithMany(p => p.Checkouts)
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("FK__Checkouts__Membe__5070F446");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PK__Members__42A68F27A53AE7CC");

            entity.Property(e => e.MemberId)
                .ValueGeneratedOnAdd()
                .HasColumnName("Member_ID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.JoinDate).HasColumnName("Join_Date");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Passwords).HasMaxLength(255);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
        });

        modelBuilder.Entity<Return>(entity =>
        {
            entity.HasKey(e => e.ReturnId).HasName("PK__Returns__0F4F4C568C0FB7A0");

            entity.Property(e => e.ReturnId)
                .ValueGeneratedOnAdd()
                .HasColumnName("Return_ID");
            entity.Property(e => e.CheckoutId).HasColumnName("Checkout_ID");
            entity.Property(e => e.ReturnDate).HasColumnName("Return_Date");

            entity.HasOne(d => d.Checkout).WithMany(p => p.Returns)
                .HasForeignKey(d => d.CheckoutId)
                .HasConstraintName("FK__Returns__Checkou__5441852A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
