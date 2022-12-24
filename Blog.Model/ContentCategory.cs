using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Model
{
    public class ContentCategory : Core.ModelBase
    {
        public ContentCategory()
        {
        }
        public ContentCategory(int contentId, int categoryId)
        {
            CategoryId = categoryId;
            ContentId = contentId;
        }
        public int CategoryId { get; set; }
        public int ContentId { get; set; }

    }
}
