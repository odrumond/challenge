﻿// <auto-generated />
using System;
using Data.Data.Contexts.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Migrations.Postgres
{
    [DbContext(typeof(PostgresCallCenterDbContext))]
    partial class PostgresCallCenterDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Data.Data.Models.AgentDto", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid[]>("Queues")
                        .IsRequired()
                        .HasColumnType("uuid[]");

                    b.Property<int>("State")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Agents");
                });

            modelBuilder.Entity("Data.Data.Models.EventsDto", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AgentId")
                        .HasColumnType("uuid");

                    b.Property<int>("EventAction")
                        .HasColumnType("integer");

                    b.Property<Guid[]>("Queues")
                        .IsRequired()
                        .HasColumnType("uuid[]");

                    b.Property<DateTime>("TimeStampUtc")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("AgentId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Data.Data.Models.EventsDto", b =>
                {
                    b.HasOne("Data.Data.Models.AgentDto", "Agent")
                        .WithMany("Events")
                        .HasForeignKey("AgentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Agent");
                });

            modelBuilder.Entity("Data.Data.Models.AgentDto", b =>
                {
                    b.Navigation("Events");
                });
#pragma warning restore 612, 618
        }
    }
}
