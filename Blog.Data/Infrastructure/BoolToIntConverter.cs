﻿using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Blog.Data.Infrastructure
{
    
    public class BoolToIntConverter : ValueConverter<bool, int>
    {
        public BoolToIntConverter(ConverterMappingHints mappingHints = null)
            :base(
                 v=>Convert.ToInt32(v),
                 v=>Convert.ToBoolean(v),
                 mappingHints)
        {
        }

        public static ValueConverterInfo DefaultInfo { get; }
            = new ValueConverterInfo(typeof(bool), typeof(int), i => new BoolToIntConverter(i.MappingHints));
    }
}
