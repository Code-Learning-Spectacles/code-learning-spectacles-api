using System;
using System.Collections.Generic;
using CodeLearningSpectaclesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeLearningSpectaclesAPI.Data;

public partial class CodeLearningDbContext : DbContext
{
    public CodeLearningDbContext()
    {
    }

    public CodeLearningDbContext(DbContextOptions<CodeLearningDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Codeconstruct> Codeconstructs { get; set; }

    public virtual DbSet<Codinglanguage> Codinglanguages { get; set; }

    public virtual DbSet<Constructtype> Constructtypes { get; set; }

    public virtual DbSet<Languageconstruct> Languageconstructs { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<Profilelanguageconstruct> Profilelanguageconstructs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Codeconstruct>(entity =>
        {
            entity.HasKey(e => e.Codeconstructid).HasName("codeconstruct_pkey");

            entity.ToTable("codeconstruct");

            entity.Property(e => e.Codeconstructid).HasColumnName("codeconstructid");
            entity.Property(e => e.Constructtypeid).HasColumnName("constructtypeid");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.HasOne(d => d.Constructtype).WithMany(p => p.Codeconstructs)
                .HasForeignKey(d => d.Constructtypeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("codeconstruct_constructtypeid_fkey");
        });

        modelBuilder.Entity<Codinglanguage>(entity =>
        {
            entity.HasKey(e => e.Codinglanguageid).HasName("codinglanguage_pkey");

            entity.ToTable("codinglanguage");

            entity.Property(e => e.Codinglanguageid).HasColumnName("codinglanguageid");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Constructtype>(entity =>
        {
            entity.HasKey(e => e.Constructtypeid).HasName("constructtype_pkey");

            entity.ToTable("constructtype");

            entity.Property(e => e.Constructtypeid).HasColumnName("constructtypeid");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Languageconstruct>(entity =>
        {
            entity.HasKey(e => e.Languageconstructid).HasName("languageconstruct_pkey");

            entity.ToTable("languageconstruct");

            entity.Property(e => e.Languageconstructid).HasColumnName("languageconstructid");
            entity.Property(e => e.Codeconstructid).HasColumnName("codeconstructid");
            entity.Property(e => e.Codinglanguageid).HasColumnName("codinglanguageid");
            entity.Property(e => e.Construct)
                .HasMaxLength(1000)
                .HasColumnName("construct");

            entity.HasOne(d => d.Codeconstruct).WithMany(p => p.Languageconstructs)
                .HasForeignKey(d => d.Codeconstructid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("languageconstruct_codeconstructid_fkey");

            entity.HasOne(d => d.Codinglanguage).WithMany(p => p.Languageconstructs)
                .HasForeignKey(d => d.Codinglanguageid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("languageconstruct_codinglanguageid_fkey");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.Profileid).HasName("profile_pkey");

            entity.ToTable("profile");

            entity.Property(e => e.Profileid).HasColumnName("profileid");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Profilelanguageconstruct>(entity =>
        {
            entity.HasKey(e => e.Profilelanguageconstructid).HasName("profilelanguageconstruct_pkey");

            entity.ToTable("profilelanguageconstruct");

            entity.Property(e => e.Profilelanguageconstructid).HasColumnName("profilelanguageconstructid");
            entity.Property(e => e.Languageconstructid).HasColumnName("languageconstructid");
            entity.Property(e => e.Notes)
                .HasMaxLength(1000)
                .HasColumnName("notes");
            entity.Property(e => e.Profileid).HasColumnName("profileid");

            entity.HasOne(d => d.Languageconstruct).WithMany(p => p.Profilelanguageconstructs)
                .HasForeignKey(d => d.Languageconstructid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("profilelanguageconstruct_languageconstructid_fkey");

            entity.HasOne(d => d.Profile).WithMany(p => p.Profilelanguageconstructs)
                .HasForeignKey(d => d.Profileid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("profilelanguageconstruct_profileid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
