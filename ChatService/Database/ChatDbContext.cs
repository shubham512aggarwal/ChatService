using ChatService.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Database
{
    public class ChatDbContext: DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
        {
        }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<ChatParticipant> ChatParticipants { get; set; }
        public DbSet<ChatUser> ChatUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChatParticipant>()
                .HasKey(cp => new { cp.ChatRoomId, cp.UserId});

            modelBuilder.Entity<ChatParticipant>()
                .HasOne(cp => cp.ChatRoom)
                .WithMany(cr => cr.Participants)
                .HasForeignKey(cp => cp.ChatRoomId);

            modelBuilder.Entity<ChatRoom>()
                .HasMany(c => c.Messages)
                .WithOne(m => m.ChatRoom)
                .HasForeignKey(m => m.ChatRoomId);

            modelBuilder.Entity<Message>()
                .Property(m => m.Status)
                .HasConversion<int>();

            modelBuilder.Entity<ChatUser>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<ChatUser>()
                .HasMany(c => c.ChatParticipants)
                .WithOne()
                .HasForeignKey(cp => cp.UserId);
        }
    }
}
