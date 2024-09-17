using Microsoft.EntityFrameworkCore;
using TestCase.Models;
using TestCase.Repositories.Base;

namespace TestCase.Repositories
{
    public class ProductRepository : GenericIdRepository<Product>
    {
        public ProductRepository(DbContext context)
            : base(context)
        {
        }
    }
}
