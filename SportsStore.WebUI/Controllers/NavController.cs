using System.Collections.Generic;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using System.Linq;
namespace SportsStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        private readonly IProductRepository _repository;

        public NavController(IProductRepository repository)
        {
            _repository = repository;
        }

        public PartialViewResult Menu(string category = null)
        {
            
            ViewBag.SelectedCategory = category;
            IEnumerable<string> categories = _repository.Products.Select(c => c.Category).Distinct().OrderBy(c => c);
            return PartialView(categories);
        }

    }
}
