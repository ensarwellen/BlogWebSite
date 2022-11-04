using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Model
{
    public class Media : Core.ModelBase
    {
        public string Filename { get; set; }
        public string FileSlug { get; set; }
        public string Alt { get; set; }
        public string Title { get; set; }
        public string MediaUrl { get; set; }

    }
}
