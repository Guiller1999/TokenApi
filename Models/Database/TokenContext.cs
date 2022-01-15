using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace TokenApi.Models
{
    public partial class TokenContext : DbContext
    {
        public TokenContext()
        {
        }

        public TokenContext(DbContextOptions<TokenContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblPersona> TblPersonas { get; set; }
        public virtual DbSet<TblRol> TblRols { get; set; }
        public virtual DbSet<TblRolUsuario> TblRolUsuarios { get; set; }
        public virtual DbSet<TblUsuario> TblUsuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<TblPersona>(entity =>
            {
                entity.ToTable("tbl_persona");

                entity.Property(e => e.Apellidos)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("apellidos");

                entity.Property(e => e.Cedula)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cedula");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<TblRol>(entity =>
            {
                entity.ToTable("tbl_rol");

                entity.Property(e => e.Rol)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("rol");
            });

            modelBuilder.Entity<TblRolUsuario>(entity =>
            {
                entity.ToTable("tbl_Rol_Usuario");

                entity.Property(e => e.IdRol).HasColumnName("Id_Rol");

                entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.TblRolUsuarios)
                    .HasForeignKey(d => d.IdRol)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_Rol_Usuario_tbl_rol");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.TblRolUsuarios)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_Rol_Usuario_tbl_usuario");
            });

            modelBuilder.Entity<TblUsuario>(entity =>
            {
                entity.ToTable("tbl_usuario");

                entity.Property(e => e.IdPersona).HasColumnName("Id_Persona");

                entity.Property(e => e.Password)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.HasOne(d => d.IdPersonaNavigation)
                    .WithMany(p => p.TblUsuarios)
                    .HasForeignKey(d => d.IdPersona)
                    .HasConstraintName("FK_tbl_usuario_tbl_persona");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
