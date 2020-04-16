using HyppeDesafio.Models;
using Microsoft.EntityFrameworkCore;

namespace HyppeDesafio.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<EventUser>()
                .HasOne(pt => pt.Event)
                .WithMany(p => p.EventUsers)
                .HasForeignKey(pt => pt.EventId);

            modelBuilder.Entity<EventUser>()
                .HasOne(pt => pt.User)
                .WithMany(t => t.EventUsers)
                .HasForeignKey(pt => pt.UserId);

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<EventUser> EventUsers { get; set; }



    }
}