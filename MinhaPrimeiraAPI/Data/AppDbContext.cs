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
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseNpgsql("Host=localhost;Username=berto;Password=1234;Database=GymTrackerDb;");

        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Routine> Routines { get; set; }
        public DbSet<Muscle> Muscles { get; set; }
        public DbSet<ExerciseMuscle> ExerciseMuscles { get; set; }
        public DbSet<ExerciseRoutine> ExerciseRoutines { get; set; }
        public DbSet<Set> Sets { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.Muscles)
                .WithMany()
                .UsingEntity<ExerciseMuscle>(
                    r => r.HasOne<Muscle>(e => e.Muscle).WithMany(e => e.ExerciseMuscles),
                    l => l.HasOne<Exercise>(e => e.Exercise).WithMany(e => e.ExerciseMuscles));

            modelBuilder.Entity<Routine>(entity =>
            {
                entity.HasMany(r => r.Exercises)
                     .WithMany(e => e.Routines)
                     .UsingEntity<ExerciseRoutine>(
                        l => l.HasOne<Exercise>(e => e.Exercise).WithMany(e => e.ExerciseRoutines).OnDelete(DeleteBehavior.Restrict),
                        r => r.HasOne<Routine>(e => e.Routine).WithMany(e => e.ExerciseRoutines).OnDelete(DeleteBehavior.Restrict)
                            .OnDelete(DeleteBehavior.Cascade)
                    );
                     //.HasForeignKey(emr => emr.RoutineId)
                     //.OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<User>()
                .HasMany(u => u.Exercises)
                .WithOne(e => e.User)
                .HasForeignKey(u => u.UserId)
                .IsRequired();
        }

    }
}
