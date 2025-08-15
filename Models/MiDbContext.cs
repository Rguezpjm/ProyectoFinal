using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProyectoFinal.Models;

public partial class MiDbContext : DbContext
{
    public MiDbContext()
    {
    }

    public MiDbContext(DbContextOptions<MiDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bodega> Bodegas { get; set; }

    public virtual DbSet<Caja> Cajas { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Empresa> Empresas { get; set; }

    public virtual DbSet<Pai> Pais { get; set; }

    public virtual DbSet<Provincium> Provincia { get; set; }

    public virtual DbSet<Tiposcliente> Tiposclientes { get; set; }

    public virtual DbSet<UserLogin> UserLogins { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=FACTURACION;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bodega>(entity =>
        {
            entity.HasKey(e => e.BodCodigo);

            entity.ToTable("BODEGA");

            entity.Property(e => e.BodCodigo)
                .ValueGeneratedNever()
                .HasColumnName("BOD_CODIGO");
            entity.Property(e => e.BodDescripcion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BOD_DESCRIPCION");
            entity.Property(e => e.BodReferencia)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("BOD_REFERENCIA");
        });

        modelBuilder.Entity<Caja>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CAJA");

            entity.Property(e => e.CajCodigo).HasColumnName("CAJ_CODIGO");
            entity.Property(e => e.CajDescripcion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CAJ_DESCRIPCION");
            entity.Property(e => e.CajReferencia)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("CAJ_REFERENCIA");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CLIENTES");

            entity.Property(e => e.CliApellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CLI_APELLIDO");
            entity.Property(e => e.CliCargo)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("CLI_CARGO");
            entity.Property(e => e.CliCedula)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CLI_CEDULA");
            entity.Property(e => e.CliCelular)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CLI_CELULAR");
            entity.Property(e => e.CliCodigo).HasColumnName("CLI_CODIGO");
            entity.Property(e => e.CliContacto)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CLI_CONTACTO");
            entity.Property(e => e.CliCorreo)
                .HasMaxLength(65)
                .IsUnicode(false)
                .HasColumnName("CLI_CORREO");
            entity.Property(e => e.CliDireccion)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("CLI_DIRECCION");
            entity.Property(e => e.CliNombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CLI_NOMBRE");
            entity.Property(e => e.CliPagina)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("CLI_PAGINA");
            entity.Property(e => e.CliPais)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("CLI_PAIS");
            entity.Property(e => e.CliProvincia)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("CLI_PROVINCIA");
            entity.Property(e => e.CliRnc)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CLI_RNC");
            entity.Property(e => e.CliTelefono)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CLI_TELEFONO");
            entity.Property(e => e.CliTipo)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CLI_TIPO");
        });

        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.HasKey(e => e.EmpCodigo);

            entity.ToTable("EMPRESA");

            entity.Property(e => e.EmpCodigo)
                .ValueGeneratedNever()
                .HasColumnName("EMP_CODIGO");
            entity.Property(e => e.EmpDireccion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMP_DIRECCION");
            entity.Property(e => e.EmpLogo)
                .HasColumnType("image")
                .HasColumnName("EMP_LOGO");
            entity.Property(e => e.EmpNombre)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("EMP_NOMBRE");
            entity.Property(e => e.EmpRnc)
                .HasMaxLength(11)
                .HasColumnName("EMP_RNC");
            entity.Property(e => e.EmpTelefono)
                .HasMaxLength(12)
                .HasColumnName("EMP_TELEFONO");
        });

        modelBuilder.Entity<Pai>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PAIS");

            entity.Property(e => e.IdPais).HasColumnName("ID_PAIS");
            entity.Property(e => e.PaisNombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PAIS_NOMBRE");
        });

        modelBuilder.Entity<Provincium>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PROVINCIA");

            entity.Property(e => e.IdPais).HasColumnName("ID_PAIS");
            entity.Property(e => e.IdProvincia).HasColumnName("ID_PROVINCIA");
            entity.Property(e => e.ProNombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PRO_NOMBRE");
        });

        modelBuilder.Entity<Tiposcliente>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TIPOSCLIENTES");

            entity.Property(e => e.TipCodigo).HasColumnName("TIP_CODIGO");
            entity.Property(e => e.TipDescripcion)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("TIP_DESCRIPCION");
        });

        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.HasKey(e => e.LogCodigo);

            entity.ToTable("USER_LOGIN");

            entity.Property(e => e.LogCodigo)
                .ValueGeneratedNever()
                .HasColumnName("LOG_CODIGO");
            entity.Property(e => e.LogApellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LOG_APELLIDO");
            entity.Property(e => e.LogCargo)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("LOG_CARGO");
            entity.Property(e => e.LogCedula)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("LOG_CEDULA");
            entity.Property(e => e.LogClave)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("LOG_CLAVE");
            entity.Property(e => e.LogCorreo)
                .HasMaxLength(65)
                .IsUnicode(false)
                .HasColumnName("LOG_CORREO");
            entity.Property(e => e.LogDepartamento)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("LOG_DEPARTAMENTO");
            entity.Property(e => e.LogDireccion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LOG_DIRECCION");
            entity.Property(e => e.LogNickname)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("LOG_NICKNAME");
            entity.Property(e => e.LogNombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LOG_NOMBRE");
            entity.Property(e => e.LogPais)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LOG_PAIS");
            entity.Property(e => e.LogProvincia)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("LOG_PROVINCIA");
            entity.Property(e => e.LogStatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("LOG_STATUS");
            entity.Property(e => e.LogTelefono)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("LOG_TELEFONO");
            entity.Property(e => e.LogUsuario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LOG_USUARIO");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
