using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PoupaDev.API.Entities;

namespace PoupaDev.API.Persistence
{
    public class PoupaDevContext : DbContext
    {
        
        public DbSet <ObjetivoFinanceiro> Objetivos { get; set; }
        
        public PoupaDevContext(DbContextOptions<PoupaDevContext> options)
        : base(options){}

        protected override void OnModelCreating(ModelBuilder builder){
            builder.Entity<ObjetivoFinanceiro>(e => {
                e.HasKey(of => of.Id);

                e.Property(of => of.ValorObjetivo)
                    .HasColumnType("decimal(18,4)");

                e.HasMany(of => of.Operacoes)
                    .WithOne()
                    .HasForeignKey(op => op.IdObjetivo)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Operacao>(o => {
                 o.HasKey(op => op.Id); 

                 o.Property(op => op.Valor).HasColumnType("decimal(18,4)");
            });
        }

    }
}