using Blog.Data.Infrastructure;
using Blog.Data.Infrastructure.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Data
{
    public class MediaData : EntityBaseData<Model.Media>
    {
        public MediaData(IOptions<DatabaseSettings> dbOptions) 
            : base(new DataContext(dbOptions.Value.ConnectionString))
        {

        }
    }
}
