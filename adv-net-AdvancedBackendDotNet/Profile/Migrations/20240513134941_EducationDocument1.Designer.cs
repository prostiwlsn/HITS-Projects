﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Profile.Data;

#nullable disable

namespace Profile.Migrations
{
    [DbContext(typeof(ProfileDbContext))]
    [Migration("20240513134941_EducationDocument1")]
    partial class EducationDocument1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Profile.Data.Models.Document", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id", "UserId");

                    b.ToTable("Document");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Profile.Data.Models.DocumentFile", b =>
                {
                    b.Property<Guid>("FileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("DocumentId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("DocumentUserId")
                        .HasColumnType("uuid");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("FileId");

                    b.HasIndex("DocumentId", "DocumentUserId");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("Profile.Data.Models.ProfileInformation", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Citizenship")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("EducationDocumentId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("EducationDocumentUserId")
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("PassportId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PassportUserId")
                        .HasColumnType("uuid");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SecondName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.HasIndex("EducationDocumentId", "EducationDocumentUserId");

                    b.HasIndex("PassportId", "PassportUserId");

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("Profile.Data.Models.EducationDocument", b =>
                {
                    b.HasBaseType("Profile.Data.Models.Document");

                    b.Property<Guid>("DocumentTypeId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int[]>("NextEducationLevels")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.ToTable("EducationDocuments");
                });

            modelBuilder.Entity("Profile.Data.Models.Passport", b =>
                {
                    b.HasBaseType("Profile.Data.Models.Document");

                    b.Property<string>("BirthPlace")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateOnly>("GivenDate")
                        .HasColumnType("date");

                    b.Property<string>("GivenPlace")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("SeriesNumber")
                        .HasColumnType("integer");

                    b.ToTable("Passports");
                });

            modelBuilder.Entity("Profile.Data.Models.DocumentFile", b =>
                {
                    b.HasOne("Profile.Data.Models.Document", "Document")
                        .WithMany("Files")
                        .HasForeignKey("DocumentId", "DocumentUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Document");
                });

            modelBuilder.Entity("Profile.Data.Models.ProfileInformation", b =>
                {
                    b.HasOne("Profile.Data.Models.EducationDocument", "EducationDocument")
                        .WithMany()
                        .HasForeignKey("EducationDocumentId", "EducationDocumentUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Profile.Data.Models.Passport", "Passport")
                        .WithMany()
                        .HasForeignKey("PassportId", "PassportUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EducationDocument");

                    b.Navigation("Passport");
                });

            modelBuilder.Entity("Profile.Data.Models.EducationDocument", b =>
                {
                    b.HasOne("Profile.Data.Models.Document", null)
                        .WithOne()
                        .HasForeignKey("Profile.Data.Models.EducationDocument", "Id", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Profile.Data.Models.Passport", b =>
                {
                    b.HasOne("Profile.Data.Models.Document", null)
                        .WithOne()
                        .HasForeignKey("Profile.Data.Models.Passport", "Id", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Profile.Data.Models.Document", b =>
                {
                    b.Navigation("Files");
                });
#pragma warning restore 612, 618
        }
    }
}
