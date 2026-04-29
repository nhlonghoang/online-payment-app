using Microsoft.AspNetCore.Mvc;
using OnlinePaymentApp.DataAccess.Repository.IRepository;
using OnlinePaymentApp.Utility;
using System.Security.Claims;

namespace OnlinePaymentApp.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {

        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /*
        * Called by view to render view
        * view automatically lookup in folder Views/Components/{Nameof this file except ViewComponent suffix}/Default.cshtml
        */
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {    // logged in

                if (HttpContext.Session.GetInt32(SD.SessionCart) == null)
                { // create session if doesn't have
                    HttpContext.Session.SetInt32(SD.SessionCart,
                    _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).Count());
                }

                return View(HttpContext.Session.GetInt32(SD.SessionCart));
            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
        }

    }
}
