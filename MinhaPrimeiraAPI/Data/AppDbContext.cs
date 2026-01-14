using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MinhaPrimeiraAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MinhaPrimeiraAPI.Data
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlServer("Server=localhost;Database=GymTrackerDb;Trusted_Connection=True;TrustServerCertificate=True;");

        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Routine> Routines { get; set; }
        public DbSet<Muscle> Muscles { get; set; }
        public DbSet<ExerciseMuscle> ExerciseMuscles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.Muscles)
                .WithMany()
                .UsingEntity<ExerciseMuscle>(
                    r => r.HasOne<Muscle>(e => e.Muscle).WithMany(e => e.ExerciseMuscles),
                    l => l.HasOne<Exercise>(e => e.Exercise).WithMany(e => e.ExerciseMuscles));

            modelBuilder.Entity<ExerciseMuscleRoutine>(entity =>
            {
                entity.HasOne(emr => emr.Routine)
                      .WithMany(r => r.ExerciseMuscleRoutine)
                      .HasForeignKey(emr => emr.RoutineId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(emr => emr.ExerciseMuscle)
                      .WithMany()
                      .HasForeignKey(emr => emr.ExerciseMuscleId);
            });


            modelBuilder.Entity<User>()
                .HasMany(u => u.Exercises)
                .WithOne(e => e.User)
                .HasForeignKey(u => u.UserId)
                .IsRequired();
        }

    }
}
