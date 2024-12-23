﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Techa.DocumentGenerator.Infrastructure.Data;

#nullable disable

namespace Techa.DocumentGenerator.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241222191625_InitialDb")]
    partial class InitialDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Techa.DocumentGenerator.Domain.Entities.AAA.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<int?>("ModifiedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("ModifiedByUserId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Techa.DocumentGenerator.Domain.Entities.AAA.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasComputedColumnSql("[FirstName] + ' ' + [LastName]");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Mobile")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<int?>("ModifiedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("NationalCode")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProfileId")
                        .HasColumnType("int");

                    b.Property<Guid>("SecurityStamp")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("ModifiedByUserId");

                    b.HasIndex("ProfileId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Techa.DocumentGenerator.Domain.Entities.AAA.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<int?>("ModifiedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("ModifiedByUserId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Techa.DocumentGenerator.Domain.Entities.AttachmentFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("ModifiedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("ModifiedByUserId");

                    b.ToTable("AttachmentFiles");
                });

            modelBuilder.Entity("Techa.DocumentGenerator.Domain.Entities.EventLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("DataJson")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EntityId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EntityName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EventType")
                        .HasColumnType("int");

                    b.Property<string>("ExecutedScript")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("HasError")
                        .HasColumnType("bit");

                    b.Property<string>("IPAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Method")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ModifiedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("ModifiedByUserId");

                    b.ToTable("EventLogs");
                });

            modelBuilder.Entity("Techa.DocumentGenerator.Domain.Entities.AAA.Role", b =>
                {
                    b.HasOne("Techa.DocumentGenerator.Domain.Entities.AAA.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId");

                    b.HasOne("Techa.DocumentGenerator.Domain.Entities.AAA.User", "ModifiedByUser")
                        .WithMany()
                        .HasForeignKey("ModifiedByUserId");

                    b.Navigation("CreatedByUser");

                    b.Navigation("ModifiedByUser");
                });

            modelBuilder.Entity("Techa.DocumentGenerator.Domain.Entities.AAA.User", b =>
                {
                    b.HasOne("Techa.DocumentGenerator.Domain.Entities.AAA.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId");

                    b.HasOne("Techa.DocumentGenerator.Domain.Entities.AAA.User", "ModifiedByUser")
                        .WithMany()
                        .HasForeignKey("ModifiedByUserId");

                    b.HasOne("Techa.DocumentGenerator.Domain.Entities.AttachmentFile", "Profile")
                        .WithMany()
                        .HasForeignKey("ProfileId");

                    b.Navigation("CreatedByUser");

                    b.Navigation("ModifiedByUser");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("Techa.DocumentGenerator.Domain.Entities.AAA.UserRole", b =>
                {
                    b.HasOne("Techa.DocumentGenerator.Domain.Entities.AAA.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId");

                    b.HasOne("Techa.DocumentGenerator.Domain.Entities.AAA.User", "ModifiedByUser")
                        .WithMany()
                        .HasForeignKey("ModifiedByUserId");

                    b.HasOne("Techa.DocumentGenerator.Domain.Entities.AAA.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Techa.DocumentGenerator.Domain.Entities.AAA.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CreatedByUser");

                    b.Navigation("ModifiedByUser");

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Techa.DocumentGenerator.Domain.Entities.AttachmentFile", b =>
                {
                    b.HasOne("Techa.DocumentGenerator.Domain.Entities.AAA.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId");

                    b.HasOne("Techa.DocumentGenerator.Domain.Entities.AAA.User", "ModifiedByUser")
                        .WithMany()
                        .HasForeignKey("ModifiedByUserId");

                    b.Navigation("CreatedByUser");

                    b.Navigation("ModifiedByUser");
                });

            modelBuilder.Entity("Techa.DocumentGenerator.Domain.Entities.EventLog", b =>
                {
                    b.HasOne("Techa.DocumentGenerator.Domain.Entities.AAA.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId");

                    b.HasOne("Techa.DocumentGenerator.Domain.Entities.AAA.User", "ModifiedByUser")
                        .WithMany()
                        .HasForeignKey("ModifiedByUserId");

                    b.Navigation("CreatedByUser");

                    b.Navigation("ModifiedByUser");
                });

            modelBuilder.Entity("Techa.DocumentGenerator.Domain.Entities.AAA.User", b =>
                {
                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
