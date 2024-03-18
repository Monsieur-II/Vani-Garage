﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Vani.Infras;

#nullable disable

namespace Vani.Infras.Migrations.VaniDb
{
    [DbContext(typeof(VaniDbContext))]
    [Migration("20240317163213_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Vani.Domain.Models.Car", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DateTimeCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("LastModifiedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("MakeId")
                        .HasColumnType("int");

                    b.Property<int>("Mileage")
                        .HasColumnType("int");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MakeId");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("Vani.Domain.Models.Make", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Makes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Country = "USA",
                            Name = "Ford"
                        },
                        new
                        {
                            Id = 2,
                            Country = "USA",
                            Name = "Chevrolet"
                        },
                        new
                        {
                            Id = 3,
                            Country = "Japan",
                            Name = "Toyota"
                        },
                        new
                        {
                            Id = 4,
                            Country = "Japan",
                            Name = "Nissan"
                        },
                        new
                        {
                            Id = 5,
                            Country = "Japan",
                            Name = "Honda"
                        },
                        new
                        {
                            Id = 6,
                            Country = "Germany",
                            Name = "BMW"
                        });
                });

            modelBuilder.Entity("Vani.Domain.Models.Car", b =>
                {
                    b.HasOne("Vani.Domain.Models.Make", "Make")
                        .WithMany("Cars")
                        .HasForeignKey("MakeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Make");
                });

            modelBuilder.Entity("Vani.Domain.Models.Make", b =>
                {
                    b.Navigation("Cars");
                });
#pragma warning restore 612, 618
        }
    }
}