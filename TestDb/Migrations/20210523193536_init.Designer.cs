﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TestDbModel;

namespace TestDbModel.Migrations
{
    [DbContext(typeof(TestDb))]
    [Migration("20210523193536_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.15");

            modelBuilder.Entity("TestDbModel.Tables.Subdivision", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<long?>("ParentId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("dbo.Subdivisions");
                });

            modelBuilder.Entity("TestDbModel.Tables.Subdivision", b =>
                {
                    b.HasOne("TestDbModel.Tables.Subdivision", "Parent")
                        .WithMany("Child")
                        .HasForeignKey("ParentId");
                });
#pragma warning restore 612, 618
        }
    }
}
