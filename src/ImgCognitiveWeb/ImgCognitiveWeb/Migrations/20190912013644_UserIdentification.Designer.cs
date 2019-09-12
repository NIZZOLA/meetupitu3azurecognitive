﻿// <auto-generated />
using System;
using ImgCognitiveWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ImgCognitiveWeb.Migrations
{
    [DbContext(typeof(ImageStorageContext))]
    [Migration("20190912013644_UserIdentification")]
    partial class UserIdentification
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ImgCognitiveWeb.Models.Picture", b =>
                {
                    b.Property<int>("PictureId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasMaxLength(200);

                    b.Property<string>("Description")
                        .HasMaxLength(100);

                    b.Property<string>("Result");

                    b.Property<bool>("Status");

                    b.Property<bool>("Storage");

                    b.Property<Guid>("UserId");

                    b.HasKey("PictureId");

                    b.ToTable("Picture");
                });
#pragma warning restore 612, 618
        }
    }
}
