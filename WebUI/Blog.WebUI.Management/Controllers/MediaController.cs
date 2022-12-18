using Blog.Data;
using Blog.WebUI.Management.Helpers;
using Blog.WebUI.Management.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.WebUI.Management.Controllers
{
    public class MediaController:Controller
    {
        MediaData _mediaData;
        ContentData _contentData;
        public MediaController(MediaData _mediaData, ContentData _contentData)
        {
            this._mediaData = _mediaData;
            this._contentData = _contentData;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var medias = _mediaData.GetAll();
            return View(medias);
        }
        [HttpGet]
        public IActionResult Add()
        {
            var media = new Model.Media();
            return View(media);
        }
        [HttpPost]
        public IActionResult Add(Model.Media media, IFormFile file)
        {
            var errors = new List<string>();
            if (file==null || file.Length == 0)
            {
                ViewBag.Result = new ViewModelResult(false, "Dosya yok");
                return View(media);
            }
            var extension = Path.GetExtension(file.FileName).Trim('.').ToLower();
            if(!(new [] {"jpg", "png", "jpeg"}.Contains(extension)))
            {
                ViewBag.Result = new ViewModelResult(false, "Uzantı hatalı");
                return View(media);
            }
            var local_image_dir = $"wwwroot/_uploads/images";
            var local_image_path = $"{local_image_dir}/{file.FileName}";
            if (!Directory.Exists(Path.Combine(local_image_dir)))
                Directory.CreateDirectory(Path.Combine(local_image_dir));
            using(Stream fileStream = new FileStream(local_image_path, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            media.MediaUrl = $"{local_image_path}";
            media.FileSlug = Path.GetFileNameWithoutExtension(file.FileName).ToSlug();
            media.Alt = media.Alt ?? "";
            media.Title = media.Title ?? "";
            var operationResult = _mediaData.Insert(media);
            if(operationResult.IsSucceed)
            {
                ViewBag.Result = new ViewModelResult(true, "Yeni Media Eklendi");
                return View(new Model.Media());
            }
            ViewBag.Result = new ViewModelResult(false, operationResult.Message);
            return View(media);
        }
        
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var result = true;
            var media = _mediaData.GetByKey(id);
            if (media == null)
                return RedirectToAction("Index", "Media", new { q = "media-bulunamadi" });

            var media_url = media.MediaUrl;
            if (System.IO.File.Exists(media_url))
                System.IO.File.Delete(media_url);

            result = _mediaData.DeleteByKey(id).IsSucceed;
            if(result)
            {
                var contents = _contentData.GetBy(x => x.MediaId == media.Id);
                foreach (var c in contents)
                {
                    c.MediaId = -1;
                    _contentData.Update(c);
                }
            }
           
                return RedirectToAction("Index", "Media", new { q = "media-silindi" });                       
        }
    }
}
