﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TicTic.Data;

namespace TicTic.Migrations
{
    [DbContext(typeof(TicTicContext))]
    partial class TicTicContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TicTic.Data.GameData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("cellCode")
                        .HasColumnType("int");

                    b.Property<string>("cellFiller")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("roomDataId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("roomDataId");

                    b.ToTable("GameData");
                });

            modelBuilder.Entity("TicTic.Data.RoomData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("HostId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsHostReady")
                        .HasColumnType("bit");

                    b.Property<bool>("IsJoinerReady")
                        .HasColumnType("bit");

                    b.Property<string>("JoinerId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoomNo")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("RoomData");
                });

            modelBuilder.Entity("TicTic.Data.GameData", b =>
                {
                    b.HasOne("TicTic.Data.RoomData", "roomData")
                        .WithMany("GameData")
                        .HasForeignKey("roomDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("roomData");
                });

            modelBuilder.Entity("TicTic.Data.RoomData", b =>
                {
                    b.Navigation("GameData");
                });
#pragma warning restore 612, 618
        }
    }
}
