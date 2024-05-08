﻿// <auto-generated />
using System;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccess.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20240428084355_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.4");

            modelBuilder.Entity("Model.Reservation.ReservationModel", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("FromDateInclusive")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ReservationMadeByUserId")
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("ToDateInclusive")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("VehicleReservedId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ReservationMadeByUserId");

                    b.HasIndex("VehicleReservedId");

                    b.ToTable("ReservationModels");
                });

            modelBuilder.Entity("Model.User.UserModel", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Organization")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("UserModels");
                });

            modelBuilder.Entity("Model.Vehicle.VehicleModel", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("VehicleModels");
                });

            modelBuilder.Entity("Model.Reservation.ReservationModel", b =>
                {
                    b.HasOne("Model.User.UserModel", "ReservationMadeByUser")
                        .WithMany("ReservationsMadeByUser")
                        .HasForeignKey("ReservationMadeByUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Model.Vehicle.VehicleModel", "VehicleReserved")
                        .WithMany("Reservations")
                        .HasForeignKey("VehicleReservedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ReservationMadeByUser");

                    b.Navigation("VehicleReserved");
                });

            modelBuilder.Entity("Model.User.UserModel", b =>
                {
                    b.Navigation("ReservationsMadeByUser");
                });

            modelBuilder.Entity("Model.Vehicle.VehicleModel", b =>
                {
                    b.Navigation("Reservations");
                });
#pragma warning restore 612, 618
        }
    }
}
