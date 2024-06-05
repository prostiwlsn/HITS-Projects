﻿// <auto-generated />
using System;
using Dictionary.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Dictionary.Migrations
{
    [DbContext(typeof(DictionaryDbContext))]
    [Migration("20240417103251_Init1")]
    partial class Init1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Dictionary.Data.Models.DocumentType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("EducationLevelId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EducationLevelId");

                    b.ToTable("DocumentTypes");
                });

            modelBuilder.Entity("Dictionary.Data.Models.EducationLevel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<Guid?>("DocumentTypeId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DocumentTypeId");

                    b.ToTable("EducationLevels");
                });

            modelBuilder.Entity("Dictionary.Data.Models.Faculty", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Faculties");
                });

            modelBuilder.Entity("Dictionary.Data.Models.Program", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("EducationForm")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("EducationLevelId")
                        .HasColumnType("integer");

                    b.Property<Guid>("FacultyId")
                        .HasColumnType("uuid");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("EducationLevelId");

                    b.HasIndex("FacultyId");

                    b.ToTable("Programs");
                });

            modelBuilder.Entity("Dictionary.Data.Models.DocumentType", b =>
                {
                    b.HasOne("Dictionary.Data.Models.EducationLevel", "EducationLevel")
                        .WithMany()
                        .HasForeignKey("EducationLevelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EducationLevel");
                });

            modelBuilder.Entity("Dictionary.Data.Models.EducationLevel", b =>
                {
                    b.HasOne("Dictionary.Data.Models.DocumentType", null)
                        .WithMany("EducationLevels")
                        .HasForeignKey("DocumentTypeId");
                });

            modelBuilder.Entity("Dictionary.Data.Models.Program", b =>
                {
                    b.HasOne("Dictionary.Data.Models.EducationLevel", "EducationLevel")
                        .WithMany()
                        .HasForeignKey("EducationLevelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dictionary.Data.Models.Faculty", "Faculty")
                        .WithMany()
                        .HasForeignKey("FacultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EducationLevel");

                    b.Navigation("Faculty");
                });

            modelBuilder.Entity("Dictionary.Data.Models.DocumentType", b =>
                {
                    b.Navigation("EducationLevels");
                });
#pragma warning restore 612, 618
        }
    }
}
