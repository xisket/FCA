using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class ProductController : ApiController
    {
        [HttpPost]
        [Route("api/Product/GetPaged")]
        public HttpResponseMessage GetPaged(Domain.PagedQuery pagedQuery)
        {
            var fieldSort = "";
            var fieldSortDescending = true;
            if(pagedQuery.SortFieldExpressions!=null)
            {
                fieldSort = pagedQuery.SortFieldExpressions.First().FieldName;
                fieldSortDescending = pagedQuery.SortFieldExpressions.First().Descending;
            }

           var products = CrossCutting.Application.Resolve<Domain.IProductRepository>().Paged(pagedQuery.RequestedPage , pagedQuery.PagedSize , pagedQuery.SearchString , fieldSort, fieldSortDescending);
          
            var okResponse = this.Request.CreateResponse(HttpStatusCode.OK, products);

            return okResponse;
        }

        [HttpGet]
        [Route("api/Product/Get/{id}")]
        public HttpResponseMessage Get(int id )
        {
            var products = CrossCutting.Application.Resolve<Domain.IProductRepository>().ById(id );

            var okResponse = this.Request.CreateResponse(HttpStatusCode.OK, products);

            return okResponse;
        }
        [HttpGet]
        [Route("api/Product/Get")]
        public HttpResponseMessage Get( )
        {
            var products = CrossCutting.Application.Resolve<Domain.IProductRepository>().All();

            var okResponse = this.Request.CreateResponse(HttpStatusCode.OK, products);

            return okResponse;
        }


        [HttpPost]
        [Route("api/Product/Modify")]
        public HttpResponseMessage Modify(Domain.Product product)
        {
            CrossCutting.Application.Resolve<Domain.IProductRepository>().UpdatePoduct(product);

            var okResponse = this.Request.CreateResponse(HttpStatusCode.OK, product);

            return okResponse;
        }

        [HttpPost]
        [Route("api/Product/Create")]
        public HttpResponseMessage Create(Domain.Product product)
        {
            CrossCutting.Application.Resolve<Domain.IProductRepository>().AddProduct(product.Name,product.Description);

            var okResponse = this.Request.CreateResponse(HttpStatusCode.OK, product);

            return okResponse;
        }

        [HttpPost]
        [Route("api/Product/Delete")]
        public HttpResponseMessage Delete(Domain.Product product)
        {
            CrossCutting.Application.Resolve<Domain.IProductRepository>().DeleteProduct( product.Id);

            var okResponse = this.Request.CreateResponse(HttpStatusCode.OK, product);

            return okResponse;
        }

        [HttpPost]
        [Route("api/Product/DeleteProductProvider")]
        public HttpResponseMessage   DeleteProductProvider(Domain.ProductProvider productProvider)
        {
            CrossCutting.Application.Resolve<Domain.IProductRepository>().DeleteProductProvider(productProvider.ProductId, productProvider.ProviderId);

            var okResponse = this.Request.CreateResponse(HttpStatusCode.OK);

            return okResponse;
        }
        [HttpPost]
        [Route("api/Product/AddProductProvider")]
        public HttpResponseMessage AddProductProvider(Domain.ProductProvider productProvider)
        {
            CrossCutting.Application.Resolve<Domain.IProductRepository>().AddProductProvider( productProvider.ProductId, productProvider.ProviderId);

            var okResponse = this.Request.CreateResponse(HttpStatusCode.OK);

            return okResponse;
        }

        [HttpGet]
        [Route("api/Product/GetProviders")]
        public HttpResponseMessage GetProviders()
        {
            var providers = CrossCutting.Application.Resolve<Domain.IProviderRepository>().All();

            var okResponse = this.Request.CreateResponse(HttpStatusCode.OK, providers);

            return okResponse;
        }
    }
}