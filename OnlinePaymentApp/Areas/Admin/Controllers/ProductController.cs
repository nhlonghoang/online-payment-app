using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlinePaymentApp.DataAccess.Repository.IRepository;
using OnlinePaymentApp.DataAcess.Data;
using OnlinePaymentApp.Models;
using OnlinePaymentApp.Models.ViewModels;
using OnlinePaymentApp.Utility;

namespace OnlinePaymentApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {   
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment) {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.ProductRepository.GetAll(includeProperties:"Category").ToList();
            return View(products);
        }

        public IActionResult Upsert(int? id)
        {
            IEnumerable<SelectListItem> categoryList = _unitOfWork.CategoryRepository.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            // Using ViewModel to bundle data
            ProductVM productVM = new()
            {
                CategoryList = categoryList,
                Product = new Product()
            };

            //ViewBag.CategoryList = categoryList; // using ViewBag
            //ViewData["CategoryList"] = categoryList; // using ViewData, need to cast to use

            if (id == null || id == 0)
            {
                return View(productVM);
            }
            // Populate product when id is valid
            productVM.Product = _unitOfWork.ProductRepository.Get(u => u.Id == id);

            return View(productVM);
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)  // Check model validation
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null) { 
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        // delete old image
                        var oldImageUrl = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImageUrl))
                        {
                            System.IO.File.Delete(oldImageUrl);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }

                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.ProductRepository.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.ProductRepository.Update(productVM.Product);
                }
                
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            else {
                productVM.CategoryList = _unitOfWork.CategoryRepository.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                
                return View(productVM);
            }
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
