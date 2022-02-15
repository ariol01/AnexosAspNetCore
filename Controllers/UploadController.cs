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
        public ActionResult Index()
        {
            var arquivos = _arquivoContext.Anexos.ToList();
            return View();
        }

        // GET: UploadController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UploadController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UploadController/Create        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AnexoDeDocumento anexoDeDocumento)
        {
            var arquivoPdf = anexoDeDocumento.Arquivo;
            var tamanhoMaximo = 50000000;
            if (anexoDeDocumento.Arquivo.FileName.Contains(".zip") || anexoDeDocumento.Arquivo.FileName.Contains(".exe") || anexoDeDocumento.Arquivo.FileName.Contains(".bat"))
            {
                _notyfService.Error("Erro. Documento com extensão .Exe ou .Zip ou .Bat não permitido.");
                return RedirectToAction(nameof(Index));
            }

            if (arquivoPdf.Length > tamanhoMaximo)
            {
                _notyfService.Warning("Erro. Tamanho máximo do documento é 5Mb.");
            }
            if (ModelState.IsValid)
            {
                if (arquivoPdf != null)
                {
                    MemoryStream ms = new MemoryStream();
                    arquivoPdf.OpenReadStream().CopyTo(ms);

                    anexoDeDocumento.Bytes = ms.ToArray();
                    anexoDeDocumento.ContentType = arquivoPdf.ContentType;

                    _arquivoContext.Anexos.Add(anexoDeDocumento);
                    _arquivoContext.SaveChanges();

                    _notyfService.Success("Documento salvo com sucesso.");
                    return RedirectToAction(nameof(Index));
                }
            }
            _notyfService.Error("Erro. Requisição falhou. Tente novamente.");
            return RedirectToAction(nameof(Index));
        }

        // GET: UploadController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UploadController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UploadController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UploadController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
