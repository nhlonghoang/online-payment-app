using Microsoft.AspNetCore.Mvc;

namespace OnlinePaymentApp.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
