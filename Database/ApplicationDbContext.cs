using Microsoft.EntityFrameworkCore;
using sports_up_backend.Models;

namespace sports_up_backend.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Lobby> Lobbies { get; set; }
        public DbSet<LobbyPlayer> LobbyPlayers { get; set; }
        public DbSet<PlayerRating> PlayerRatings { get; set; }
        public DbSet<Message> Messages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Lobby>()
                .HasOne(l => l.Owner)
                .WithMany(u => u.OwnedLobbies)
                .HasForeignKey(l => l.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LobbyPlayer>()
                .HasOne(lp => lp.User)
                .WithMany(u => u.LobbyPlayers)
                .HasForeignKey(lp => lp.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LobbyPlayer>()
                .HasOne(lp => lp.Lobby)
                .WithMany(l => l.LobbyPlayers)
                .HasForeignKey(lp => lp.LobbyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PlayerRating>()
                .HasOne(pr => pr.RatedByUser)
                .WithMany(u => u.RatingsGiven)
                .HasForeignKey(pr => pr.RatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PlayerRating>()
                .HasOne(pr => pr.RatedUser)
                .WithMany(u => u.RatingsReceived)
                .HasForeignKey(pr => pr.RatedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Lobby)
                .WithMany(l => l.Messages)
                .HasForeignKey(m => m.LobbyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
