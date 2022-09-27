﻿// <auto-generated />
using System;
using Bridge.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Bridge.Infrastructure.Migrations.Bridge.Postgresql
{
    [DbContext(typeof(BridgeContext))]
    partial class BridgeContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Categories")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ContactNumber")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Places", (string)null);
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

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("PlaceId")
                        .HasColumnType("bigint");

                    b.Property<decimal?>("Price")
                        .HasPrecision(18, 4)
                        .HasColumnType("numeric(18,4)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PlaceId");

                    b.ToTable("Products", (string)null);
                });

            modelBuilder.Entity("Bridge.Domain.Users.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("IdentityUserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Bridge.Domain.Places.Entities.Place", b =>
                {
                    b.OwnsOne("Bridge.Domain.Places.Entities.Place.Location#Bridge.Domain.Common.ValueObjects.PlaceLocation", "Location", b1 =>
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

                            b1.ToTable("Places", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("PlaceId");
                        });

                    b.OwnsMany("Bridge.Domain.Places.Entities.Place.OpeningTimes#Bridge.Domain.Places.Entities.OpeningTime", "OpeningTimes", b1 =>
                        {
                            b1.Property<long>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("bigint");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<long>("Id"));

                            b1.Property<TimeSpan?>("BreakEndTime")
                                .HasColumnType("interval");

                            b1.Property<TimeSpan?>("BreakStartTime")
                                .HasColumnType("interval");

                            b1.Property<TimeSpan>("CloseTime")
                                .HasColumnType("interval");

                            b1.Property<string>("Day")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<TimeSpan>("OpenTime")
                                .HasColumnType("interval");

                            b1.Property<long>("PlaceId")
                                .HasColumnType("bigint");

                            b1.HasKey("Id");

                            b1.HasIndex("PlaceId");

                            b1.ToTable("OpeningTime", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("PlaceId");
                        });

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
