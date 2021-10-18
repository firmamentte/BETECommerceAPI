using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace BETECommerceAPI.Data.Entities
{
    public partial class BETECommerceContext : DbContext
    {
        public BETECommerceContext()
        {
        }

        public BETECommerceContext(DbContextOptions<BETECommerceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<ItemDetail> ItemDetails { get; set; }
        public virtual DbSet<ItemDetailPicture> ItemDetailPictures { get; set; }
        public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual DbSet<PurchaseOrderItemDetail> PurchaseOrderItemDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(FirmamentUtilities.Utilities.DatabaseHelper.ConnectionString);
                optionsBuilder.UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("ApplicationUser");

                entity.Property(e => e.ApplicationUserId).ValueGeneratedNever();

                entity.Property(e => e.UserPassword)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ItemDetail>(entity =>
            {
                entity.ToTable("ItemDetail");

                entity.Property(e => e.ItemDetailId).ValueGeneratedNever();

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.ItemDescription)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.PercentageDiscount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SalePrice).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<ItemDetailPicture>(entity =>
            {
                entity.ToTable("ItemDetailPicture");

                entity.Property(e => e.ItemDetailPictureId).ValueGeneratedNever();

                entity.Property(e => e.PictureFileName)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.ItemDetail)
                    .WithMany(p => p.ItemDetailPictures)
                    .HasForeignKey(d => d.ItemDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemDetailPicture_ItemDetail");
            });

            modelBuilder.Entity<PurchaseOrder>(entity =>
            {
                entity.ToTable("PurchaseOrder");

                entity.Property(e => e.PurchaseOrderId).ValueGeneratedNever();

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentStatus)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PurchaseOrderNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShippingStatus)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.ApplicationUser)
                    .WithMany(p => p.PurchaseOrders)
                    .HasForeignKey(d => d.ApplicationUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PurchaseOrder_ApplicationUser");
            });

            modelBuilder.Entity<PurchaseOrderItemDetail>(entity =>
            {
                entity.ToTable("PurchaseOrderItemDetail");

                entity.Property(e => e.PurchaseOrderItemDetailId).ValueGeneratedNever();

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.PercentageDiscount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PictureFileName)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.SalePrice).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.ItemDetail)
                    .WithMany(p => p.PurchaseOrderItemDetails)
                    .HasForeignKey(d => d.ItemDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PurchaseOrderItemDetail_ItemDetail");

                entity.HasOne(d => d.PurchaseOrder)
                    .WithMany(p => p.PurchaseOrderItemDetails)
                    .HasForeignKey(d => d.PurchaseOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PurchaseOrderItemDetail_PurchaseOrder");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
