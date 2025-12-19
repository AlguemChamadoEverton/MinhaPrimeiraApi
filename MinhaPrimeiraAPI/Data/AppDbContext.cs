using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MinhaPrimeiraAPI.Models;
using System;
using System.Collections.Generic;

namespace MinhaPrimeiraAPI.Data
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlServer("Server=localhost;Database=GymTrackerDb;Trusted_Connection=True;TrustServerCertificate=True;");

        public DbSet<ExerciseModel> Exercises { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<RoutineModel> Routines { get; set; }
        public DbSet<MuscleModel> Muscles { get; set; }
        public DbSet<ExerciseMuscle> ExerciseMuscles { get; set; }
    }
}
