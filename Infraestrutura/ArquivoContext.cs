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
            optionsBuilder.UseSqlServer(@"");

        }

        public DbSet<AnexoDeDocumento> Anexos { get; set; }
    }
}
