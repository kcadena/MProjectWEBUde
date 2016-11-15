using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;

namespace MProjectWeb.Models.Postgres
{
    public partial class MProjectContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(@"PORT=5432;TIMEOUT=15;POOLING=True;MINPOOLSIZE=1;MAXPOOLSIZE=20;COMMANDTIMEOUT=20;PASSWORD=123;USERID=postgres;HOST=localhost;DATABASE=MProjectPru");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<actividades>(entity =>
            {
                entity.HasKey(e => new { e.keym, e.id_actividad, e.id_usuario });

                entity.Property(e => e.descripcion)
                    .IsRequired()
                    .HasColumnType("varchar");

                entity.Property(e => e.fecha_ultima_modificacion).HasColumnType("varchar");

                entity.Property(e => e.nombre)
                    .IsRequired()
                    .HasColumnType("varchar");

                entity.HasOne(d => d.id_usuarioNavigation).WithMany(p => p.actividades).HasForeignKey(d => d.id_usuario).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.caracteristicas).WithMany(p => p.actividades).HasForeignKey(d => new { d.keym_car, d.id_caracteristica, d.id_usuario_car }).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<archivos>(entity =>
            {
                entity.HasKey(e => new { e.keym, e.id_archivo, e.id_usuario_own });

                entity.Property(e => e.descripcion).HasColumnType("varchar");

                entity.Property(e => e.fecha_carga).HasColumnType("date");

                entity.Property(e => e.fecha_ultima_modificacion).HasColumnType("date");

                entity.Property(e => e.nombre)
                    .IsRequired()
                    .HasColumnType("varchar");

                entity.Property(e => e.subtitulo).HasColumnType("varchar");

                entity.Property(e => e.tipo).HasColumnType("varchar");

                entity.Property(e => e.titulo).HasColumnType("varchar");

                entity.HasOne(d => d.caracteristicas).WithMany(p => p.archivos).HasForeignKey(d => new { d.keym_car, d.id_caracteristica, d.id_usuario_car }).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<caracteristicas>(entity =>
            {
                entity.HasKey(e => new { e.keym, e.id_caracteristica, e.id_usuario });

                entity.Property(e => e.estado)
                    .IsRequired()
                    .HasColumnType("varchar");

                entity.Property(e => e.fecha_fin).HasColumnType("varchar");

                entity.Property(e => e.fecha_inicio).HasColumnType("varchar");

                entity.Property(e => e.fecha_ultima_modificacion).HasColumnType("varchar");

                entity.Property(e => e.porcentaje_asignado).HasDefaultValueSql("0");

                entity.Property(e => e.porcentaje_cumplido).HasDefaultValueSql("0");

                entity.Property(e => e.publicacion_web).HasDefaultValueSql("false");

                entity.Property(e => e.tipo_caracteristica)
                    .IsRequired()
                    .HasMaxLength(1)
                    .HasColumnType("bpchar");

                entity.HasOne(d => d.id_usuarioNavigation).WithMany(p => p.caracteristicas).HasForeignKey(d => d.id_usuario).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.usuario_asignadoNavigation).WithMany(p => p.caracteristicasNavigation).HasForeignKey(d => d.usuario_asignado);

                entity.HasOne(d => d.caracteristicasNavigation).WithMany(p => p.InversecaracteristicasNavigation).HasForeignKey(d => new { d.keym_padre, d.id_caracteristica_padre, d.id_usuario_padre });
            });

            modelBuilder.Entity<costos>(entity =>
            {
                entity.HasKey(e => new { e.keym, e.id_costo, e.id_usuario });

                entity.Property(e => e.nombre)
                    .IsRequired()
                    .HasColumnType("varchar");

                entity.HasOne(d => d.id_usuarioNavigation).WithMany(p => p.costos).HasForeignKey(d => d.id_usuario).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.caracteristicas).WithMany(p => p.costosNavigation).HasForeignKey(d => new { d.keym_car, d.id_caracteristica, d.id_usuario_car }).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<meta_datos>(entity =>
            {
                entity.HasKey(e => new { e.keym, e.id_meta_dato, e.id_usuario });

                entity.Property(e => e.descripcion)
                    .IsRequired()
                    .HasColumnType("varchar");

                entity.Property(e => e.fecha_ultima_modificacion).HasColumnType("date");

                entity.HasOne(d => d.id_tipo_datoNavigation).WithMany(p => p.meta_datos).HasForeignKey(d => d.id_tipo_dato).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.id_usuarioNavigation).WithMany(p => p.meta_datos).HasForeignKey(d => d.id_usuario).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<plantillas>(entity =>
            {
                entity.HasKey(e => new { e.keym, e.id_plantilla, e.id_usuario });

                entity.Property(e => e.descripcion)
                    .IsRequired()
                    .HasColumnType("varchar");

                entity.Property(e => e.fecha_ultima_modificacion).HasColumnType("date");

                entity.Property(e => e.nombre)
                    .IsRequired()
                    .HasColumnType("varchar");

                entity.HasOne(d => d.id_usuarioNavigation).WithMany(p => p.plantillas).HasForeignKey(d => d.id_usuario).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<plantillas_meta_datos>(entity =>
            {
                entity.HasKey(e => new { e.keym, e.id_plantilla_meta_dato, e.id_usuario });

                entity.Property(e => e.fecha_ultima_modificacion).HasColumnType("varchar");

                entity.HasOne(d => d.id_usuarioNavigation).WithMany(p => p.plantillas_meta_datos).HasForeignKey(d => d.id_usuario).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.meta_datos).WithMany(p => p.plantillas_meta_datos).HasForeignKey(d => new { d.keym_met, d.id_meta_dato, d.id_usuario_met }).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.plantillas).WithMany(p => p.plantillas_meta_datos).HasForeignKey(d => new { d.keym_pla, d.id_plantilla, d.id_usuario_pla }).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<presupuesto>(entity =>
            {
                entity.HasKey(e => new { e.keym, e.id_presupuesto, e.id_usuario });

                entity.Property(e => e.nombre)
                    .IsRequired()
                    .HasColumnType("varchar");

                entity.HasOne(d => d.id_usuarioNavigation).WithMany(p => p.presupuesto).HasForeignKey(d => d.id_usuario).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.caracteristicas).WithMany(p => p.presupuestoNavigation).HasForeignKey(d => new { d.keym_car, d.id_caracteristica, d.id_usuario_car }).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<proyectos>(entity =>
            {
                entity.HasKey(e => new { e.keym, e.id_proyecto, e.id_usuario });

                entity.Property(e => e.descripcion)
                    .IsRequired()
                    .HasColumnType("varchar");

                entity.Property(e => e.fecha_ultima_modificacion).HasColumnType("varchar");

                entity.Property(e => e.icon).HasColumnType("varchar");

                entity.Property(e => e.nombre).HasColumnType("varchar");

                entity.Property(e => e.plantilla)
                    .IsRequired()
                    .HasColumnType("varchar");

                entity.HasOne(d => d.id_usuarioNavigation).WithMany(p => p.proyectos).HasForeignKey(d => d.id_usuario).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.caracteristicas).WithMany(p => p.proyectos).HasForeignKey(d => new { d.keym_car, d.id_caracteristica, d.id_usuario_car }).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<proyectos_meta_datos>(entity =>
            {
                entity.HasKey(e => new { e.keym, e.id_proyecto_meta_dato, e.id_usuario });

                entity.Property(e => e.fecha_ultima_modificacion).HasColumnType("varchar");

                entity.Property(e => e.tipo)
                    .IsRequired()
                    .HasColumnType("varchar");

                entity.Property(e => e.valor)
                    .IsRequired()
                    .HasColumnType("varchar");

                entity.HasOne(d => d.id_usuarioNavigation).WithMany(p => p.proyectos_meta_datos).HasForeignKey(d => d.id_usuario).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.proyectos).WithMany(p => p.proyectos_meta_datos).HasForeignKey(d => new { d.keym_pro, d.id_proyecto, d.id_usuario_pro }).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<recursos>(entity =>
            {
                entity.HasKey(e => new { e.keym, e.id_recurso, e.id_usuario });

                entity.Property(e => e.cantidad).HasDefaultValueSql("1");

                entity.Property(e => e.nombre_recurso)
                    .IsRequired()
                    .HasColumnType("varchar");

                entity.HasOne(d => d.id_usuarioNavigation).WithMany(p => p.recursos).HasForeignKey(d => d.id_usuario).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.caracteristicas).WithMany(p => p.recursosNavigation).HasForeignKey(d => new { d.keym_car, d.id_caracteristica, d.id_usuario_car }).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<repositorios_usuarios>(entity =>
            {
                entity.HasKey(e => e.id_usuario);

                entity.Property(e => e.id_usuario).ValueGeneratedNever();

                entity.Property(e => e.ruta_repositorio)
                    .IsRequired()
                    .HasColumnType("varchar");

                entity.HasOne(d => d.id_usuarioNavigation).WithOne(p => p.repositorios_usuarios).HasForeignKey<repositorios_usuarios>(d => d.id_usuario).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<table_sequence>(entity =>
            {
                entity.HasKey(e => e.id_usuario);

                entity.Property(e => e.id_usuario).ValueGeneratedNever();

                entity.Property(e => e.actividades).HasDefaultValueSql("0");

                entity.Property(e => e.archivos).HasDefaultValueSql("0");

                entity.Property(e => e.caracteristicas).HasDefaultValueSql("0");

                entity.Property(e => e.costos).HasDefaultValueSql("0");

                entity.Property(e => e.presuspuesto).HasDefaultValueSql("0");

                entity.Property(e => e.proyectos).HasDefaultValueSql("0");

                entity.Property(e => e.proyectos_meta_datos).HasDefaultValueSql("0");

                entity.Property(e => e.recursos).HasDefaultValueSql("0");

                entity.HasOne(d => d.id_usuarioNavigation).WithOne(p => p.table_sequence).HasForeignKey<table_sequence>(d => d.id_usuario).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<tipos_datos>(entity =>
            {
                entity.HasKey(e => e.id_tipo_dato);

                entity.Property(e => e.id_tipo_dato).ValueGeneratedNever();

                entity.Property(e => e.descripcion)
                    .IsRequired()
                    .HasColumnType("varchar");
            });

            modelBuilder.Entity<usuarios>(entity =>
            {
                entity.HasKey(e => e.id_usuario);

                entity.Property(e => e.id_usuario).ValueGeneratedNever();

                entity.Property(e => e.administrador).HasDefaultValueSql("false");

                entity.Property(e => e.apellido)
                    .IsRequired()
                    .HasColumnType("varchar");

                entity.Property(e => e.cargo).HasColumnType("varchar");

                entity.Property(e => e.disponible).HasDefaultValueSql("false");

                entity.Property(e => e.e_mail)
                    .IsRequired()
                    .HasColumnType("varchar");

                entity.Property(e => e.entidad).HasColumnType("varchar");

                entity.Property(e => e.genero)
                    .IsRequired()
                    .HasMaxLength(1)
                    .HasColumnType("bpchar");

                entity.Property(e => e.imagen).HasColumnType("varchar");

                entity.Property(e => e.nombre)
                    .IsRequired()
                    .HasColumnType("varchar");

                entity.Property(e => e.pass)
                    .IsRequired()
                    .HasColumnType("varchar");

                entity.Property(e => e.telefono).HasColumnType("varchar");
            });
        }

        public virtual DbSet<actividades> actividades { get; set; }
        public virtual DbSet<archivos> archivos { get; set; }
        public virtual DbSet<caracteristicas> caracteristicas { get; set; }
        public virtual DbSet<costos> costos { get; set; }
        public virtual DbSet<meta_datos> meta_datos { get; set; }
        public virtual DbSet<plantillas> plantillas { get; set; }
        public virtual DbSet<plantillas_meta_datos> plantillas_meta_datos { get; set; }
        public virtual DbSet<presupuesto> presupuesto { get; set; }
        public virtual DbSet<proyectos> proyectos { get; set; }
        public virtual DbSet<proyectos_meta_datos> proyectos_meta_datos { get; set; }
        public virtual DbSet<recursos> recursos { get; set; }
        public virtual DbSet<repositorios_usuarios> repositorios_usuarios { get; set; }
        public virtual DbSet<table_sequence> table_sequence { get; set; }
        public virtual DbSet<tipos_datos> tipos_datos { get; set; }
        public virtual DbSet<usuarios> usuarios { get; set; }
    }
}