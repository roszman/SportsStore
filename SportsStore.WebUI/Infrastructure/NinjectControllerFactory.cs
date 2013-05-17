using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using Ninject;
using SportsStore.Domain.Entities;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Concrete;

namespace SportsStore.WebUI.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
// ReSharper disable FieldCanBeMadeReadOnly.Local
        private IKernel _ninjectKernel;
// ReSharper restore FieldCanBeMadeReadOnly.Local

        public NinjectControllerFactory()
        {
            _ninjectKernel = new StandardKernel();
            AddBindings();
        }

        private void AddBindings()
        {
            //Mock<IProductRepository> mock = new Mock<IProductRepository>();
            //mock.Setup(m => m.Products).Returns(new List<Product>
            //    {
            //        new Product {Name = "Football", Price = 25},
            //        new Product {Name = "Surf board", Price = 25},
            //        new Product {Name = "Running shoes", Price = 25},

            //    }.AsQueryable());

            _ninjectKernel.Bind<IProductRepository>().To<EFProductRepository>();
            _ninjectKernel.Bind<IOrderProcessor>().To<EmailOrderProcssor>();            
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController) _ninjectKernel.Get(controllerType);
        }
    }
}