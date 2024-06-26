﻿// <auto-generated />
using System;
using Application.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Application.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240519062755_ManagerName1")]
    partial class ManagerName1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Application.Data.Models.ApplicationInfo", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("EducationDocumentId")
                        .HasColumnType("uuid");

                    b.Property<string>("EducationDocumentName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastChange")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("ManagerId")
                        .HasColumnType("uuid");

                    b.Property<string>("ManagerName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int[]>("NextEducationLevels")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<string>("SecondName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("Application.Data.Models.ChosenProgram", b =>
                {
                    b.Property<Guid>("ApplicationInfoId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProgramId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("FacultyId")
                        .HasColumnType("uuid");

                    b.Property<string>("FacultyName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("Priority")
                        .HasColumnType("bigint");

                    b.Property<string>("ProgramName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ApplicationInfoId", "ProgramId");

                    b.ToTable("ChosenProgram");
                });

            modelBuilder.Entity("Application.Data.Models.ChosenProgram", b =>
                {
                    b.HasOne("Application.Data.Models.ApplicationInfo", "Application")
                        .WithMany("ChosenPrograms")
                        .HasForeignKey("ApplicationInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Application");
                });

            modelBuilder.Entity("Application.Data.Models.ApplicationInfo", b =>
                {
                    b.Navigation("ChosenPrograms");
                });
#pragma warning restore 612, 618
        }
    }
}
