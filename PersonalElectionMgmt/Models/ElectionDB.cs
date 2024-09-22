using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
namespace PersonalElectionMgmt.Models
{
    public class ElectionDB : DbContext
    {
        public DbSet<Citizen> Citizens { get; set; }
        public DbSet<Idea> Ideas { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public string connstr;

        public ElectionDB(string connstr)
        {
            this.connstr = connstr;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(connstr);
    }
}
