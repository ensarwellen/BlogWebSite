using Blog.Data.Infrastructure;
using Blog.Data.Infrastructure.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Data
{
    public class CommentData : EntityBaseData<Model.Comment>
    {
        public CommentData(IOptions<DatabaseSettings> dbOptions) 
            : base(new DataContext(dbOptions.Value.ConnectionString))
        {

        }
    }
}
