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

        public DbSet<ExercicioModel> Exercicios { get; set; }

        public DbSet<UserModel> Users { get; set; }

        public DbSet<RoutinesModel> Routines { get; set; }
    }
}
