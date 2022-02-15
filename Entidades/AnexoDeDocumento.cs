using DSAnexoDocumentoProjeto.Helper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static DSAnexoDocumentoProjeto.Helper.AllowedExtensionsAttribute;

namespace DSAnexoDocumentoProjeto.Entidades
{
    public class AnexoDeDocumento
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        [Required(ErrorMessage ="Preencha o campo obrigatório título do arquivo")]
        [Display(Name ="Título do arquivo:")]
        public string Titulo { get; set; }
        [Required(ErrorMessage = "Preencha o campo obrigatório  descrição de arquivo")]
        [Display(Name ="Descrição do arquivo:")]
        
        [StringLength(2000)]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "Preencha o campo obrigatório nome do arquivo")]
        [Display(Name ="Nome do arquivo:")]
        public string NomeDoArquivo { get; set; }
       
        [Display(Name ="Date de criação")]
        public DateTime DataDeCriacao { get; set; } = DateTime.Now;

       [NotMapped]
        [Display(Name ="Selecione um documento:")]
        [Required(ErrorMessage = "O campo arquivo é obrigatório")]
        [MaxFileSize(5 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png",".pdf"})]
        public IFormFile Arquivo { get; set; }
        public byte[] Bytes { get; set; }
        public string ContentType { get; set; }

    }
}
