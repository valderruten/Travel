using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace App_Library.Models;

public partial class DbTravelContext : DbContext
{
    public DbTravelContext()
    {
    }

    public DbTravelContext(DbContextOptions<DbTravelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Autore> Autores { get; set; }

    public virtual DbSet<AutoresHasLibro> AutoresHasLibros { get; set; }

    public virtual DbSet<Editoriale> Editoriales { get; set; }

    public virtual DbSet<Libro> Libros { get; set; }

    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Autore>(entity =>
        {
            entity.ToTable("autores");

            entity.Property(e => e.Apellidos)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("apellidos");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<AutoresHasLibro>(entity =>
        {
            entity.HasKey(e => e.LibrosIsbn);

            entity.ToTable("autores_has_libros");

            entity.Property(e => e.LibrosIsbn)
                .ValueGeneratedNever()
                .HasColumnName("libros_ISBN");
            entity.Property(e => e.AutoresId).HasColumnName("autores_id");

            entity.HasOne(d => d.Autores).WithMany(p => p.AutoresHasLibros)
                .HasForeignKey(d => d.AutoresId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_autores_has_libros_autores");

            entity.HasOne(d => d.LibrosIsbnNavigation).WithOne(p => p.AutoresHasLibro)
                .HasForeignKey<AutoresHasLibro>(d => d.LibrosIsbn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_autores_has_libros_libros");
        });

        modelBuilder.Entity<Editoriale>(entity =>
        {
            entity.ToTable("editoriales");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Sede)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("sede");
        });

        modelBuilder.Entity<Libro>(entity =>
        {
            entity.HasKey(e => e.Isbn);

            entity.ToTable("libros");

            entity.Property(e => e.Isbn)
                .ValueGeneratedNever()
                .HasColumnName("ISBN");
            entity.Property(e => e.EditorialesId).HasColumnName("editoriales_id");
            entity.Property(e => e.NPaginas)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("n_paginas");
            entity.Property(e => e.Sinopsis)
                .HasColumnType("text")
                .HasColumnName("sinopsis");
            entity.Property(e => e.Titulo)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("titulo");

            entity.HasOne(d => d.Editoriales).WithMany(p => p.Libros)
                .HasForeignKey(d => d.EditorialesId)
                .HasConstraintName("FK_libros_editoriales");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
