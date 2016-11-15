using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;

namespace MProjectWeb.Models.Sqlite
{
    public partial class MProjectDeskSQLITEContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //options.UseSqlite(@"data source=C:\Users\admi\Desktop\Trabajo de grado\PROGRAMMING\Project.Management\MProjectWEB\MProjectWeb\src\MProjectWeb\Models\MProjectDeskSQLITE.sqlite");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<actividades>(entity =>
            {
                entity.HasKey(e => e.id_actividad);

                entity.Property(e => e.descripcion).HasColumnType("VARCHAR");

                entity.Property(e => e.nombre)
                    .IsRequired()
                    .HasColumnType("VARCHAR");

                entity.HasOne(d => d.id_usuarioNavigation).WithMany(p => p.actividades).HasForeignKey(d => d.id_usuario);
            });

            modelBuilder.Entity<archivos>(entity =>
            {
                entity.HasKey(e => e.id_archivo);

                entity.Property(e => e.contenido).HasColumnType("VARCHAR");

                entity.Property(e => e.fecha_carga)
                    .IsRequired()
                    .HasColumnType("DATE");

                entity.Property(e => e.nombre)
                    .IsRequired()
                    .HasColumnType("VARCHAR");

                entity.HasOne(d => d.id_caracteristicaNavigation).WithMany(p => p.archivos).HasForeignKey(d => d.id_caracteristica).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.id_tipo_archivoNavigation).WithMany(p => p.archivos).HasForeignKey(d => d.id_tipo_archivo).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.id_usuarioNavigation).WithMany(p => p.archivos).HasForeignKey(d => d.id_usuario);
            });

            modelBuilder.Entity<caracteristicas>(entity =>
            {
                entity.HasKey(e => e.id_caracteristica);

                entity.Property(e => e.estado).HasColumnType("VARCHAR");

                entity.Property(e => e.fecha_fin).HasColumnType("date");

                entity.Property(e => e.fecha_inicio).HasColumnType("DATE");

                entity.HasOne(d => d.id_actividadNavigation).WithMany(p => p.caracteristicas).HasForeignKey(d => d.id_actividad);

                entity.HasOne(d => d.id_proyectoNavigation).WithMany(p => p.caracteristicas).HasForeignKey(d => d.id_proyecto);

                entity.HasOne(d => d.id_usuarioNavigation).WithMany(p => p.caracteristicas).HasForeignKey(d => d.id_usuario);

                entity.HasOne(d => d.padre_caracteristicaNavigation).WithMany(p => p.Inversepadre_caracteristicaNavigation).HasForeignKey(d => d.padre_caracteristica);

                entity.HasOne(d => d.proyecto_padreNavigation).WithMany(p => p.caracteristicasNavigation).HasForeignKey(d => d.proyecto_padre);
            });

            modelBuilder.Entity<meta_datos>(entity =>
            {
                entity.HasKey(e => e.id_meta_datos);

                entity.Property(e => e.descripcion)
                    .IsRequired()
                    .HasColumnType("VARCHAR");

                entity.Property(e => e.meta_dato_ir)
                    .IsRequired()
                    .HasColumnType("BOOLEAN");

                entity.HasOne(d => d.id_tipo_datoNavigation).WithMany(p => p.meta_datos).HasForeignKey(d => d.id_tipo_dato).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<plantillas>(entity =>
            {
                entity.HasKey(e => e.id_plantilla);

                entity.Property(e => e.descripcion)
                    .IsRequired()
                    .HasColumnType("VARCHAR");

                entity.Property(e => e.nombre)
                    .IsRequired()
                    .HasColumnType("VARCHAR");
            });

            modelBuilder.Entity<plantillas_meta_datos>(entity =>
            {
                entity.HasKey(e => e.id_plantilla_meta_dato);

                entity.Property(e => e.requerido)
                    .IsRequired()
                    .HasColumnType("BOOLEAN");

                entity.HasOne(d => d.id_meta_datosNavigation).WithMany(p => p.plantillas_meta_datos).HasForeignKey(d => d.id_meta_datos).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.id_plantillaNavigation).WithMany(p => p.plantillas_meta_datos).HasForeignKey(d => d.id_plantilla).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<proyectos>(entity =>
            {
                entity.HasKey(e => e.id_proyecto);

                entity.Property(e => e.IR_proyecto)
                    .IsRequired()
                    .HasColumnType("BOOLEAN");

                entity.HasOne(d => d.id_plantillaNavigation).WithMany(p => p.proyectos).HasForeignKey(d => d.id_plantilla).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.id_repositorioNavigation).WithMany(p => p.proyectos).HasForeignKey(d => d.id_repositorio).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.id_usuarioNavigation).WithMany(p => p.proyectos).HasForeignKey(d => d.id_usuario);
            });

            modelBuilder.Entity<proyectos_meta_datos>(entity =>
            {
                entity.HasKey(e => e.id_proyecto_meta_dato);

                entity.Property(e => e.valor)
                    .IsRequired()
                    .HasColumnType("VARCHAR");

                entity.HasOne(d => d.id_plantilla_meta_datoNavigation).WithMany(p => p.proyectos_meta_datos).HasForeignKey(d => d.id_plantilla_meta_dato).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.id_proyectoNavigation).WithMany(p => p.proyectos_meta_datos).HasForeignKey(d => d.id_proyecto).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<repositorio>(entity =>
            {
                entity.HasKey(e => e.id_repositorio);

                entity.Property(e => e.descripcion)
                    .IsRequired()
                    .HasColumnType("VARCHAR");

                entity.Property(e => e.ruta_proyecto)
                    .IsRequired()
                    .HasColumnType("VARCHAR");
            });

            modelBuilder.Entity<tipos_archivos>(entity =>
            {
                entity.HasKey(e => e.id_tipo_archivo);

                entity.Property(e => e.nombre)
                    .IsRequired()
                    .HasColumnType("VARCHAR");
            });

            modelBuilder.Entity<tipos_datos>(entity =>
            {
                entity.HasKey(e => e.id_tipo_dato);

                entity.Property(e => e.decripcion)
                    .IsRequired()
                    .HasColumnType("VARCHAR");
            });

            modelBuilder.Entity<tipos_usuarios>(entity =>
            {
                entity.HasKey(e => e.id_tipo_usu);

                entity.Property(e => e.nombre).IsRequired();
            });

            modelBuilder.Entity<usuarios>(entity =>
            {
                entity.HasKey(e => e.id_usuario);

                entity.Property(e => e.apellido)
                    .IsRequired()
                    .HasColumnType("VARCHAR");

                entity.Property(e => e.e_mail)
                    .IsRequired()
                    .HasColumnType("VARCHAR");

                entity.Property(e => e.nombre)
                    .IsRequired()
                    .HasColumnType("VARCHAR");

                entity.Property(e => e.pass)
                    .IsRequired()
                    .HasColumnType("VARCHAR");
            });

            modelBuilder.Entity<usuarios_tipo_usuarios>(entity =>
            {
                entity.HasKey(e => new { e.id_usuario, e.id_tipo_usu });

                entity.HasOne(d => d.id_tipo_usuNavigation).WithMany(p => p.usuarios_tipo_usuarios).HasForeignKey(d => d.id_tipo_usu).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.id_usuarioNavigation).WithMany(p => p.usuarios_tipo_usuarios).HasForeignKey(d => d.id_usuario).OnDelete(DeleteBehavior.Restrict);
            });
        }

        public virtual DbSet<actividades> actividades { get; set; }
        public virtual DbSet<archivos> archivos { get; set; }
        public virtual DbSet<caracteristicas> caracteristicas { get; set; }
        public virtual DbSet<meta_datos> meta_datos { get; set; }
        public virtual DbSet<plantillas> plantillas { get; set; }
        public virtual DbSet<plantillas_meta_datos> plantillas_meta_datos { get; set; }
        public virtual DbSet<proyectos> proyectos { get; set; }
        public virtual DbSet<proyectos_meta_datos> proyectos_meta_datos { get; set; }
        public virtual DbSet<repositorio> repositorio { get; set; }
        public virtual DbSet<tipos_archivos> tipos_archivos { get; set; }
        public virtual DbSet<tipos_datos> tipos_datos { get; set; }
        public virtual DbSet<tipos_usuarios> tipos_usuarios { get; set; }
        public virtual DbSet<usuarios> usuarios { get; set; }
        public virtual DbSet<usuarios_tipo_usuarios> usuarios_tipo_usuarios { get; set; }
    }
}