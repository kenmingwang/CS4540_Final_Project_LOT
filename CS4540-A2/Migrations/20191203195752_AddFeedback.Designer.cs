﻿// <auto-generated />
using System;
using CS4540_A2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CS4540A2.Migrations.LOS
{
    [DbContext(typeof(LOSContext))]
    [Migration("20191203195752_AddFeedback")]
    partial class AddFeedback
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CS4540_A2.Models.AssignmentFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte[]>("Content");

                    b.Property<int>("LearningOutcomeLId");

                    b.Property<long>("Size");

                    b.Property<string>("UntrustedName");

                    b.Property<DateTime>("UploadDT");

                    b.HasKey("Id");

                    b.HasIndex("LearningOutcomeLId");

                    b.ToTable("SyllabusFile");
                });

            modelBuilder.Entity("CS4540_A2.Models.Course", b =>
                {
                    b.Property<int>("CId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Dept")
                        .IsRequired()
                        .HasMaxLength(6);

                    b.Property<string>("Description");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Number");

                    b.Property<string>("Semester")
                        .IsRequired()
                        .HasMaxLength(2);

                    b.Property<int>("Year");

                    b.HasKey("CId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("CS4540_A2.Models.CourseNote", b =>
                {
                    b.Property<int>("CNId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CourseCId");

                    b.Property<bool>("IsApproved");

                    b.Property<DateTime>("PostDate");

                    b.Property<string>("ProfessorFullName")
                        .IsRequired();

                    b.Property<string>("Text")
                        .HasMaxLength(350);

                    b.HasKey("CNId");

                    b.HasIndex("CourseCId");

                    b.ToTable("CourseNotes");
                });

            modelBuilder.Entity("CS4540_A2.Models.ExamplesFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte[]>("Content");

                    b.Property<bool>("IsAverage");

                    b.Property<bool>("IsBad");

                    b.Property<bool>("IsGood");

                    b.Property<int>("LearningOutcomeLId");

                    b.Property<long>("Size");

                    b.Property<string>("UntrustedName");

                    b.Property<DateTime>("UploadDT");

                    b.HasKey("Id");

                    b.HasIndex("LearningOutcomeLId");

                    b.ToTable("ExamplesFile");
                });

            modelBuilder.Entity("CS4540_A2.Models.Feedback", b =>
                {
                    b.Property<int>("fId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CourseEffectiveRate");

                    b.Property<int>("CourseObjMetRate");

                    b.Property<int>("CourseOrganizedRate");

                    b.Property<int>("CourseOverallRate");

                    b.Property<int>("cId");

                    b.HasKey("fId");

                    b.HasIndex("cId");

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("CS4540_A2.Models.LOSNote", b =>
                {
                    b.Property<int>("LNId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsProfessorNote");

                    b.Property<int>("LearningOutcomeLId");

                    b.Property<DateTime>("PostDate");

                    b.Property<string>("Text")
                        .HasMaxLength(350);

                    b.HasKey("LNId");

                    b.HasIndex("LearningOutcomeLId");

                    b.ToTable("LOSNotes");
                });

            modelBuilder.Entity("CS4540_A2.Models.LearningOutcome", b =>
                {
                    b.Property<int>("LId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CourseCId");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("LId");

                    b.HasIndex("CourseCId");

                    b.ToTable("LOS");
                });

            modelBuilder.Entity("CS4540_A2.Models.AssignmentFile", b =>
                {
                    b.HasOne("CS4540_A2.Models.LearningOutcome", "LO")
                        .WithMany()
                        .HasForeignKey("LearningOutcomeLId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CS4540_A2.Models.CourseNote", b =>
                {
                    b.HasOne("CS4540_A2.Models.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseCId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CS4540_A2.Models.ExamplesFile", b =>
                {
                    b.HasOne("CS4540_A2.Models.LearningOutcome", "LO")
                        .WithMany()
                        .HasForeignKey("LearningOutcomeLId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CS4540_A2.Models.Feedback", b =>
                {
                    b.HasOne("CS4540_A2.Models.Course", "Course")
                        .WithMany()
                        .HasForeignKey("cId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CS4540_A2.Models.LOSNote", b =>
                {
                    b.HasOne("CS4540_A2.Models.LearningOutcome", "LO")
                        .WithMany()
                        .HasForeignKey("LearningOutcomeLId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CS4540_A2.Models.LearningOutcome", b =>
                {
                    b.HasOne("CS4540_A2.Models.Course", "Course")
                        .WithMany("LOS")
                        .HasForeignKey("CourseCId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}