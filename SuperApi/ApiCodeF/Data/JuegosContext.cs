using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApiCodeF;

    public class JuegosContext : DbContext
    {
        public JuegosContext (DbContextOptions<JuegosContext> options)
            : base(options)
        {
            
        }

        public DbSet<ApiCodeF.Game> Game { get; set; } = default!;

    public DbSet<ApiCodeF.Genre> Genre { get; set; } = default!;
    }
