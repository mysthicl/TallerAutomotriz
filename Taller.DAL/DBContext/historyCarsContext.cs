using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Taller.Entity
{
    public partial class historyCarsContext : DbContext
    {
        public historyCarsContext()
        {
        }

        public historyCarsContext(DbContextOptions<historyCarsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Configuracion> Configuracions { get; set; } = null!;
        public virtual DbSet<Correlativo> Correlativos { get; set; } = null!;
        public virtual DbSet<Menu> Menus { get; set; } = null!;
        public virtual DbSet<RolMenu> RolMenus { get; set; } = null!;
        public virtual DbSet<TblCarro> TblCarros { get; set; } = null!;
        public virtual DbSet<TblCompra> TblCompras { get; set; } = null!;
        public virtual DbSet<TblCotizacion> TblCotizacions { get; set; } = null!;
        public virtual DbSet<TblDetalleCompra> TblDetalleCompras { get; set; } = null!;
        public virtual DbSet<TblDetalleCotizacion> TblDetalleCotizacions { get; set; } = null!;
        public virtual DbSet<TblDetalleVenta> TblDetalleVenta { get; set; } = null!;
        public virtual DbSet<TblHistorialCarro> TblHistorialCarros { get; set; } = null!;
        public virtual DbSet<TblProducto> TblProductos { get; set; } = null!;
        public virtual DbSet<TblProveedor> TblProveedors { get; set; } = null!;
        public virtual DbSet<TblReparacion> TblReparacions { get; set; } = null!;
        public virtual DbSet<TblRol> TblRols { get; set; } = null!;
        public virtual DbSet<TblUsuario> TblUsuarios { get; set; } = null!;
        public virtual DbSet<TblVenta> TblVenta { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Configuracion>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Configuracion");

                entity.Property(e => e.Propiedad)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("propiedad");

                entity.Property(e => e.Recurso)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("recurso");

                entity.Property(e => e.Valor)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("valor");
            });

            modelBuilder.Entity<Correlativo>(entity =>
            {
                entity.HasKey(e => e.IdCorrelativo)
                    .HasName("PK__Correlat__24E65C735AD6735B");

                entity.ToTable("Correlativo");

                entity.Property(e => e.IdCorrelativo).HasColumnName("id_correlativo");

                entity.Property(e => e.CantidadNumero).HasColumnName("cantidad_numero");

                entity.Property(e => e.NumeroCorrelativo).HasColumnName("numero_correlativo");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("tipo");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasKey(e => e.IdMenu)
                    .HasName("PK__Menu__C26AF483EE2438DC");

                entity.ToTable("Menu");

                entity.Property(e => e.IdMenu).HasColumnName("idMenu");

                entity.Property(e => e.Controlador)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("controlador");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.EsActivo).HasColumnName("esActivo");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Icono)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("icono");

                entity.Property(e => e.IdMenuPadre).HasColumnName("idMenuPadre");

                entity.Property(e => e.PaginaAccion)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("paginaAccion");

                entity.HasOne(d => d.IdMenuPadreNavigation)
                    .WithMany(p => p.InverseIdMenuPadreNavigation)
                    .HasForeignKey(d => d.IdMenuPadre)
                    .HasConstraintName("FK__Menu__idMenuPadr__440B1D61");
            });

            modelBuilder.Entity<RolMenu>(entity =>
            {
                entity.HasKey(e => e.IdRolMenu)
                    .HasName("PK__RolMenu__CD2045D8BF7B9A6A");

                entity.ToTable("RolMenu");

                entity.Property(e => e.IdRolMenu).HasColumnName("idRolMenu");

                entity.Property(e => e.EsActivo).HasColumnName("esActivo");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdMenu).HasColumnName("idMenu");

                entity.Property(e => e.IdRol).HasColumnName("idRol");

                entity.HasOne(d => d.IdMenuNavigation)
                    .WithMany(p => p.RolMenus)
                    .HasForeignKey(d => d.IdMenu)
                    .HasConstraintName("FK__RolMenu__idMenu__44FF419A");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.RolMenus)
                    .HasForeignKey(d => d.IdRol)
                    .HasConstraintName("FK__RolMenu__idRol__45F365D3");
            });

            modelBuilder.Entity<TblCarro>(entity =>
            {
                entity.HasKey(e => e.IdCarro)
                    .HasName("PK__tbl_carr__D3C318A13573EC4E");

                entity.ToTable("tbl_carro");

                entity.Property(e => e.IdCarro).HasColumnName("id_carro");

                entity.Property(e => e.Marca)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Modelo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Placa)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("placa");
            });

            modelBuilder.Entity<TblCompra>(entity =>
            {
                entity.HasKey(e => e.IdCompra)
                    .HasName("PK__tbl_comp__C4BAA604BE17EC44");

                entity.ToTable("tbl_compra");

                entity.Property(e => e.IdCompra).HasColumnName("id_compra");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha");

                entity.Property(e => e.IdDetalleCompra).HasColumnName("id_detalle_compra");

                entity.Property(e => e.IdProducto).HasColumnName("id_producto");

                entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.Property(e => e.NumeroCompra)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TotalDeLaCompra)
                    .HasColumnType("money")
                    .HasColumnName("total_de_la_compra");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.TblCompras)
                    .HasForeignKey(d => d.IdProducto)
                    .HasConstraintName("FK__tbl_compr__id_pr__47DBAE45");

                entity.HasOne(d => d.IdProveedorNavigation)
                    .WithMany(p => p.TblCompras)
                    .HasForeignKey(d => d.IdProveedor)
                    .HasConstraintName("FK__tbl_compr__id_pr__46E78A0C");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.TblCompras)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK__tbl_compr__id_us__48CFD27E");
            });

            modelBuilder.Entity<TblCotizacion>(entity =>
            {
                entity.HasKey(e => e.IdCotizacion)
                    .HasName("PK__tbl_coti__9713D1746C582B50");

                entity.ToTable("tbl_cotizacion");

                entity.Property(e => e.IdCotizacion).HasColumnName("id_cotizacion");

                entity.Property(e => e.Cotizacion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("cotizacion");

                entity.Property(e => e.FechaCotizacion)
                    .HasColumnType("date")
                    .HasColumnName("fecha_cotizacion");

                entity.Property(e => e.IdProducto).HasColumnName("id_producto");

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.Property(e => e.NumeroCotizacion)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("numeroCotizacion");

                entity.Property(e => e.TotalDeLaCotizacion)
                    .HasColumnType("money")
                    .HasColumnName("totalDeLaCotizacion");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.TblCotizacions)
                    .HasForeignKey(d => d.IdProducto)
                    .HasConstraintName("FK__tbl_cotiz__id_pr__49C3F6B7");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.TblCotizacions)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK__tbl_cotiz__id_us__4AB81AF0");
            });

            modelBuilder.Entity<TblDetalleCompra>(entity =>
            {
                entity.HasKey(e => e.IdDetalleCompra)
                    .HasName("PK__tbl_deta__BD16E279AFC7300B");

                entity.ToTable("tbl_detalle_compra");

                entity.Property(e => e.IdDetalleCompra).HasColumnName("id_detalle_compra");

                entity.Property(e => e.Cantidad).HasColumnName("cantidad");

                entity.Property(e => e.CantidadCompra).HasColumnName("cantidad_compra");

                entity.Property(e => e.IdCompra).HasColumnName("id_compra");

                entity.Property(e => e.IdProducto).HasColumnName("id_producto");

                entity.Property(e => e.Precio)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("precio");

                entity.Property(e => e.Subtotal)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("subtotal");

                entity.HasOne(d => d.IdCompraNavigation)
                    .WithMany(p => p.TblDetalleCompras)
                    .HasForeignKey(d => d.IdCompra)
                    .HasConstraintName("FK__tbl_detal__id_co__4BAC3F29");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.TblDetalleCompras)
                    .HasForeignKey(d => d.IdProducto)
                    .HasConstraintName("FK__tbl_detal__id_pr__4CA06362");
            });

            modelBuilder.Entity<TblDetalleCotizacion>(entity =>
            {
                entity.HasKey(e => e.IdDetalleCotizacion)
                    .HasName("PK__tbl_deta__F367943697E87454");

                entity.ToTable("tbl_detalle_cotizacion");

                entity.Property(e => e.IdDetalleCotizacion).HasColumnName("id_detalle_cotizacion");

                entity.Property(e => e.Cantidad).HasColumnName("cantidad");

                entity.Property(e => e.IdCotizacion).HasColumnName("id_cotizacion");

                entity.Property(e => e.IdProducto).HasColumnName("id_producto");

                entity.Property(e => e.Precio)
                    .HasColumnType("money")
                    .HasColumnName("precio");

                entity.Property(e => e.Subtotal)
                    .HasColumnType("money")
                    .HasColumnName("subtotal");

                entity.HasOne(d => d.IdCotizacionNavigation)
                    .WithMany(p => p.TblDetalleCotizacions)
                    .HasForeignKey(d => d.IdCotizacion)
                    .HasConstraintName("FK__tbl_detal__id_co__4D94879B");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.TblDetalleCotizacions)
                    .HasForeignKey(d => d.IdProducto)
                    .HasConstraintName("FK__tbl_detal__id_pr__4E88ABD4");
            });

            modelBuilder.Entity<TblDetalleVenta>(entity =>
            {
                entity.HasKey(e => e.IdDetalleVenta)
                    .HasName("PK__tbl_deta__5B265D474FB587C7");

                entity.ToTable("tbl_detalle_venta");

                entity.Property(e => e.IdDetalleVenta).HasColumnName("id_detalle_venta");

                entity.Property(e => e.Cantidad).HasColumnName("cantidad");

                entity.Property(e => e.CantidadVendida).HasColumnName("cantidad_vendida");

                entity.Property(e => e.IdCotizacion).HasColumnName("id_cotizacion");

                entity.Property(e => e.IdProducto).HasColumnName("id_producto");

                entity.Property(e => e.IdVenta).HasColumnName("id_venta");

                entity.Property(e => e.Precio)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("precio");

                entity.Property(e => e.Subtotal)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("subtotal");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.TblDetalleVenta)
                    .HasForeignKey(d => d.IdProducto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tbl_detal__id_pr__4F7CD00D");

                entity.HasOne(d => d.IdVentaNavigation)
                    .WithMany(p => p.TblDetalleVenta)
                    .HasForeignKey(d => d.IdVenta)
                    .HasConstraintName("FK__tbl_detal__id_ve__5070F446");
            });

            modelBuilder.Entity<TblHistorialCarro>(entity =>
            {
                entity.HasKey(e => e.IdHistorialCarro)
                    .HasName("PK__tbl_hist__22CF346D767ABA0B");

                entity.ToTable("tbl_historial_carro");

                entity.Property(e => e.IdHistorialCarro).HasColumnName("id_historial_carro");

                entity.Property(e => e.IdCarro).HasColumnName("id_carro");

                entity.Property(e => e.IdPlaca).HasColumnName("id_placa");

                entity.HasOne(d => d.IdCarroNavigation)
                    .WithMany(p => p.TblHistorialCarros)
                    .HasForeignKey(d => d.IdCarro)
                    .HasConstraintName("FK__tbl_histo__id_ca__5165187F");
            });

            modelBuilder.Entity<TblProducto>(entity =>
            {
                entity.HasKey(e => e.IdProducto)
                    .HasName("PK__tbl_prod__FF341C0DF4BFE9A8");

                entity.ToTable("tbl_producto");

                entity.Property(e => e.IdProducto).HasColumnName("id_producto");

                entity.Property(e => e.CantidadEnStock).HasColumnName("cantidad_en_stock");

                entity.Property(e => e.CodigoProducto)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("codigo_producto");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.NombreImagen)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombreImagen");

                entity.Property(e => e.Precio)
                    .HasColumnType("money")
                    .HasColumnName("precio");

                entity.Property(e => e.UrlImagen)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("urlImagen");

                entity.Property(e => e.Valor)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("valor");
            });

            modelBuilder.Entity<TblProveedor>(entity =>
            {
                entity.HasKey(e => e.IdProveedor)
                    .HasName("PK__tbl_prov__8D3DFE284B1DCD3C");

                entity.ToTable("tbl_proveedor");

                entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");

                entity.Property(e => e.Direccion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("direccion");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.Telefono)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("telefono");
            });

            modelBuilder.Entity<TblReparacion>(entity =>
            {
                entity.HasKey(e => e.IdReparacion)
                    .HasName("PK__tbl_repa__5253371F300AE711");

                entity.ToTable("tbl_reparacion");

                entity.Property(e => e.IdReparacion).HasColumnName("id_reparacion");

                entity.Property(e => e.DescripcionDeLaReparacion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("descripcion_de_la_reparacion");

                entity.Property(e => e.FechaDeFin)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_de_fin");

                entity.Property(e => e.FechaDeInicio)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_de_inicio");

                entity.Property(e => e.IdCotizacion).HasColumnName("id_cotizacion");

                entity.Property(e => e.IdHistorialCarro).HasColumnName("id_historial_carro");

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.Property(e => e.NumberTracking)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("number_tracking");

                entity.Property(e => e.Status)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("status");

                entity.HasOne(d => d.IdCotizacionNavigation)
                    .WithMany(p => p.TblReparacions)
                    .HasForeignKey(d => d.IdCotizacion)
                    .HasConstraintName("FK__tbl_repar__id_co__534D60F1");

                entity.HasOne(d => d.IdHistorialCarroNavigation)
                    .WithMany(p => p.TblReparacions)
                    .HasForeignKey(d => d.IdHistorialCarro)
                    .HasConstraintName("FK__tbl_repar__id_hi__52593CB8");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.TblReparacions)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK__tbl_repar__id_us__5441852A");
            });

            modelBuilder.Entity<TblRol>(entity =>
            {
                entity.HasKey(e => e.IdRol)
                    .HasName("PK__tbl_rol__6ABCB5E0EA54702A");

                entity.ToTable("tbl_rol");

                entity.Property(e => e.IdRol).HasColumnName("id_rol");

                entity.Property(e => e.NombreRol)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("nombre_rol");
            });

            modelBuilder.Entity<TblUsuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PK__tbl_usua__4E3E04ADC53A874F");

                entity.ToTable("tbl_usuario");

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.Property(e => e.Contrasena)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("contrasena");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.IdRol).HasColumnName("id_rol");

                entity.Property(e => e.NombreDeUsuario)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre_de_usuario");

                entity.Property(e => e.NombreFoto)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UrlFoto)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.TblUsuarios)
                    .HasForeignKey(d => d.IdRol)
                    .HasConstraintName("FK__tbl_usuar__id_ro__5535A963");
            });

            modelBuilder.Entity<TblVenta>(entity =>
            {
                entity.HasKey(e => e.IdVenta)
                    .HasName("PK__tbl_vent__459533BF7B9B9786");

                entity.ToTable("tbl_venta");

                entity.Property(e => e.IdVenta).HasColumnName("id_venta");

                entity.Property(e => e.Direccion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("direccion");

                entity.Property(e => e.DocumentoCliente)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("documentoCliente");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha");

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.Property(e => e.NombreCliente)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("nombreCliente");

                entity.Property(e => e.NumeroVenta)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("numeroVenta");

                entity.Property(e => e.Telefono).HasColumnName("telefono");

                entity.Property(e => e.TotalDeLaVenta)
                    .HasColumnType("money")
                    .HasColumnName("total_de_la_venta");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.TblVenta)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK__tbl_venta__id_us__5629CD9C");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
