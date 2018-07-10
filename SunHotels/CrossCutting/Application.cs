using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace CrossCutting
{
    public static class Application
    {
        private static readonly UnityContainer _current = new UnityContainer();
         
        public static void RegisterTypes()
        {
            _current.RegisterType< Domain.IProductRepository,  DataLayer.Products>(new PerResolveLifetimeManager());
            _current.RegisterType< Domain.IProviderRepository,  DataLayer.Providers>(new PerResolveLifetimeManager());
        }

        public static T Resolve<T>()
        {
            return _current.Resolve<T>();
        }
    }
}
