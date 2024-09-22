﻿// <auto-generated />
using System;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccess.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20240921071747_Added Phonenumber to Usermodel")]
    partial class AddedPhonenumbertoUsermodel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("InvitationModelRole", b =>
                {
                    b.Property<Guid>("InvitationModelId")
                        .HasColumnType("char(36)");

                    b.Property<string>("RolesName")
                        .HasColumnType("varchar(255)");

                    b.HasKey("InvitationModelId", "RolesName");

                    b.HasIndex("RolesName");

                    b.ToTable("InvitationModelRole");
                });

            modelBuilder.Entity("Model.Invitation.InvitationModel", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("AcceptedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("AcceptedById")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("CreatedById")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("AcceptedById");

                    b.HasIndex("CreatedById");

                    b.ToTable("InvitationModels");
                });

            modelBuilder.Entity("Model.Organization.OrganizationModel", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("Model.Reservation.ReservationModel", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<DateOnly>("EndDateInclusive")
                        .HasColumnType("date");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("ReservationCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("ReservationMadeByUserId")
                        .HasColumnType("char(36)");

                    b.Property<DateOnly>("StartDateInclusive")
                        .HasColumnType("date");

                    b.Property<Guid>("VehicleReservedId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("ReservationMadeByUserId");

                    b.HasIndex("VehicleReservedId");

                    b.ToTable("ReservationModels");
                });

            modelBuilder.Entity("Model.ReservationStatus.ReservationStatusModel", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ReservationId")
                        .HasColumnType("char(36)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("StatusChanged")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("StatusChangedByUserId")
                        .HasColumnType("char(36)");

                    b.Property<string>("StatusReason")
                        .HasMaxLength(512)
                        .HasColumnType("varchar(512)");

                    b.HasKey("Id");

                    b.HasIndex("ReservationId");

                    b.HasIndex("StatusChangedByUserId");

                    b.ToTable("ReservationStatusModels");
                });

            modelBuilder.Entity("Model.Roles.Role", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Name");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Model.User.UserModel", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<string>("AuthId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("OrganizationId")
                        .HasColumnType("char(36)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Model.Vehicle.VehicleModel", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("VehicleModels");
                });

            modelBuilder.Entity("OrganizationModelUserModel", b =>
                {
                    b.Property<Guid>("AdminsId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("OrganizationModelId")
                        .HasColumnType("char(36)");

                    b.HasKey("AdminsId", "OrganizationModelId");

                    b.HasIndex("OrganizationModelId");

                    b.ToTable("OrganizationModelUserModel");
                });

            modelBuilder.Entity("RoleUserModel", b =>
                {
                    b.Property<string>("RolesName")
                        .HasColumnType("varchar(255)");

                    b.Property<Guid>("UsersId")
                        .HasColumnType("char(36)");

                    b.HasKey("RolesName", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("RoleUserModel");
                });

            modelBuilder.Entity("InvitationModelRole", b =>
                {
                    b.HasOne("Model.Invitation.InvitationModel", null)
                        .WithMany()
                        .HasForeignKey("InvitationModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Model.Roles.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Model.Invitation.InvitationModel", b =>
                {
                    b.HasOne("Model.User.UserModel", "AcceptedBy")
                        .WithMany()
                        .HasForeignKey("AcceptedById");

                    b.HasOne("Model.User.UserModel", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.Navigation("AcceptedBy");

                    b.Navigation("CreatedBy");
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

            modelBuilder.Entity("Model.ReservationStatus.ReservationStatusModel", b =>
                {
                    b.HasOne("Model.Reservation.ReservationModel", "Reservation")
                        .WithMany("ReservationStatusChanges")
                        .HasForeignKey("ReservationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Model.User.UserModel", "StatusChangedByUser")
                        .WithMany("ReservationStatusChanges")
                        .HasForeignKey("StatusChangedByUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reservation");

                    b.Navigation("StatusChangedByUser");
                });

            modelBuilder.Entity("Model.User.UserModel", b =>
                {
                    b.HasOne("Model.Organization.OrganizationModel", "Organization")
                        .WithMany("Users")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("OrganizationModelUserModel", b =>
                {
                    b.HasOne("Model.User.UserModel", null)
                        .WithMany()
                        .HasForeignKey("AdminsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Model.Organization.OrganizationModel", null)
                        .WithMany()
                        .HasForeignKey("OrganizationModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RoleUserModel", b =>
                {
                    b.HasOne("Model.Roles.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Model.User.UserModel", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Model.Organization.OrganizationModel", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Model.Reservation.ReservationModel", b =>
                {
                    b.Navigation("ReservationStatusChanges");
                });

            modelBuilder.Entity("Model.User.UserModel", b =>
                {
                    b.Navigation("ReservationStatusChanges");

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