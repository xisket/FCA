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
    public class ProductController : Controller
    {
        public IEnumerable<Models.ProductViewModel> GetProductsPaged(int requestedPage, int pageSize, string searchString, string orderByFieldName, bool orderByDescending)
        {
            using (var client = new HttpClient())
            {
                var sortFieldExpressions = new List<Domain.SortFieldExpression>();
                sortFieldExpressions.Add(new Domain.SortFieldExpression() { FieldName = orderByFieldName, Descending = orderByDescending });
                var pagedQuery = new Domain.PagedQuery() { PagedSize = pageSize, RequestedPage = requestedPage, SearchString = searchString, SortFieldExpressions = sortFieldExpressions };
                var stringpagedQuery = JsonConvert.SerializeObject(pagedQuery);

                var httpContent = new StringContent(stringpagedQuery, Encoding.UTF8, "application/json");

                var httpResponse = client.PostAsync("http://localhost:50667/api/Product/GetPaged", httpContent).Result;
                try
                {
                    var response = httpResponse;
                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        var items = JsonConvert.DeserializeObject<IEnumerable<Domain.Product>>(result);
                        return items.Select(x => new Models.ProductViewModel()
                        {
                            Id = x.Id,
                            Name = x.Name.Trim(),
                            Description = x.Description.Trim()
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
        public IEnumerable<Models.ProductViewModel> GetAllProducts()
        {
            using (var client = new HttpClient())
            {
                    var httpResponse = client.GetAsync("http://localhost:50667/api/Product/Get").Result;
                try
                {
                    var response = httpResponse;
                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        var items = JsonConvert.DeserializeObject<IEnumerable<Domain.Product>>(result);
                        return items.Select(x => new Models.ProductViewModel()
                        {
                            Id = x.Id,
                            Name = x.Name.Trim(),
                            Description = x.Description.Trim()
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

        public Models.ProductViewModel UpdateProduct(Models.ProductViewModel product)
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

                var httpResponse = client.PostAsync("http://localhost:50667/api/Product/Modify", httpContent).Result;
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

        public Models.ProductViewModel DeleteProduct(Models.ProductViewModel product)
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

                var httpResponse = client.PostAsync("http://localhost:50667/api/Product/Delete", httpContent).Result;
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

        public void DeleteProductProviderPost(int productId, int providerId)
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

                var httpResponse = client.PostAsync("http://localhost:50667/api/Product/DeleteProductProvider", httpContent).Result;
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
        public ActionResult Index(int? requestedPage, string searchString, string orderByFieldName, bool? orderByDescending = true, int? totalPages = -1)
        {
            var pagedSize = 6;
            if (!requestedPage.HasValue || requestedPage.Value <= 0)
                requestedPage = 1;
            if (totalPages == -1)
            {
                var allproducts = GetAllProducts();
                totalPages = (int)(allproducts.Count()+ pagedSize-1) / pagedSize; 
            }
             
            if (requestedPage.Value >= totalPages)
                requestedPage = totalPages;
            ViewBag.requestedPage = requestedPage;
            ViewBag.orderByFieldName = orderByFieldName;
            ViewBag.orderByDescending = orderByDescending;
            ViewBag.totalPages = totalPages;

            //var products = GetAllProducts();
            var products = GetProductsPaged(requestedPage.Value, 6, searchString, orderByFieldName, orderByDescending.Value);

            return View(products.ToList());
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            var product = GetProductById(id);
            return View(product);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "Name, Description")]Models.ProductViewModel product)
        {
            if (ModelState.IsValid)
            {

                var newProduct = CreateProduct(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            var product = GetProductById(id);
            return View(product);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(int id)
        {
            var productToUpdate = GetProductById(id);
            if (TryUpdateModel(productToUpdate, "",
               new string[] { "Name", "Description" }))
            {
                productToUpdate = UpdateProduct(productToUpdate);
                return RedirectToAction("Index");

            }
            return View(productToUpdate);
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            var product = GetProductById(id);
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var productToDelete = GetProductById(id);
                productToDelete = DeleteProduct(productToDelete);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult DeleteProductProvider(int productId, int providerId)
        {
            DeleteProductProviderPost(productId, providerId);
            return RedirectToAction("Details", "Product", new { id = productId });
        }

        public ActionResult AddProductProvider(int productId, int providerId)
        {
            AddProductProviderPost(productId, providerId);
            return RedirectToAction("Details", "Product", new { id = productId });
        }


        // GET: Product
        public ActionResult IndexProviders(int id)
        {

            var product = GetProductById(id);

            return View(product);
        }

    }
}
