using Microsoft.EntityFrameworkCore;

namespace AppModels.kurumi {
    public partial class kurumiContext : DbContext {
        public kurumiContext() { }

        public kurumiContext(DbContextOptions<kurumiContext> options) : base(options) { }

        public virtual DbSet<TaskGroup> TaskGroup { get; set; }
        public virtual DbSet<Tasks> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<TaskGroup>(entity => {
                entity.HasKey(e => e.GroupId);

                entity.ToTable("TASK_GROUP", "kurumi");

                entity.Property(e => e.GroupId)
                    .HasColumnName("GROUP_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Tasks>(entity => {
                entity.HasKey(e => e.TaskId);

                entity.ToTable("TASKS", "kurumi");

                entity.Property(e => e.TaskId)
                    .HasColumnName("TASK_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Content)
                    .HasColumnName("CONTENT")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.GroupId)
                    .HasColumnName("GROUP_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Period)
                    .HasColumnName("PERIOD")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Pic)
                    .HasColumnName("PIC")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasColumnType("int(11)");
            });
        }
    }
}