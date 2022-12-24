using Blog.Data;
using Blog.WebUI.Management.Authorize;
using Blog.WebUI.Management.Helpers;
using Blog.WebUI.Management.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.WebUI.Management.Controllers
{
    [Authorize]
    public class AuthorController : Controller
    {
        AuthorData _authorData;

        public AuthorController(AuthorData _authorData)
        {
            this._authorData = _authorData;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var authors = _authorData.GetBy(x => !x.IsDeleted);
            return View(authors);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var author = new Model.Author();
            return View(author);
        }

        [HttpPost]
        public IActionResult Add(Model.Author author)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(author.Fullname)) errors.Add("Ad Soyad Boş Bırakılamaz");
            if (string.IsNullOrEmpty(author.Username)) errors.Add("Kullanıcı Adı Boş Bırakılamaz");
            if (string.IsNullOrEmpty(author.Password)) errors.Add("Şifre Boş Bırakılamaz");
            if (errors.Count() > 0)
            {
                ViewBag.Result = new ViewModelResult(false, "Hata Oluştu", errors);
                return View(author);
            }

            var operationResult = _authorData.Insert(author);
            if (operationResult.IsSucceed)
            {
                ViewBag.Result = new ViewModelResult(true, "Yeni Yazar eklendi");
                return View(new Model.Author());
            }

            ViewBag.Result = new ViewModelResult(false, operationResult.Message);
            return View(author);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var author = _authorData.GetByKey(id);
            if (author == null)
                return RedirectToAction("Index", "Author", new { q = "kullanici-bulunamadi" });

            return View(author);
        }

        [HttpPost]
        public IActionResult Edit(Model.Author author)
        {
            var modelInDb = _authorData.GetByKey(author.Id);

            var errors = new List<string>();
            if (string.IsNullOrEmpty(author.Fullname)) errors.Add("Ad Soyad Boş Bırakılamaz");
            if (string.IsNullOrEmpty(author.Username)) errors.Add("Kullanıcı Adı Boş Bırakılamaz");
            if (string.IsNullOrEmpty(author.Password)) errors.Add("Şifre Boş Bırakılamaz");
            if (errors.Count() > 0)
            {
                ViewBag.Result = new ViewModelResult(false, "Hata Oluştu", errors);
                return View(author);
            }

            var exist_content = _authorData.GetBy(x => x.Username == author.Username && !x.IsDeleted && x.Id != author.Id).FirstOrDefault();
            if (exist_content != null)
            {
                ViewBag.Result = new ViewModelResult(false, "Bu kullanıcı adı kullanılıyor");
                return View(author);
            }

            modelInDb.Fullname = author.Fullname;
            modelInDb.IsActive = author.IsActive;
            modelInDb.Mail = author.Mail;
            modelInDb.Password = author.Password;
            modelInDb.Username = author.Username;

            var operationResult = _authorData.Update(modelInDb);
            if (operationResult.IsSucceed)
            {
                ViewBag.Result = new ViewModelResult(true, "Yazar Güncellendi.");
                return View(modelInDb);
            }

            ViewBag.Result = new ViewModelResult(false, operationResult.Message);
            return View(author);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var author = _authorData.GetByKey(id);
            if (author == null)
                return RedirectToAction("Index", "Author", new { q = "yazar-bulunamadi" });

            author.IsDeleted = true;
            var operationResult = _authorData.Update(author);
            if (operationResult.IsSucceed)
                return RedirectToAction("Index", "Author", new { q = "yazar-silindi" });
            else
                return RedirectToAction("Index", "Author", new { q = "yazar-silemedim" });
        }
    }
}
