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
    [Migration("20221017132317_AddStatusAndCreationTime")]
    partial class AddStatusAndCreationTime
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
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Categories")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ContactNumber")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationDateTime")
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
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Categories")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("PlaceId")
                        .HasColumnType("bigint");

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

            modelBuilder.Entity("Bridge.Domain.Places.Entities.Place", b =>
                {
                    b.OwnsOne("Bridge.Domain.Common.ValueObjects.Address", "Address", b1 =>
                        {
                            b1.Property<long>("PlaceId")
                                .HasColumnType("bigint");

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
                            b1.Property<long>("PlaceId")
                                .HasColumnType("bigint");

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
                            b1.Property<long>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("bigint");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<long>("Id"));

                            b1.Property<TimeSpan?>("BreakEndTime")
                                .HasColumnType("interval");

                            b1.Property<TimeSpan?>("BreakStartTime")
                                .HasColumnType("interval");

                            b1.Property<TimeSpan?>("CloseTime")
                                .HasColumnType("interval");

                            b1.Property<string>("Day")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<bool>("Dayoff")
                                .HasColumnType("boolean");

                            b1.Property<TimeSpan?>("OpenTime")
                                .HasColumnType("interval");

                            b1.Property<long>("PlaceId")
                                .HasColumnType("bigint");

                            b1.Property<bool>("TwentyFourHours")
                                .HasColumnType("boolean");

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
#pragma warning restore 612, 618
        }
    }
}
