using DSAnexoDocumentoProjeto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DSAnexoDocumentoProjeto.Entidades;
using DSAnexoDocumentoProjeto.Infraestrutura;
using Newtonsoft.Json;

namespace DSAnexoDocumentoProjeto.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ArquivoContext _arquivoContext;

        public HomeController(ILogger<HomeController> logger, ArquivoContext arquivoContext)
        {
            _logger = logger;
            _arquivoContext = arquivoContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public JsonResult GetChartDocumento()
        {
            var pdfCount = _arquivoContext.Anexos.Count(x => x.ContentType.Contains("pdf"));
            var pngCount = _arquivoContext.Anexos.Count(x => x.ContentType.Contains("png"));
            var jpgCount = _arquivoContext.Anexos.Count(x => x.ContentType.Contains("jpg"));
            var jpegCount = _arquivoContext.Anexos.Count(x => x.ContentType.Contains("jpeg"));
           
            var listaDocumento = new List<ChartAnexo>();

            listaDocumento.Add(new ChartAnexo
            {
                CategoriaDocumento = "pdf",
                QuantidadeDocumento = pdfCount
            });

            listaDocumento.Add(new ChartAnexo
            {
                CategoriaDocumento = "png",
                QuantidadeDocumento = pngCount
            });

            listaDocumento.Add(new ChartAnexo
            {
                CategoriaDocumento = "jpg",
                QuantidadeDocumento = jpgCount
            });

            listaDocumento.Add(new ChartAnexo
            {
                CategoriaDocumento = "jpeg",
                QuantidadeDocumento = jpegCount
            });

            JsonConvert.SerializeObject(listaDocumento);

            return Json(new {JSONList = listaDocumento});

        }
    }
}
