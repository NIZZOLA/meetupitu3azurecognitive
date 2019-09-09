﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ImgCognitiveWeb.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using ImgCognitiveWeb.Utils;
using AzureStorageLib.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using ImageRecognition.Services;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace ImgCognitiveWeb.Controllers
{
    public class PicturesController : Controller
    {
        private readonly ImageStorageContext _context;
        private readonly IConfiguration _configuration;
        private readonly AzureStorageConfig _azureStorageConfig;
        private readonly string _computerVisionKey;
        private readonly string _computerVisionEndpoint;

        // Specify the features to return
        private static readonly List<VisualFeatureTypes> features =
            new List<VisualFeatureTypes>()
        {
            VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
            VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
            VisualFeatureTypes.Tags, VisualFeatureTypes.Adult, VisualFeatureTypes.Objects
        };

        public PicturesController(ImageStorageContext context, IOptions<AzureStorageConfig> configOptions, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _azureStorageConfig = new AzureStorageConfig()
            {
                AccountName = _configuration["AzureStorageConfig:accountName"],
                AccountKey = _configuration["AzureStorageConfig:accountKey"],
                ImageContainer = _configuration["AzureStorageConfig:imageContainer"],
                QueueName = _configuration["AzureStorageConfig:queueName"]
            };
            _computerVisionKey = _configuration["ComputerVisionKey"];
            _computerVisionEndpoint = _configuration["ComputerVisionEndpoint"];
        }

        // GET: Pictures
        public async Task<IActionResult> Index()
        {
            return View(await _context.Picture.ToListAsync());
        }

        // GET: Pictures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var picture = await _context.Picture
                .FirstOrDefaultAsync(m => m.PictureId == id);
            if (picture == null)
            {
                return NotFound();
            }

            PictureIAService picServ = new PictureIAService(_computerVisionEndpoint, _computerVisionKey);

            if (picture.Storage)
            {
                string url = AzureStorageLib.Services.AzureStorageService.BlobUrl(_azureStorageConfig, picture.Address);
            }
            else
            {

                var response = await picServ.AnalyzeRemoteAsync(picture.Address, features);
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(response);

                ViewBag.features = json;
            }


            return View(picture);
        }

        // GET: Pictures/Play/5
        public async Task<IActionResult> Play(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var picture = await _context.Picture
                .FirstOrDefaultAsync(m => m.PictureId == id);
            if (picture == null)
            {
                return NotFound();
            }

            PictureIAService picServ = new PictureIAService(_computerVisionEndpoint, _computerVisionKey);

            if (picture.Storage)
            {
                string url = AzureStorageLib.Services.AzureStorageService.BlobUrl(_azureStorageConfig, picture.Address);
            }
            else
            {

                var response = await picServ.AnalyzeRemoteAsync(picture.Address, features);
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(response);

                ViewBag.features = json;
            }


            return View(picture);
        }



        // GET: Pictures/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pictures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PictureId,Description,Address,Storage,Status,Result")] Picture picture, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (picture.Storage)
                {
                    if (file == null || file.Length == 0)
                    {
                        //retorna a viewdata com erro
                        ViewData["Erro"] = "Error: Arquivo(s) não selecionado(s)";
                        return View(ViewData);
                    }
                    else
                    {
                        if (!file.IsImage())
                        {
                            ViewData["Erro"] = "Error: Arquivo enviado não é imagem válida";
                            return View(ViewData);
                        }

                        string filename = file.FileName.Replace(" ", "");
                        var result = AzureStorageLib.Services.AzureStorageService.UploadFileToStorage(_azureStorageConfig, file.OpenReadStream(), filename);
                        picture.Address = filename;
                    }

                }

                _context.Add(picture);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(picture);
        }



        // GET: Pictures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var picture = await _context.Picture.FindAsync(id);
            if (picture == null)
            {
                return NotFound();
            }
            return View(picture);
        }

        // POST: Pictures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PictureId,Description,Address,Storage,Status,Result")] Picture picture)
        {
            if (id != picture.PictureId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(picture);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PictureExists(picture.PictureId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(picture);
        }

        // GET: Pictures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var picture = await _context.Picture.FirstOrDefaultAsync(m => m.PictureId == id);
            if (picture == null)
            {
                return NotFound();
            }

            return View(picture);
        }

        // POST: Pictures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var picture = await _context.Picture.FindAsync(id);
            if (picture.Storage)
            {
                await AzureStorageLib.Services.AzureStorageService.DeleteSpecificBlob(_azureStorageConfig, picture.Address);
            }
            _context.Picture.Remove(picture);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PictureExists(int id)
        {
            return _context.Picture.Any(e => e.PictureId == id);
        }
    }
}
