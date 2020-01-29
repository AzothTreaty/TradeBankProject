using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeBank3.Models;

namespace TradeBank3.DBContext
{
    public class TradeBankContext : DbContext
    {
        public TradeBankContext(DbContextOptions<TradeBankContext> options) :base(options)
        {

        }

       

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
        //entities
        public DbSet<Baseline> Baseline { get; set; }
        public DbSet<UserInput> UserInput { get; set; }

    }
}
