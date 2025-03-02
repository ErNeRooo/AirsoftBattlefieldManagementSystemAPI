﻿// <auto-generated />
using System;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AirsoftBattlefieldManagementSystemAPI.Migrations
{
    [DbContext(typeof(BattleManagementSystemDbContext))]
    [Migration("20250301104337_CreateDeathTableAndRoomTable")]
    partial class CreateDeathTableAndRoomTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Entities.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(320)
                        .HasColumnType("nvarchar(320)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("AccountId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Entities.Battle", b =>
                {
                    b.Property<int>("BattleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BattleId"));

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.HasKey("BattleId");

                    b.HasIndex("RoomId");

                    b.ToTable("Battles");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Entities.Death", b =>
                {
                    b.Property<int>("DeathId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DeathId"));

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.HasKey("DeathId");

                    b.HasIndex("LocationId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Death");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Entities.Kill", b =>
                {
                    b.Property<int>("KillId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("KillId"));

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.HasKey("KillId");

                    b.HasIndex("LocationId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Kills");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Entities.Location", b =>
                {
                    b.Property<int>("LocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LocationId"));

                    b.Property<short>("Accuracy")
                        .HasColumnType("smallint");

                    b.Property<short>("Bearing")
                        .HasColumnType("smallint");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.HasKey("LocationId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Entities.Player", b =>
                {
                    b.Property<int>("PlayerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PlayerId"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDead")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("TeamId")
                        .HasColumnType("int");

                    b.HasKey("PlayerId");

                    b.HasIndex("AccountId");

                    b.HasIndex("TeamId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Entities.PlayerLocation", b =>
                {
                    b.Property<int>("PlayerLocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PlayerLocationId"));

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.HasKey("PlayerLocationId");

                    b.HasIndex("LocationId");

                    b.HasIndex("PlayerId");

                    b.ToTable("PlayerLocations");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Entities.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Room");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Entities.Team", b =>
                {
                    b.Property<int>("TeamId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TeamId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.HasKey("TeamId");

                    b.HasIndex("RoomId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Entities.Battle", b =>
                {
                    b.HasOne("AirsoftBattlefieldManagementSystemAPI.Entities.Room", "Room")
                        .WithMany("Battles")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Entities.Death", b =>
                {
                    b.HasOne("AirsoftBattlefieldManagementSystemAPI.Entities.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AirsoftBattlefieldManagementSystemAPI.Entities.Player", "Player")
                        .WithMany("Deaths")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Entities.Kill", b =>
                {
                    b.HasOne("AirsoftBattlefieldManagementSystemAPI.Entities.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AirsoftBattlefieldManagementSystemAPI.Entities.Player", "Player")
                        .WithMany("Kills")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Entities.Player", b =>
                {
                    b.HasOne("AirsoftBattlefieldManagementSystemAPI.Entities.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AirsoftBattlefieldManagementSystemAPI.Entities.Team", "Team")
                        .WithMany("Players")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Entities.PlayerLocation", b =>
                {
                    b.HasOne("AirsoftBattlefieldManagementSystemAPI.Entities.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AirsoftBattlefieldManagementSystemAPI.Entities.Player", "Player")
                        .WithMany("PlayerLocations")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Entities.Team", b =>
                {
                    b.HasOne("AirsoftBattlefieldManagementSystemAPI.Entities.Room", "Room")
                        .WithMany("Teams")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Entities.Player", b =>
                {
                    b.Navigation("Deaths");

                    b.Navigation("Kills");

                    b.Navigation("PlayerLocations");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Entities.Room", b =>
                {
                    b.Navigation("Battles");

                    b.Navigation("Teams");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Entities.Team", b =>
                {
                    b.Navigation("Players");
                });
#pragma warning restore 612, 618
        }
    }
}
