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
    [Migration("20250401091944_Mig_20250401_01")]
    partial class Mig_20250401_01
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

                    b.Property<int?>("ProjectId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("ModifiedByUserId");

                    b.HasIndex("ProjectId");

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

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("ModifiedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProfileId")
                        .HasColumnType("int");

                    b.Property<int?>("ProjectId")
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

                    b.HasIndex("ProjectId");

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

            modelBuilder.Entity("Techa.DocumentGenerator.Domain.Entities.BaseInfo.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ConnectionString")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("DbName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DbPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DbServerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DbUserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

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

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Techa.DocumentGenerator.Domain.Entities.DbInfo.StoredProcedure", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Alias")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<int?>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ModifiedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProcedureName")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<byte>("StoredProcedureType")
                        .HasColumnType("tinyint");

                    b.Property<string>("Title")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("ModifiedByUserId");

                    b.HasIndex("ProjectId");

                    b.ToTable("StoredProcedures");
                });

            modelBuilder.Entity("Techa.DocumentGenerator.Domain.Entities.DbInfo.StoredProcedureParameter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Alias")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<int?>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("DefaultValue")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsOutParameter")
                        .HasColumnType("bit");

                    b.Property<int?>("MaxLength")
                        .HasColumnType("int");

                    b.Property<string>("MaxValue")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<int?>("MinLength")
                        .HasColumnType("int");

                    b.Property<string>("MinValue")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<int?>("ModifiedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<byte>("NullableOption")
                        .HasColumnType("tinyint");

                    b.Property<string>("ParameterName")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("ParameterType")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<int>("StoredProcedureId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("ModifiedByUserId");

                    b.HasIndex("StoredProcedureId");

                    b.ToTable("StoredProcedureParameters");
                });

            modelBuilder.Entity("Techa.DocumentGenerator.Domain.Entities.DbInfo.StoredProcedureRole", b =>
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

                    b.Property<int>("StoredProcedureId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("ModifiedByUserId");

                    b.HasIndex("RoleId");

                    b.HasIndex("StoredProcedureId");

                    b.ToTable("StoredProcedureRoles");
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

            modelBuilder.Entity("Techa.DocumentGenerator.Domain.Entities.ExecutionArea.ScriptDebugger", b =>
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

                    b.Property<string>("Script")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("ModifiedByUserId");

                    b.HasIndex("UserId");

                    b.ToTable("ScriptDebuggers");
                });

            modelBuilder.Entity("Techa.DocumentGenerator.Domain.Entities.ExecutionArea.ScriptDebuggerDetail", b =>
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

                    b.Property<string>("ResultValue")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ScriptDebuggerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("ModifiedByUserId");

                    b.HasIndex("ScriptDebuggerId");

                    b.ToTable("ScriptDebuggerDetails");
                });

            modelBuilder.Entity("Techa.DocumentGenerator.Domain.Entities.AAA.Role", b =>
                {
                    b.HasOne("Techa.DocumentGenerator.Domain.Entities.AAA.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId");

                    b.HasOne("Techa.DocumentGenerator.Domain.Entities.AAA.User", "ModifiedByUser")
                        .WithMany()
                        .HasForeignKey("ModifiedByUserId");

                    b.HasOne("Techa.DocumentGenerator.Domain.Entities.BaseInfo.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId");

                    b.Navigation("CreatedByUser");

                    b.Navigation("ModifiedByUser");

                    b.Navigation("Project");
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

                    b.HasOne("Techa.DocumentGenerator.Domain.Entities.BaseInfo.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId");

                    b.Navigation("CreatedByUser");

                    b.Navigation("ModifiedByUser");

                    b.Navigation("Profile");

                    b.Navigation("Project");
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

            modelBuilder.Entity("Techa.DocumentGenerator.Domain.Entities.BaseInfo.Project", b =>
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

            modelBuilder.Entity("Techa.DocumentGenerator.Domain.Entities.DbInfo.StoredProcedure", b =>
                {
                    b.HasOne("Techa.DocumentGenerator.Domain.Entities.AAA.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId");

                    b.HasOne("Techa.DocumentGenerator.Domain.Entities.AAA.User", "ModifiedByUser")
                        .WithMany()
                        .HasForeignKey("ModifiedByUserId");

                    b.HasOne("Techa.DocumentGenerator.Domain.Entities.BaseInfo.Project", "Project")
                        .WithMany("StoredProcedures")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CreatedByUser");

                    b.Navigation("ModifiedByUser");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("Techa.DocumentGenerator.Domain.Entities.DbInfo.StoredProcedureParameter", b =>
                {
                    b.HasOne("Techa.DocumentGenerator.Domain.Entities.AAA.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId");

                    b.HasOne("Techa.DocumentGenerator.Domain.Entities.AAA.User", "ModifiedByUser")
                        .WithMany()
                        .HasForeignKey("ModifiedByUserId");

                    b.HasOne("Techa.DocumentGenerator.Domain.Entities.DbInfo.StoredProcedure", "StoredProcedure")
                        .WithMany("StoredProcedureParameters")
                        .HasForeignKey("StoredProcedureId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CreatedByUser");

                    b.Navigation("ModifiedByUser");

                    b.Navigation("StoredProcedure");
                });

            modelBuilder.Entity("Techa.DocumentGenerator.Domain.Entities.DbInfo.StoredProcedureRole", b =>
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

                    b.HasOne("Techa.DocumentGenerator.Domain.Entities.DbInfo.StoredProcedure", "StoredProcedure")
                        .WithMany()
                        .HasForeignKey("StoredProcedureId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CreatedByUser");

                    b.Navigation("ModifiedByUser");

                    b.Navigation("Role");

                    b.Navigation("StoredProcedure");
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

            modelBuilder.Entity("Techa.DocumentGenerator.Domain.Entities.ExecutionArea.ScriptDebugger", b =>
                {
                    b.HasOne("Techa.DocumentGenerator.Domain.Entities.AAA.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId");

                    b.HasOne("Techa.DocumentGenerator.Domain.Entities.AAA.User", "ModifiedByUser")
                        .WithMany()
                        .HasForeignKey("ModifiedByUserId");

                    b.HasOne("Techa.DocumentGenerator.Domain.Entities.AAA.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("CreatedByUser");

                    b.Navigation("ModifiedByUser");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Techa.DocumentGenerator.Domain.Entities.ExecutionArea.ScriptDebuggerDetail", b =>
                {
                    b.HasOne("Techa.DocumentGenerator.Domain.Entities.AAA.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId");

                    b.HasOne("Techa.DocumentGenerator.Domain.Entities.AAA.User", "ModifiedByUser")
                        .WithMany()
                        .HasForeignKey("ModifiedByUserId");

                    b.HasOne("Techa.DocumentGenerator.Domain.Entities.ExecutionArea.ScriptDebugger", "ScriptDebugger")
                        .WithMany("ScriptDebuggerDetails")
                        .HasForeignKey("ScriptDebuggerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CreatedByUser");

                    b.Navigation("ModifiedByUser");

                    b.Navigation("ScriptDebugger");
                });

            modelBuilder.Entity("Techa.DocumentGenerator.Domain.Entities.AAA.User", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Techa.DocumentGenerator.Domain.Entities.BaseInfo.Project", b =>
                {
                    b.Navigation("StoredProcedures");
                });

            modelBuilder.Entity("Techa.DocumentGenerator.Domain.Entities.DbInfo.StoredProcedure", b =>
                {
                    b.Navigation("StoredProcedureParameters");
                });

            modelBuilder.Entity("Techa.DocumentGenerator.Domain.Entities.ExecutionArea.ScriptDebugger", b =>
                {
                    b.Navigation("ScriptDebuggerDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
