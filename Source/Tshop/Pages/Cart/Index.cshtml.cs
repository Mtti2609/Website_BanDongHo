using Microsoft.AspNetCore.Mvc.RazorPages;
using Tshop.Data;
using Tshop.Helpers;
using Tshop.ViewModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Tshop.Pages.Cart
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly TshopContext _db;

        public IList<CartItem> CartItems { get; set; }
        
        public IndexModel(TshopContext context)
        {
            _db = context;
        }

        public void OnGet()
        {
            CartItems = HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();
        }
		public async Task<IActionResult> OnPostRemoveCartAsync(int productId)
		{
			// Đảm bảo rằng Session đã được khởi tạo
			if (!HttpContext.Session.IsAvailable)
			{
				return RedirectToPage("/Error"); // Hoặc bất kỳ trang lỗi nào bạn muốn
			}

			CartItems = HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();

			var itemToRemove = CartItems.FirstOrDefault(x => x.MaHh == productId);

			if (itemToRemove != null)
			{
				CartItems.Remove(itemToRemove);
				HttpContext.Session.Set(MySetting.CART_KEY, CartItems);
			}

			// Trả về trang hiện tại
			return RedirectToPage("/Cart/Index"); // Hoặc có thể dùng RedirectToPage("/Cart/Index") để refresh trang
		}

	}
}
