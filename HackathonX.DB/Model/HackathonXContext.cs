using Microsoft.EntityFrameworkCore;

namespace HackathonX.DB.Model
{
    public partial class HackathonXContext : DbContext
    {
        public HackathonXContext()
        {
        }

        public HackathonXContext(DbContextOptions<HackathonXContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Answer> Answers { get; set; } = null!;
        public virtual DbSet<Leaderboard> Leaderboards { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=HackathonX;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>(entity =>
            {
                entity.Property(e => e.Text).HasMaxLength(255);

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Answers_Questions");
            });

            modelBuilder.Entity<Leaderboard>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.Timestamp })
                    .HasName("PK_Leaderboard_1");

                entity.ToTable("Leaderboard");

                entity.Property(e => e.Timestamp)
                    .HasDefaultValue(DateTime.Now);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Leaderboards)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Leaderboard_Users");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.Text).HasMaxLength(255);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id);

                entity.Property(e => e.Name).HasMaxLength(30);

                entity.HasIndex(e => e.Name).IsUnique();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
