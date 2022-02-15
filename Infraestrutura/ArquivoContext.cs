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
            optionsBuilder.UseSqlServer(@"Data Source=anexo-documento-db.c5au5rghvszj.sa-east-1.rds.amazonaws.com;Initial Catalog=UploadDB;User=Admin;Password=$dba_ariol;");
        }

        public DbSet<AnexoDeDocumento> Anexos { get; set; }
    }
}
