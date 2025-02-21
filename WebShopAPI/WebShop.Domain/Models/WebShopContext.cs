using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebShop.Domain.Models;

public partial class WebShopContext : DbContext
{
    public WebShopContext()
    {
    }

    public WebShopContext(DbContextOptions<WebShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderProductRequested> OrderProductRequesteds { get; set; }

    public virtual DbSet<PayMethod> PayMethods { get; set; }

    public virtual DbSet<PaymentStatus> PaymentStatuses { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A2B442542A4");

            entity.ToTable("Category");

            entity.HasIndex(e => e.Name, "UQ__Category__737584F6EB4BC24D").IsUnique();

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__C3905BAFA93F75AA");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaidDate).HasColumnType("datetime");
            entity.Property(e => e.PayMethodId).HasColumnName("PayMethodID");
            entity.Property(e => e.PaymentStatusId).HasColumnName("PaymentStatusID");
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.PayMethod).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PayMethodId)
                .HasConstraintName("FK__Order__PayMethod__440B1D61");

            entity.HasOne(d => d.PaymentStatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentStatusId)
                .HasConstraintName("FK__Order__PaymentSt__44FF419A");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Order__UserID__4316F928");
        });

        modelBuilder.Entity<OrderProductRequested>(entity =>
        {
            entity.HasKey(e => e.OrderProductRequestedId).HasName("PK__OrderPro__09A8CF35244A9F6F");

            entity.ToTable("OrderProductRequested");

            entity.Property(e => e.OrderProductRequestedId).HasColumnName("OrderProductRequestedID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Quantity).HasDefaultValue(1);

            entity.HasOne(d => d.Order).WithMany(p => p.OrderProductRequesteds)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderProd__Order__48CFD27E");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderProductRequesteds)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__OrderProd__Produ__49C3F6B7");
        });

        modelBuilder.Entity<PayMethod>(entity =>
        {
            entity.HasKey(e => e.PayMethodId).HasName("PK__PayMetho__E3C33F3D7F576E6E");

            entity.ToTable("PayMethod");

            entity.HasIndex(e => e.Name, "UQ__PayMetho__737584F6827729EB").IsUnique();

            entity.Property(e => e.PayMethodId).HasColumnName("PayMethodID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PaymentStatus>(entity =>
        {
            entity.HasKey(e => e.PaymentStatusId).HasName("PK__PaymentS__34F8AC1FE412DF34");

            entity.ToTable("PaymentStatus");

            entity.HasIndex(e => e.Name, "UQ__PaymentS__737584F6D89FD21F").IsUnique();

            entity.Property(e => e.PaymentStatusId).HasColumnName("PaymentStatusID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__B40CC6EDF5CF505F");

            entity.ToTable("Product");

            entity.HasIndex(e => e.ReferenceCode, "UQ__Product__E16A0484E9DE6A8C").IsUnique();

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ReferenceCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasMany(d => d.Categories).WithMany(p => p.Products)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductCategory",
                    r => r.HasOne<Category>().WithMany()
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("FK__ProductCa__Categ__2E1BDC42"),
                    l => l.HasOne<Product>().WithMany()
                        .HasForeignKey("ProductId")
                        .HasConstraintName("FK__ProductCa__Produ__2D27B809"),
                    j =>
                    {
                        j.HasKey("ProductId", "CategoryId").HasName("PK__ProductC__159C554FDAE77E94");
                        j.ToTable("ProductCategories");
                        j.IndexerProperty<int>("ProductId").HasColumnName("ProductID");
                        j.IndexerProperty<int>("CategoryId").HasColumnName("CategoryID");
                    });
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE3ABFB422C7");

            entity.ToTable("Role");

            entity.HasIndex(e => e.Name, "UQ__Role__737584F66AA25364").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCAC37133033");

            entity.ToTable("User");

            entity.HasIndex(e => e.Identification, "UQ__User__724F06FD62F60164").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__User__A9D10534500059C0").IsUnique();

            entity.HasIndex(e => e.UserName, "UQ__User__C9F284565C62470E").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Identification)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__User__RoleID__398D8EEE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
