using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace WebLayer.Controllers
{
    public class ProductProviderController : Controller
    {
        public IEnumerable<Models.ProviderViewModel> GetAllProviders()
        {
            using (var client = new HttpClient())
            {
                var httpResponse = client.GetAsync("http://localhost:50667/api/Product/GetProviders").Result;
                try
                {
                    var response = httpResponse;
                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        var items = JsonConvert.DeserializeObject<IEnumerable<Domain.Provider>>(result);
                        return items.Select(x => new Models.ProviderViewModel()
                        {
                            Id = x.Id,
                            Name = x.Name.Trim() 
                        });
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            };
        }

        public Models.ProductViewModel GetProductById(int id)
        {
            using (var client = new HttpClient())
            {
                var httpResponse = client.GetAsync("http://localhost:50667/api/Product/Get/" + id).Result;
                try
                {
                    var response = httpResponse;
                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        var item = JsonConvert.DeserializeObject<Domain.Product>(result);
                        var providers = new List<Models.ProviderViewModel>();
                        if (item.Providers != null)
                            item.Providers.ToList().ForEach(x => providers.Add(new Models.ProviderViewModel() { Id = x.Id, Name = x.Name.Trim() }));

                        return new Models.ProductViewModel()
                        {
                            Id = item.Id,
                            Name = item.Name.Trim(),
                            Description = item.Description.Trim(),
                            Providers = providers
                        };
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            };
        }
         
        public Models.ProductViewModel CreateProduct(Models.ProductViewModel product)
        {
            using (var client = new HttpClient())
            {
                var domainProduct = new Domain.Product()
                {
                    Id = product.Id,
                    Name = product.Name.Trim(),
                    Description = product.Description.Trim()
                };

                var stringProduct = JsonConvert.SerializeObject(domainProduct);

                var httpContent = new StringContent(stringProduct, Encoding.UTF8, "application/json");

                var httpResponse = client.PostAsync("http://localhost:50667/api/Product/Create", httpContent).Result;
                try
                {
                    var response = httpResponse;
                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        var item = JsonConvert.DeserializeObject<Domain.Product>(result);
                        return new Models.ProductViewModel()
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Description = item.Description
                        };
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            };
        }
          
        public void AddProductProviderPost(int productId, int providerId)
        {
            using (var client = new HttpClient())
            {
                var domainProduct = new Domain.ProductProvider()
                {
                    ProductId = productId,
                    ProviderId = providerId
                };

                var stringProduct = JsonConvert.SerializeObject(domainProduct);

                var httpContent = new StringContent(stringProduct, Encoding.UTF8, "application/json");

                var httpResponse = client.PostAsync("http://localhost:50667/api/Product/AddProductProvider", httpContent).Result;
                try
                {
                    var response = httpResponse;
                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        var item = JsonConvert.DeserializeObject<Domain.Product>(result);

                    }

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            };
        }

        // GET: Product
        public ActionResult Index(int productId )
        {
            var product = GetProductById(productId);
            var providers = GetAllProviders();
            var productProviders = new List< Models.ProductProviderViewModel>();
            foreach (var item in providers)
            {
                if ( product.Providers.Where(x=>x.Id==item.Id).Count()==0)  //new Models.ProviderViewModel() { Id = item.Id, Name = item.Name }))
                    productProviders.Add(new Models.ProductProviderViewModel() {ProductId=product.Id,ProductName=product.Name,ProviderId=item.Id,ProviderName=item.Name });
            }
            
            return View(productProviders.ToList());
        }
         
        public ActionResult AddProductProvider(int productId, int providerId)
        {
            AddProductProviderPost(productId, providerId);
            return RedirectToAction("Details","Product",  new { id = productId });
        }

         

    }
}
