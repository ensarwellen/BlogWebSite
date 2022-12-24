using Blog.Data.Infrastructure;
using Blog.Data.Infrastructure.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Data
{
    public class RolePageData : EntityBaseData<Model.RolePage>
    {
        public RolePageData(IOptions<DatabaseSettings> dbOptions) 
            : base(new DataContext(dbOptions.Value.ConnectionString))
        {

        }
    }
}
