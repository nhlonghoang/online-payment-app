using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlinePaymentApp.DataAccess.Repository.IRepository;
using OnlinePaymentApp.DataAcess.Data;
using OnlinePaymentApp.Models;

namespace OnlinePaymentApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {   
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.ProductRepository.GetAll().ToList();
            return View(products);
        }

        public IActionResult Create()
        {
            IEnumerable<SelectListItem> categoryList = _unitOfWork.CategoryRepository.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            ViewBag.CategoryList = categoryList; // using ViewBag
            //ViewData["CategoryList"] = categoryList; // using ViewData, need to cast to use
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product obj)
        {
            if (ModelState.IsValid)  // Check model validation
            {
                _unitOfWork.ProductRepository.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            return View(); // error automatically go with the view
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product productFromDb = _unitOfWork.ProductRepository.Get(u=>u.Id==id);
            //Product? productFromDb2 = _db.products.FirstOrDefault(u => u.Id == id);
            //Product? productFromDb3 = _db.products.Where(u => u.Id == id).FirstOrDefault();
            if (productFromDb == null)
            {
                return NotFound();
            }

            return View(productFromDb);

        }
        [HttpPost]
        public IActionResult Edit(Product obj)
        {
          
            if (ModelState.IsValid)  // Check model validation
            {
                _unitOfWork.ProductRepository.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product edited successfully";
                return RedirectToAction("Index");
            }
            return View(); // error automatically go with the view
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product productFromDb = _unitOfWork.ProductRepository.Get(u => u.Id == id);
            if (productFromDb == null)
            {
                return NotFound();
            }

            return View(productFromDb);

        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product? productFromDb = _unitOfWork.ProductRepository.Get(u => u.Id == id);

            if (productFromDb == null)
            {
                return NotFound();
            }

            _unitOfWork.ProductRepository.Remove(productFromDb);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
