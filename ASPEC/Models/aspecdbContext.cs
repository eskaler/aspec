using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ASPEC.Models
{
    public partial class aspecdbContext : DbContext
    {
        public aspecdbContext()
        {
        }

        public aspecdbContext(DbContextOptions<aspecdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Depart> Depart { get; set; }
        public virtual DbSet<Device> Device { get; set; }
        public virtual DbSet<Measurement> Measurement { get; set; }
        public virtual DbSet<Point> Point { get; set; }
        public virtual DbSet<RadiationType> RadiationType { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Shift> Shift { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Task> Task { get; set; }
        public virtual DbSet<Unit> Unit { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(File.ReadAllText("dbconfig.txt"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Depart>(entity =>
            {
                entity.ToTable("depart");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("character varying");

                entity.Property(e => e.ParentId).HasColumnName("parent_id");

                entity.Property(e => e.PrjMark)
                    .HasColumnName("prj_mark")
                    .HasMaxLength(128);

                entity.Property(e => e.Scheme)
                    .HasColumnName("scheme")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("depart_fk");
            });

            modelBuilder.Entity<Device>(entity =>
            {
                entity.ToTable("device");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<Measurement>(entity =>
            {
                entity.ToTable("measurement");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateTime)
                    .HasColumnName("date_time")
                    .HasColumnType("timestamp");

                entity.Property(e => e.DeviceId).HasColumnName("device_id");

                entity.Property(e => e.TaskId).HasColumnName("task_id");

                entity.Property(e => e.UnitId).HasColumnName("unit_id");

                entity.Property(e => e.Value).HasColumnName("value");

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.Measurement)
                    .HasForeignKey(d => d.DeviceId)
                    .HasConstraintName("measurement_fk_2");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.Measurement)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("measurement_fk");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.Measurement)
                    .HasForeignKey(d => d.UnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("measurement_fk_1");
            });

            modelBuilder.Entity<Point>(entity =>
            {
                entity.ToTable("point");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.ParentId).HasColumnName("parent_id");

                entity.Property(e => e.PosX).HasColumnName("pos_x");

                entity.Property(e => e.PosY).HasColumnName("pos_y");

                entity.Property(e => e.PosZ).HasColumnName("pos_z");

                entity.Property(e => e.PrjMark)
                    .HasColumnName("prj_mark")
                    .HasMaxLength(255);

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.Point)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("point_fk");
            });

            modelBuilder.Entity<RadiationType>(entity =>
            {
                entity.ToTable("radiation_type");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<Shift>(entity =>
            {
                entity.ToTable("shift");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Shift)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("shift_fk");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("status");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("task");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClosedDate)
                    .HasColumnName("closed_date")
                    .HasColumnType("timestamp");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasColumnType("timestamp");

                entity.Property(e => e.CreatorId).HasColumnName("creator_id");

                entity.Property(e => e.ExecutorId).HasColumnName("executor_id");

                entity.Property(e => e.PointId).HasColumnName("point_id");

                entity.Property(e => e.RadiationTypeId).HasColumnName("radiation_type_id");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.TaskCreator)
                    .HasForeignKey(d => d.CreatorId)
                    .HasConstraintName("task_fk");

                entity.HasOne(d => d.Executor)
                    .WithMany(p => p.TaskExecutor)
                    .HasForeignKey(d => d.ExecutorId)
                    .HasConstraintName("task_fk_1");

                entity.HasOne(d => d.Point)
                    .WithMany(p => p.Task)
                    .HasForeignKey(d => d.PointId)
                    .HasConstraintName("task_fk_4");

                entity.HasOne(d => d.RadiationType)
                    .WithMany(p => p.Task)
                    .HasForeignKey(d => d.RadiationTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("task_fk_2");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Task)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("task_fk_3");
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.ToTable("unit");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BirthDate)
                    .HasColumnName("birth_date")
                    .HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasColumnName("login")
                    .HasColumnType("character varying");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasColumnType("character varying");

                entity.Property(e => e.Patronymic)
                    .IsRequired()
                    .HasColumnName("patronymic")
                    .HasColumnType("character varying");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.SecondName)
                    .IsRequired()
                    .HasColumnName("second_name")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_fk");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
