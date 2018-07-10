using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface IProductRepository
    {
        Product ById(int id);
        IEnumerable<Product> All();
        IEnumerable<Product> Paged(int requestedPage, int pageSize, string searchString, string orderByFieldName, bool orderByDescending);

        void AddProduct(string name, string description);
        void AddProductProvider(int productId, int providerId);

        void DeleteProduct(int id);
        void DeleteAllProductProviders(int productId);
        void DeleteProductProvider(int productId,int providerId);

        void UpdatePoduct(Domain.Product product);
    }
}
