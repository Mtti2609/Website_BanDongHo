using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tshop.Services;
using Tshop.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Tshop.Data;
using Tshop.Helpers;


namespace Tshop.Pages
{
    public class PaymentCallBackModel : PageModel
    {
        private readonly IVnPayService _vnPayService;
        private readonly TshopContext _context;
       
        public PaymentCallBackModel(IVnPayService vnPayService, TshopContext context)
        {
            _vnPayService = vnPayService;
            _context = context;
           
        }

        public IActionResult OnGet()
        {
            try
            {
                var response = _vnPayService.PaymentExecute(Request.Query);

                if (response == null || response.VnPayResponseCode != "00")
                {
                    TempData["Message"] = $"Lỗi thanh toán VN Pay: {response?.VnPayResponseCode ?? "Không có mã phản hồi từ VN Pay"}";
                    return RedirectToPage("/Cart/PaymentFail");
                }

                HttpContext.Session.Remove(MySetting.CART_KEY);
                TempData["Message"] = "Thanh toán VNPay thành công";
                return RedirectToPage("/Cart/PaymentSuccess");
                
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                TempData["Message"] = "Có lỗi xảy ra khi xử lý thanh toán";
                return RedirectToPage("/Cart/PaymentFail");
            }
        }


    }
}