using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MyAngularAppCore.Entities
{
    public partial class MyAngularAppContext : DbContext
    {
        public MyAngularAppContext()
        {
        }

        public MyAngularAppContext(DbContextOptions<MyAngularAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BuyOrder> BuyOrder { get; set; }
        public virtual DbSet<SaleOrder> SaleOrder { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-G8863CP\\SQLEXPRESS;Database=MyAngularApp;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BuyOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.Property(e => e.OrderId).HasColumnName("Order_ID");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BuyOrder)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_BuyOrder_Users");
            });

            modelBuilder.Entity<SaleOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.Property(e => e.OrderId).HasColumnName("Order_ID");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SaleOrder)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_SaleOrder_Users");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.TransactionId).HasColumnName("Transaction_ID");

                entity.Property(e => e.BuyUserId).HasColumnName("Buy_UserID");

                entity.Property(e => e.SaleUserId).HasColumnName("Sale_UserID");

                entity.Property(e => e.TransactionDateTime)
                    .HasColumnName("Transaction_DateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.TransactionType).HasColumnName("Transaction_Type");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.HasOne(d => d.BuyUser)
                    .WithMany(p => p.TransactionBuyUser)
                    .HasForeignKey(d => d.BuyUserId)
                    .HasConstraintName("FK_Transaction_Buy");

                entity.HasOne(d => d.SaleUser)
                    .WithMany(p => p.TransactionSaleUser)
                    .HasForeignKey(d => d.SaleUserId)
                    .HasConstraintName("FK_Transaction_Sale");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TransactionUser)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Transaction_Users");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.Property(e => e.Email).HasMaxLength(320);

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.S).HasMaxLength(10);

                entity.Property(e => e.WalletAmount).HasColumnType("decimal(18, 0)");
            });
        }
    }
}
