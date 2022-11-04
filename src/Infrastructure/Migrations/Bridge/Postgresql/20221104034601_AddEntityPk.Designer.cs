﻿// <auto-generated />
using System;
using Bridge.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Bridge.Infrastructure.Migrations.Bridge.Postgresql
{
    [DbContext(typeof(BridgeContext))]
    [Migration("20221104034601_AddEntityPk")]
    partial class AddEntityPk
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Bridge.Domain.Places.Entities.Place", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Categories")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ContactNumber")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationDateTimeUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("IdTemp")
                        .HasColumnType("uuid");

                    b.Property<string>("ImagePath")
                        .HasColumnType("text");

                    b.Property<DateTime?>("LastUpdateDateTimeUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Places");
                });

            modelBuilder.Entity("Bridge.Domain.Products.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Categories")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("IdTemp")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PlaceId")
                        .HasColumnType("integer");

                    b.Property<Guid>("PlaceIdTemp")
                        .HasColumnType("uuid");

                    b.Property<decimal?>("Price")
                        .HasPrecision(18, 4)
                        .HasColumnType("numeric(18,4)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PlaceId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Bridge.Domain.Places.Entities.Places.Restroom", b =>
                {
                    b.HasBaseType("Bridge.Domain.Places.Entities.Place");

                    b.Property<string>("DiaperTableLocation")
                        .HasColumnType("text");

                    b.Property<int?>("FemaleDisabledToilet")
                        .HasColumnType("integer");

                    b.Property<int?>("FemaleKidToilet")
                        .HasColumnType("integer");

                    b.Property<int?>("FemaleToilet")
                        .HasColumnType("integer");

                    b.Property<bool>("IsUnisex")
                        .HasColumnType("boolean");

                    b.Property<int?>("MaleDisabledToilet")
                        .HasColumnType("integer");

                    b.Property<int?>("MaleDisabledUrinal")
                        .HasColumnType("integer");

                    b.Property<int?>("MaleKidToilet")
                        .HasColumnType("integer");

                    b.Property<int?>("MaleKidUrinal")
                        .HasColumnType("integer");

                    b.Property<int?>("MaleToilet")
                        .HasColumnType("integer");

                    b.Property<int?>("MaleUrinal")
                        .HasColumnType("integer");

                    b.ToTable("Restrooms", (string)null);
                });

            modelBuilder.Entity("Bridge.Domain.Places.Entities.Place", b =>
                {
                    b.OwnsOne("Bridge.Domain.Common.ValueObjects.Address", "Address", b1 =>
                        {
                            b1.Property<int>("PlaceId")
                                .HasColumnType("integer");

                            b1.Property<string>("DetailAddress")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("EupMyeonDong")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("JibunAddress")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("PostalCode")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("RoadAddress")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("RoadName")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("SiDo")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("SiGuGun")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("PlaceId");

                            b1.ToTable("Places");

                            b1.WithOwner()
                                .HasForeignKey("PlaceId");
                        });

                    b.OwnsOne("Bridge.Domain.Common.ValueObjects.Location", "Location", b1 =>
                        {
                            b1.Property<int>("PlaceId")
                                .HasColumnType("integer");

                            b1.Property<double>("Easting")
                                .HasColumnType("double precision");

                            b1.Property<double>("Latitude")
                                .HasColumnType("double precision");

                            b1.Property<double>("Longitude")
                                .HasColumnType("double precision");

                            b1.Property<double>("Northing")
                                .HasColumnType("double precision");

                            b1.HasKey("PlaceId");

                            b1.ToTable("Places");

                            b1.WithOwner()
                                .HasForeignKey("PlaceId");
                        });

                    b.OwnsMany("Bridge.Domain.Places.Entities.OpeningTime", "OpeningTimes", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<TimeSpan?>("BreakEndTime")
                                .HasColumnType("interval");

                            b1.Property<TimeSpan?>("BreakStartTime")
                                .HasColumnType("interval");

                            b1.Property<TimeSpan?>("CloseTime")
                                .HasColumnType("interval");

                            b1.Property<string>("Day")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<Guid>("IdTemp")
                                .HasColumnType("uuid");

                            b1.Property<bool>("Is24Hours")
                                .HasColumnType("boolean");

                            b1.Property<bool>("IsDayoff")
                                .HasColumnType("boolean");

                            b1.Property<TimeSpan?>("OpenTime")
                                .HasColumnType("interval");

                            b1.Property<int>("PlaceId")
                                .HasColumnType("integer");

                            b1.HasKey("Id");

                            b1.HasIndex("PlaceId");

                            b1.ToTable("OpeningTime");

                            b1.WithOwner()
                                .HasForeignKey("PlaceId");
                        });

                    b.Navigation("Address")
                        .IsRequired();

                    b.Navigation("Location")
                        .IsRequired();

                    b.Navigation("OpeningTimes");
                });

            modelBuilder.Entity("Bridge.Domain.Products.Entities.Product", b =>
                {
                    b.HasOne("Bridge.Domain.Places.Entities.Place", "Place")
                        .WithMany()
                        .HasForeignKey("PlaceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Place");
                });

            modelBuilder.Entity("Bridge.Domain.Places.Entities.Places.Restroom", b =>
                {
                    b.HasOne("Bridge.Domain.Places.Entities.Place", null)
                        .WithOne()
                        .HasForeignKey("Bridge.Domain.Places.Entities.Places.Restroom", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
