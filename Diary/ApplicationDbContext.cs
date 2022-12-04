using Diary.Models.Configurations;
using Diary.Models.Domains;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;

namespace Diary
{
    public class ApplicationDbContext : DbContext
    {
        //public ApplicationDbContext()
        //    : base("name=ApplicationDbContext")
        //{
        //}

        public ApplicationDbContext(string connection)
            : base(connection)
        {
            //MessageBox.Show(connection);
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new StudentConfiguration());
            modelBuilder.Configurations.Add(new GroupConfiguration());
            modelBuilder.Configurations.Add(new RatingConfiguration());
        }

    }
}