using Bridge.Domain.Common;
using Bridge.Domain.Products.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Domain.Products.Repos
{
    public interface IProductRepository : IRepository<Product>
    {
    }
}
