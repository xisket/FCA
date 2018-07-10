using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface IProviderRepository
    {
        Provider ById(int id);
        IEnumerable<Provider> All();
        void AddProvider(string name);

        void DeleteProvider(int id);
        void DeleteProviderProduct(int providerId);
    }
}
