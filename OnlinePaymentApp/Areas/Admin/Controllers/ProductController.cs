using Microsoft.AspNetCore.Mvc;
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
            List<Product> categories = _unitOfWork.ProductRepository.GetAll().ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
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
            //Product? productFromDb2 = _db.Categories.FirstOrDefault(u => u.Id == id);
            //Product? productFromDb3 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();
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
