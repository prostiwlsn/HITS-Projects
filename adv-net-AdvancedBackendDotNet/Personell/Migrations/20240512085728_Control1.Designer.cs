﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Personell.Data;

#nullable disable

namespace Personell.Migrations
{
    [DbContext(typeof(PersonellDbContext))]
    [Migration("20240512085728_Control1")]
    partial class Control1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Personell.Data.Models.Application", b =>
                {
                    b.Property<Guid>("ApplicationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ManagerUserId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SecondName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ApplicationId");

                    b.HasIndex("ManagerUserId");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("Personell.Data.Models.Personell", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AppointerId")
                        .HasColumnType("uuid");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("character varying(13)");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("Roles")
                        .HasColumnType("integer");

                    b.Property<string>("SecondName")
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.ToTable("Personell");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Personell");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Personell.Data.Models.Admin", b =>
                {
                    b.HasBaseType("Personell.Data.Models.Personell");

                    b.HasDiscriminator().HasValue("Admin");
                });

            modelBuilder.Entity("Personell.Data.Models.MainManager", b =>
                {
                    b.HasBaseType("Personell.Data.Models.Personell");

                    b.HasDiscriminator().HasValue("MainManager");
                });

            modelBuilder.Entity("Personell.Data.Models.Manager", b =>
                {
                    b.HasBaseType("Personell.Data.Models.Personell");

                    b.Property<Guid>("FacultyId")
                        .HasColumnType("uuid");

                    b.HasDiscriminator().HasValue("Manager");
                });

            modelBuilder.Entity("Personell.Data.Models.Application", b =>
                {
                    b.HasOne("Personell.Data.Models.Manager", "Manager")
                        .WithMany("Applications")
                        .HasForeignKey("ManagerUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("Personell.Data.Models.Manager", b =>
                {
                    b.Navigation("Applications");
                });
#pragma warning restore 612, 618
        }
    }
}
