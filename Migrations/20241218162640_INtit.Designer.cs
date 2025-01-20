﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using api.Data;

#nullable disable

namespace api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241218162640_INtit")]
    partial class INtit
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("api.Models.Code", b =>
                {
                    b.Property<int>("submissionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("language")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("source")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("submissionId");

                    b.ToTable("Codes");
                });

            modelBuilder.Entity("api.Models.ProblemStruct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("problemDescription")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("problemName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Problems");
                });

            modelBuilder.Entity("api.Models.Submission", b =>
                {
                    b.Property<int>("submissionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("accepted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("language")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("problemId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("source")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("userId")
                        .HasColumnType("INTEGER");

                    b.HasKey("submissionId");

                    b.ToTable("Submissions");
                });

            modelBuilder.Entity("api.Models.TestCase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ProblemStructId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("expectedOutput")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("input")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ProblemStructId");

                    b.ToTable("TestCase");
                });

            modelBuilder.Entity("api.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("isAdmin")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("api.Models.TestCase", b =>
                {
                    b.HasOne("api.Models.ProblemStruct", null)
                        .WithMany("TestCases")
                        .HasForeignKey("ProblemStructId");
                });

            modelBuilder.Entity("api.Models.ProblemStruct", b =>
                {
                    b.Navigation("TestCases");
                });
#pragma warning restore 612, 618
        }
    }
}
