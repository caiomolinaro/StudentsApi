﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentsApi.Model;

namespace StudentsApi.Context
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Student>().HasData
            (
                new Student
                {
                    Id = 1,
                    Name = "Caio Molinaro",
                    Email = "cmolinaro19@gmail.com",
                    Age = 22
                },
                new Student
                {
                    Id = 2,
                    Name = "Maria da penha",
                    Email = "mariapenha@gmail,com",
                    Age = 23
                }
            );
        }
    }
}