using Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class EventsDbContext : IdentityDbContext
    {
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        //public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Place> Places { get; set; }
        public EventsDbContext(DbContextOptions options) : base(options)
        {
		}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Image>().Property(i => i.Title)
                                        .HasMaxLength(200)
                                        .IsRequired(false);

            modelBuilder.Entity<Image>().Property(i => i.Path)
                                        .IsRequired(true);

            OnUserCreating(modelBuilder);
            OnCommentCreating(modelBuilder);
            OnEventCreating(modelBuilder);
            OnTypeCreating(modelBuilder);
            OnPlaceCreating(modelBuilder);
        }


        private void OnCountryCreating(ModelBuilder modelBuilder)
        {
			modelBuilder.Entity<Country>().HasMany(c=>c.Regions)
                                          .WithOne(r=>r.country)
		}
        private void OnUserCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.Entity<User>().Property(u => u.Name)
            //.HasMaxLength(100)
            //.IsRequired(true);

            //modelBuilder.Entity<User>().Property(u => u.Email)
            //.HasMaxLength(200)
            //.IsRequired(true);

            //modelBuilder.Entity<User>().Property(u => u.Password)
            //.HasMaxLength(50)
            //.IsRequired(true);


            modelBuilder.Entity<User>().HasOne(u => u.Avatar)
                                       .WithOne(i => i.User)
                                       .HasForeignKey<Image>(i => i.UserId)
                                       .IsRequired(false);


            modelBuilder.Entity<User>().HasMany(u => u.CreatedComments)
                                       .WithOne(c => c.Owner)
                                       .HasForeignKey(c => c.OwnerId);

            modelBuilder.Entity<User>().HasMany(u => u.LikedComments)
                                       .WithMany(c => c.LikedUsers);

            modelBuilder.Entity<User>().HasMany(u => u.DislikedComments)
                                       .WithMany(c => c.DislikedUsers);


            modelBuilder.Entity<User>().HasMany(u => u.LikedEvents)
                                       .WithMany(e => e.LikedUsers);

            modelBuilder.Entity<User>().HasMany(u => u.FavoriteEvents)
                                       .WithMany(e => e.FavoriteUsers);
        }

        private void OnCommentCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>().HasOne(c => c.Parent)
                                          .WithMany(c => c.SubComments)
                                          .HasForeignKey(c => c.ParentId)
                                          .IsRequired(false)
                                          .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Comment>().HasOne(c => c.Place)
                                          .WithMany(p => p.Comments)
                                          .HasForeignKey(c => c.PlaceId)
                                          .IsRequired(false);

            modelBuilder.Entity<Comment>().HasOne(c => c.Event)
                                          .WithMany(e => e.Comments)
                                          .HasForeignKey(c => c.EventId)
                                          .IsRequired(false);
        }

        private void OnEventCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().HasOne(e => e.Place)
                                        .WithMany(p => p.Events)
                                        .HasForeignKey(e => e.PlaceId)
                                        .IsRequired(false);

            modelBuilder.Entity<Event>().HasMany(e => e.Types)
                                        .WithMany(et => et.Events);

            modelBuilder.Entity<Event>().HasMany(e => e.Images)
                                        .WithOne(i => i.Event)
                                        .HasForeignKey(i => i.EventId)
                                        .IsRequired(false)
                                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Event>().HasOne(e => e.Owner)
                                       .WithMany(c => c.CreatedEvents)
                                       .HasForeignKey(c => c.OwnerId);
        }

        private void OnTypeCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Genre>().HasMany(c => c.Parents)
                                            .WithMany(c => c.SubTypes);

            modelBuilder.Entity<Genre>().HasMany(et => et.Places)
                                            .WithMany(p => p.Types);
        }

        private void OnPlaceCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Place>().HasOne(p => p.Owner)
                                        .WithMany(u => u.CreatedPlaces)
                                        .HasForeignKey(p => p.OwnerId)
                                        .IsRequired(true);

            modelBuilder.Entity<Place>().HasMany(p => p.LikedUsers)
                                        .WithMany(u => u.LikedPlaces);

            modelBuilder.Entity<Place>().HasMany(p => p.FavoriteUsers)
                                        .WithMany(u => u.FavoritePlaces);

            modelBuilder.Entity<Place>().HasMany(p => p.Images)
                                        .WithOne(i => i.Place)
                                        .HasForeignKey(i => i.PlaceId)
                                        .IsRequired(false)
                                        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
