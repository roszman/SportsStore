using SportsStore.Domain.Abstract;
using System.Linq;
using System.Web.Mvc;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        public int PageSize = 4;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public ViewResult List(int page = 1)
        {
            var model = new ProductsListViewModel
                {
                    Products = _productRepository.Products
                                                 .OrderBy(p => p.ProductID)
                                                 .Skip((page - 1)*PageSize)
                                                 .Take(PageSize),
                    PagingInfo = new PagingInfo
                        {
                            CurrentPage = page,
                            ItemsPerPage = PageSize,
                            TotalItems = _productRepository.Products.Count()
                        }
                };
            
            return View(model);
        }

    }
}
