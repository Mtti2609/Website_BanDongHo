using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tshop.Data;
using Tshop.Helpers;
using Tshop.Models;
using Tshop.Services;
using Tshop.ViewModels;

namespace Tshop.Pages.Cart
{
    [Authorize]
    public class CheckoutModel : PageModel
    {
        private readonly IVnPayService _vnPayservice;
        private readonly TshopContext _db;
        

        public CheckoutModel(IVnPayService vnPayservice, TshopContext db)
        {
            _vnPayservice = vnPayservice;
            _db = db;
            
        }

        public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostCheckoutAsync()
        {
            


            // Create VnPaymentRequestModel
            var vnPayModel = new VnPaymentRequestModel
            {
                Amount = Cart.Sum(p=>p.ThanhTien), // Sử dụng giá trị đã ép kiểu
                CreatedDate = DateTime.Now,
                Description = "Dữ liệu mô phỏng",
                FullName = "Truong Minh Tri",
                OrderId = new Random().Next(1000, 100000)
            };

            // Send payment request to VNPay and redirect to VNPay's payment URL
            return Redirect(_vnPayservice.CreatePaymentUrl(HttpContext, vnPayModel));
            
        }




    }
}
