using AspNetCoreHero.ToastNotification.Abstractions;
using DSAnexoDocumentoProjeto.Entidades;
using DSAnexoDocumentoProjeto.Infraestrutura;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DSAnexoDocumentoProjeto.Entidades.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace DSAnexoDocumentoProjeto.Controllers
{
    public class UploadController : Controller
    {
        private readonly ArquivoContext _arquivoContext;
        private readonly INotyfService _notyfService;
        public UploadController(ArquivoContext arquivoContext, INotyfService notyfService)
        {
            _arquivoContext = arquivoContext;
            _notyfService = notyfService;
        }
        // GET: UploadController
        public async Task<IActionResult> Index(string filtroBusca)
        {
            var arquivos = await _arquivoContext.Anexos.ToListAsync();
            ViewData["CurrentFilter"] = filtroBusca;
            if (!string.IsNullOrEmpty(filtroBusca))
            {
                arquivos = arquivos.Where(a => a.ContentType.Contains(filtroBusca.ToLower()) || a.Titulo.Equals(filtroBusca.ToLower())
                                               || a.Descricao.Contains(filtroBusca.ToLower())).ToList();
                _notyfService.Success("Dado encontrado com sucesso.");
            }

            return View(arquivos);
        }

        // GET: UploadController/Details/5
        public ActionResult ViewPdf(int id)
        {
            var documentoAnexo = _arquivoContext.Anexos.AsNoTracking().FirstOrDefault(x => x.Id == id);
            return File(documentoAnexo.Bytes, documentoAnexo.ContentType);
        }

        // GET: UploadController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UploadController/Create        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AnexoDeDocumento anexoDeDocumento)
        {
            var arquivoPdf = anexoDeDocumento.Arquivo;
            var tamanhoMaximo = 50000000;

            if (arquivoPdf.Length > tamanhoMaximo)
            {
                _notyfService.Warning("Erro. Tamanho máximo do documento é 5Mb.");
                return View();
            }
            if (anexoDeDocumento.Arquivo.FileName.Contains(".zip") || anexoDeDocumento.Arquivo.FileName.Contains(".exe") || anexoDeDocumento.Arquivo.FileName.Contains(".bat"))
            {
                _notyfService.Error("Erro. Documento com extensão .Exe ou .Zip ou .Bat não permitido.");
                return View();
            }

            if (ModelState.IsValid)
            {
                if (anexoDeDocumento != null && anexoDeDocumento.Arquivo != null)
                {
                    MemoryStream ms = new MemoryStream();
                    arquivoPdf.OpenReadStream().CopyTo(ms);

                    anexoDeDocumento.Bytes = ms.ToArray();
                    anexoDeDocumento.ContentType = arquivoPdf.ContentType;
                    anexoDeDocumento.Descricao.ToLower();
                    anexoDeDocumento.Titulo.ToLower();

                    _arquivoContext.Anexos.Add(anexoDeDocumento);
                    _arquivoContext.SaveChanges();

                    _notyfService.Success("Documento salvo com sucesso.");
                    return RedirectToAction(nameof(Index));
                }
            }
            _notyfService.Error("Erro.Dados do documento inválido.");
            return View();
        }

        // GET: UploadController/Edit/5
        public ActionResult Edit(int id)
        {
            var documentoAnexo = _arquivoContext.Anexos.AsNoTracking().FirstOrDefault(x => x.Id == id);
            var anexoDoc = new AnexoDocumentoViewModel
            {
                Id = documentoAnexo.Id,
                ContentType = documentoAnexo.ContentType,
                Titulo = documentoAnexo.Titulo,
                NomeDoArquivo = documentoAnexo.NomeDoArquivo,
                Descricao = documentoAnexo.Descricao,
                Bytes = documentoAnexo.Bytes,
                DataDeCriacao = documentoAnexo.DataDeCriacao,

            };
            if (anexoDoc != null)
            {
                return View(anexoDoc);
            }
            _notyfService.Warning("Atenção. Não é possível editar o documento.");
            return View();
        }

        // POST: UploadController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AnexoDocumentoViewModel anexoDeDocumentoViewModel)
        {

            var arquivoFormFile = anexoDeDocumentoViewModel.Arquivo;
            var documentoAnexo = _arquivoContext.Anexos.AsNoTracking().FirstOrDefault(x => x.Id == anexoDeDocumentoViewModel.Id);
            var tamanhoMaximo = 50000000;

            if (anexoDeDocumentoViewModel.Arquivo != null)
            {
                MemoryStream ms = new MemoryStream();
                arquivoFormFile.OpenReadStream().CopyTo(ms);
                anexoDeDocumentoViewModel.Bytes = ms.ToArray();
                anexoDeDocumentoViewModel.ContentType = arquivoFormFile.ContentType;
            }
            else
            {
                var stream = new MemoryStream(documentoAnexo.Bytes);
                arquivoFormFile = new FormFile(stream, 0, documentoAnexo.Bytes.Length, anexoDeDocumentoViewModel.Descricao, anexoDeDocumentoViewModel.NomeDoArquivo);
                anexoDeDocumentoViewModel.Bytes = documentoAnexo.Bytes;
                anexoDeDocumentoViewModel.ContentType = documentoAnexo.ContentType;
            }

            if (arquivoFormFile.Length > tamanhoMaximo)
            {
                _notyfService.Warning("Erro. Tamanho máximo do documento é 5Mb.");
                return View();
            }
            if (arquivoFormFile.FileName.Contains(".zip") || arquivoFormFile.FileName.Contains(".exe") || arquivoFormFile.FileName.Contains(".bat"))
            {
                _notyfService.Error("Erro. Documento com extensão .Exe ou .Zip ou .Bat não permitido.");
                return View();
            }

            if (ModelState.IsValid)
            {
                if (anexoDeDocumentoViewModel != null)
                {

                    anexoDeDocumentoViewModel.Arquivo = documentoAnexo.Arquivo;
                    anexoDeDocumentoViewModel.Descricao.ToLower();
                    anexoDeDocumentoViewModel.Titulo.ToLower();

                    var anexoDocumento = new AnexoDeDocumento
                    {
                        Id = anexoDeDocumentoViewModel.Id,
                        Titulo = anexoDeDocumentoViewModel.Titulo,
                        NomeDoArquivo = anexoDeDocumentoViewModel.NomeDoArquivo,
                        Descricao = anexoDeDocumentoViewModel.Descricao,
                        DataDeCriacao = anexoDeDocumentoViewModel.DataDeCriacao,
                        ContentType = anexoDeDocumentoViewModel.ContentType,
                        Bytes = anexoDeDocumentoViewModel.Bytes,
                    };

                    _arquivoContext.Anexos.Update(anexoDocumento);
                    _arquivoContext.SaveChanges();

                    _notyfService.Success("Documento salvo com sucesso.");
                    return RedirectToAction(nameof(Index));
                }
            }
            _notyfService.Error("Erro.Dados do documento inválido.");
            return View();
        }


        public ActionResult Download(int id)
        {
            var documentoAnexo = _arquivoContext.Anexos.AsNoTracking().FirstOrDefault(x => x.Id == id);
            var extensaoDoArquivo = documentoAnexo.NomeDoArquivo + "." + documentoAnexo.ContentType.Split("/")[1];
            return File(documentoAnexo.Bytes, "application/x-msdownload", extensaoDoArquivo);
        }

        // POST: UploadController/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (id > 0)
            {
                var documentoAnexo = _arquivoContext.Anexos.AsNoTracking().FirstOrDefault(x => x.Id == id);

                _arquivoContext.Anexos.Remove(documentoAnexo);
                _notyfService.Success("Documento excluído com sucesso.");
                _arquivoContext.SaveChanges();

            }
            _notyfService.Error("Erro ao tentar excluir documento.");
            return RedirectToAction("Index", "Upload");
        }
    }
}
