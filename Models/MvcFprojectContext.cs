using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace samadApp.Models{
    public class MvcFprojectContext:IdentityDbContext<Users,Role,string>
    {
        public MvcFprojectContext(DbContextOptions<MvcFprojectContext> options):base(options){}
        public DbSet<Sell> Sell{get;set;}
        public DbSet<Price> Price{get;set;}
    }
}