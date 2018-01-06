using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DAL.Models
{
    public partial class TalkBackDBContext : DbContext
    {
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<User> User { get; set; }

        public TalkBackDBContext(DbContextOptions<TalkBackDBContext> options) : base(options)
        {

        }

        public static TalkBackDBContext getInstance(string connection)
        {
            DbContextOptions<TalkBackDBContext> options = new DbContextOptionsBuilder<TalkBackDBContext>().UseSqlServer(connection).Options;
            return new TalkBackDBContext(options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ReceiverName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.SenderName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Time).HasColumnType("datetime");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Name);

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(20);
            });
        }
    }
}
