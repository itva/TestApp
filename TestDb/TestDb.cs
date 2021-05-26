using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TestDbModel.Tables;

namespace TestDbModel
{
    public class TestDb : DbContext
    {
        public DbSet<Subdivision> Subdivisions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)=> optionsBuilder.UseSqlite(@"Data Source=.\Db\Test.db");
    }
}
