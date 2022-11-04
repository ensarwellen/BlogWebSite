﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Model
{
    public class Tag : Core.ModelBase
    {
        public string Name { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string Slug { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

    }
}
