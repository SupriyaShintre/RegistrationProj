using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Registration.Models
{
    public partial class student_dbContext : DbContext
    {
        public student_dbContext()
        {
        }

        public student_dbContext(DbContextOptions<student_dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<StudentInfo> StudentInfos { get; set; } = null!;
        public virtual DbSet<StudentInformation> StudentInformations { get; set; } = null!;
        public virtual DbSet<StudentTb> StudentTbs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {


            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("student_info");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Email)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Gender)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("gender");

                entity.Property(e => e.Lan)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("lan");

                entity.Property(e => e.Lang)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("lang");

                entity.Property(e => e.MobileNo).HasColumnName("mobileNo");

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<StudentInformation>(entity =>
            {
                entity.ToTable("student_information");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Gender)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("gender");

                entity.Property(e => e.Lang)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("lang");

                entity.Property(e => e.Languages)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("language");

                entity.Property(e => e.MobileNo)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("mobileNo");

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<StudentTb>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("student_tb");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Email)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Gender)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("gender");

                entity.Property(e => e.Lan)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("lan");

                entity.Property(e => e.Lang)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("lang");

                entity.Property(e => e.MobileNo).HasColumnName("mobileNo");

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
