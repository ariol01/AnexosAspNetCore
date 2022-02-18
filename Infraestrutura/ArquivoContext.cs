using Microsoft.EntityFrameworkCore;
using DSAnexoDocumentoProjeto.Entidades;
namespace DSAnexoDocumentoProjeto.Infraestrutura
{
    public class ArquivoContext : DbContext
    {
        public ArquivoContext(DbContextOptions<ArquivoContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=SQL5086.site4now.net;Initial Catalog=db_a83110_anexodb;User Id=db_a83110_anexodb_admin;Password=arioldba123");

        }

        public DbSet<AnexoDeDocumento> Anexos { get; set; }
    }
}
