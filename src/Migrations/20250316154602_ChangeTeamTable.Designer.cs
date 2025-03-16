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
    [Migration("20250316154602_ChangeTeamTable")]
    partial class ChangeTeamTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Models.Entities.Account", b =>
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

                    b.ToTable("Account");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Models.Entities.Battle", b =>
                {
                    b.Property<int>("BattleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BattleId"));

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.HasKey("BattleId");

                    b.HasIndex("RoomId");

                    b.ToTable("Battle");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Models.Entities.Death", b =>
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

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Models.Entities.Kill", b =>
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

                    b.ToTable("Kill");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Models.Entities.Location", b =>
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

                    b.ToTable("Location");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Models.Entities.Player", b =>
                {
                    b.Property<int>("PlayerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PlayerId"));

                    b.Property<int?>("AccountId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDead")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<int?>("RoomId")
                        .HasColumnType("int");

                    b.Property<int?>("TeamId")
                        .HasColumnType("int");

                    b.HasKey("PlayerId");

                    b.HasIndex("AccountId");

                    b.HasIndex("RoomId");

                    b.HasIndex("TeamId");

                    b.ToTable("Player");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Models.Entities.PlayerLocation", b =>
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

                    b.ToTable("PlayerLocation");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Models.Entities.Room", b =>
                {
                    b.Property<int>("RoomId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoomId"));

                    b.Property<int>("AdminPlayerId")
                        .HasColumnType("int");

                    b.Property<string>("JoinCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MaxPlayers")
                        .HasColumnType("int");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoomId");

                    b.HasIndex("AdminPlayerId");

                    b.ToTable("Room");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Models.Entities.Team", b =>
                {
                    b.Property<int>("TeamId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TeamId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<int>("OfficerPlayerId")
                        .HasColumnType("int");

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.HasKey("TeamId");

                    b.HasIndex("OfficerPlayerId");

                    b.HasIndex("RoomId");

                    b.ToTable("Team");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Models.Entities.Battle", b =>
                {
                    b.HasOne("AirsoftBattlefieldManagementSystemAPI.Models.Entities.Room", "Room")
                        .WithMany("Battles")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Models.Entities.Death", b =>
                {
                    b.HasOne("AirsoftBattlefieldManagementSystemAPI.Models.Entities.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AirsoftBattlefieldManagementSystemAPI.Models.Entities.Player", "Player")
                        .WithMany("Deaths")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Models.Entities.Kill", b =>
                {
                    b.HasOne("AirsoftBattlefieldManagementSystemAPI.Models.Entities.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AirsoftBattlefieldManagementSystemAPI.Models.Entities.Player", "Player")
                        .WithMany("Kills")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Models.Entities.Player", b =>
                {
                    b.HasOne("AirsoftBattlefieldManagementSystemAPI.Models.Entities.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId");

                    b.HasOne("AirsoftBattlefieldManagementSystemAPI.Models.Entities.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId");

                    b.HasOne("AirsoftBattlefieldManagementSystemAPI.Models.Entities.Team", "Team")
                        .WithMany("Players")
                        .HasForeignKey("TeamId");

                    b.Navigation("Account");

                    b.Navigation("Room");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Models.Entities.PlayerLocation", b =>
                {
                    b.HasOne("AirsoftBattlefieldManagementSystemAPI.Models.Entities.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AirsoftBattlefieldManagementSystemAPI.Models.Entities.Player", "Player")
                        .WithMany("PlayerLocations")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Models.Entities.Room", b =>
                {
                    b.HasOne("AirsoftBattlefieldManagementSystemAPI.Models.Entities.Player", "AdminPlayer")
                        .WithMany()
                        .HasForeignKey("AdminPlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AdminPlayer");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Models.Entities.Team", b =>
                {
                    b.HasOne("AirsoftBattlefieldManagementSystemAPI.Models.Entities.Player", "OfficerPlayer")
                        .WithMany()
                        .HasForeignKey("OfficerPlayerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("AirsoftBattlefieldManagementSystemAPI.Models.Entities.Room", "Room")
                        .WithMany("Teams")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OfficerPlayer");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Models.Entities.Player", b =>
                {
                    b.Navigation("Deaths");

                    b.Navigation("Kills");

                    b.Navigation("PlayerLocations");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Models.Entities.Room", b =>
                {
                    b.Navigation("Battles");

                    b.Navigation("Teams");
                });

            modelBuilder.Entity("AirsoftBattlefieldManagementSystemAPI.Models.Entities.Team", b =>
                {
                    b.Navigation("Players");
                });
#pragma warning restore 612, 618
        }
    }
}
