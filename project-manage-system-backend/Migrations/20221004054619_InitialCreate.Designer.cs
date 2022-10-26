﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using project_manage_system_backend.Shares;

namespace project_manage_system_backend.Migrations
{
    [DbContext(typeof(PMSContext))]
    [Migration("20221004054619_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("project_manage_system_backend.Models.Invitation", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ApplicantAccount")
                        .HasColumnType("TEXT");

                    b.Property<int?>("InvitedProjectID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("InviterAccount")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsAgreed")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("ApplicantAccount");

                    b.HasIndex("InvitedProjectID");

                    b.HasIndex("InviterAccount");

                    b.ToTable("Invitations");
                });

            modelBuilder.Entity("project_manage_system_backend.Models.Jira", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("APIToken")
                        .HasColumnType("TEXT");

                    b.Property<string>("Account")
                        .HasColumnType("TEXT");

                    b.Property<int>("BoardId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DomainURL")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Jiras");
                });

            modelBuilder.Entity("project_manage_system_backend.Models.Project", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("OwnerAccount")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("OwnerAccount");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("project_manage_system_backend.Models.Repo", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AccountColonPw")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsSonarqube")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Owner")
                        .HasColumnType("TEXT");

                    b.Property<int?>("ProjectID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProjectKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("RepoId")
                        .HasColumnType("TEXT");

                    b.Property<string>("SonarqubeUrl")
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("ProjectID");

                    b.ToTable("Repositories");
                });

            modelBuilder.Entity("project_manage_system_backend.Models.User", b =>
                {
                    b.Property<string>("Account")
                        .HasColumnType("TEXT");

                    b.Property<string>("Authority")
                        .HasColumnType("TEXT");

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.HasKey("Account");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("project_manage_system_backend.Models.UserProject", b =>
                {
                    b.Property<int>("ProjectId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Account")
                        .HasColumnType("TEXT");

                    b.HasKey("ProjectId", "Account");

                    b.HasIndex("Account");

                    b.ToTable("UserProject");
                });

            modelBuilder.Entity("project_manage_system_backend.Models.Invitation", b =>
                {
                    b.HasOne("project_manage_system_backend.Models.User", "Applicant")
                        .WithMany()
                        .HasForeignKey("ApplicantAccount");

                    b.HasOne("project_manage_system_backend.Models.Project", "InvitedProject")
                        .WithMany()
                        .HasForeignKey("InvitedProjectID");

                    b.HasOne("project_manage_system_backend.Models.User", "Inviter")
                        .WithMany()
                        .HasForeignKey("InviterAccount");

                    b.Navigation("Applicant");

                    b.Navigation("InvitedProject");

                    b.Navigation("Inviter");
                });

            modelBuilder.Entity("project_manage_system_backend.Models.Project", b =>
                {
                    b.HasOne("project_manage_system_backend.Models.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerAccount");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("project_manage_system_backend.Models.Repo", b =>
                {
                    b.HasOne("project_manage_system_backend.Models.Project", "Project")
                        .WithMany("Repositories")
                        .HasForeignKey("ProjectID");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("project_manage_system_backend.Models.UserProject", b =>
                {
                    b.HasOne("project_manage_system_backend.Models.User", "User")
                        .WithMany("Projects")
                        .HasForeignKey("Account")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("project_manage_system_backend.Models.Project", "Project")
                        .WithMany("Users")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");

                    b.Navigation("User");
                });

            modelBuilder.Entity("project_manage_system_backend.Models.Project", b =>
                {
                    b.Navigation("Repositories");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("project_manage_system_backend.Models.User", b =>
                {
                    b.Navigation("Projects");
                });
#pragma warning restore 612, 618
        }
    }
}
