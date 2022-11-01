using Microsoft.EntityFrameworkCore;
using tech_test_payment_api.Entities;

namespace tech_test_payment_api.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
        public DbSet<Vendedor> Vendedores { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<Item> Itens { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Venda>(tabela =>
            {
                tabela.HasKey(e => e.Id);
                tabela.HasMany(e => e.ItensVendidos)
                    .WithOne()
                    .HasForeignKey(e => e.VendaId);

            });

            builder.Entity<Vendedor>(tabela =>
            {
                tabela.HasKey(v => v.Id);
                
            });

            


        }
    }
}