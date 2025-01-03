﻿// <auto-generated />
using System;
using Ecology.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ecology.Data.Migrations
{
    [DbContext(typeof(WebDbContext))]
    [Migration("20241211185032_AddChatMessage")]
    partial class AddChatMessage
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Ecology.Data.Models.ChatMessageData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ChatMessages");
                });

            modelBuilder.Entity("Ecology.Data.Models.CommentData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CommentText")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PostId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Ecology.Data.Models.Ecology.EcologyData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ForMainPage")
                        .HasColumnType("integer");

                    b.Property<string>("ImageSrc")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Ecologies");
                });

            modelBuilder.Entity("Ecology.Data.Models.UserData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AvatarUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Language")
                        .HasColumnType("integer");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Ecology.Data.Models.UserEcologyLikesData", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("EcologyDataId")
                        .HasColumnType("integer");

                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "EcologyDataId");

                    b.HasIndex("EcologyDataId");

                    b.ToTable("UserEcologyLikesData");
                });

            modelBuilder.Entity("Ecology.Data.Models.ChatMessageData", b =>
                {
                    b.HasOne("Ecology.Data.Models.UserData", "User")
                        .WithMany("ChatMessages")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Ecology.Data.Models.CommentData", b =>
                {
                    b.HasOne("Ecology.Data.Models.Ecology.EcologyData", "Ecology")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ecology.Data.Models.UserData", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Ecology");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Ecology.Data.Models.Ecology.EcologyData", b =>
                {
                    b.HasOne("Ecology.Data.Models.UserData", "User")
                        .WithMany("Ecologies")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Ecology.Data.Models.UserEcologyLikesData", b =>
                {
                    b.HasOne("Ecology.Data.Models.Ecology.EcologyData", "EcologyData")
                        .WithMany("UsersWhoLikeIt")
                        .HasForeignKey("EcologyDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ecology.Data.Models.UserData", "User")
                        .WithMany("PostsWhichUsersLike")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EcologyData");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Ecology.Data.Models.Ecology.EcologyData", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("UsersWhoLikeIt");
                });

            modelBuilder.Entity("Ecology.Data.Models.UserData", b =>
                {
                    b.Navigation("ChatMessages");

                    b.Navigation("Comments");

                    b.Navigation("Ecologies");

                    b.Navigation("PostsWhichUsersLike");
                });
#pragma warning restore 612, 618
        }
    }
}
