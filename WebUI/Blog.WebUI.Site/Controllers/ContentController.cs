using Blog.Data;
using Blog.WebUI.Site.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.WebUI.Site.Controllers
{
    public class ContentController : Controller
    {
        ContentData _contentData;
        ContentCategoryData _contentCategoryData;

        public ContentController(ContentData _contentData, ContentCategoryData _contentCategoryData)
        {
            this._contentData = _contentData;
            this._contentCategoryData = _contentCategoryData;
        }
        public IActionResult Index(string slug)
        {
            var content = _contentData.GetContentBySlug(slug);
            if (content == null)
                return RedirectToAction("Index", "Home", new { q = "makale-bulunamadi" });

            var category = _contentCategoryData.GetBy(x => x.ContentId == content.Id).FirstOrDefault();
            var relateds = new List<Model.Content>();

            if (category != null)
            {
                var category_content_ids = _contentCategoryData.GetBy(x => x.CategoryId == category.Id)
                .Take(6).Select(x => x.ContentId).ToList();

                relateds = _contentData.GetContentsByIds(category_content_ids, 6);
            }
            else
            {
                var dtNow = DateTime.Now;
                relateds = _contentData.GetBy(x => x.IsActive && !x.IsDeleted && x.PublishDate <= dtNow);
            }

            var model = new ContentViewModel()
            {
                Content = content,
                Relateds = relateds,
            };

            return View(model);
        }
    }
}
