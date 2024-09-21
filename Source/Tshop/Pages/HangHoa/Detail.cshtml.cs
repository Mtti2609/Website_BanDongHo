using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Tshop.Data;
using Tshop.ViewModels;

namespace Tshop.Pages.HangHoa
{
	public class DetailModel : PageModel
	{
		private readonly TshopContext _db;

		public DetailModel(TshopContext context)
		{
			_db = context;
		}

		[BindProperty]
		public ChiTietHangHoaVM chiTietHangHoaVM { get; set; }

		public async Task<IActionResult> OnGetAsync(int id)
		{
			var data = await _db.HangHoas
				.Include(p => p.MaLoaiNavigation)
				.SingleOrDefaultAsync(p => p.MaHh == id);

			if (data == null)
			{
				return NotFound(); // Trả về trạng thái 404 Not Found
			}

			var result = new ChiTietHangHoaVM
			{
				MaHh = data.MaHh,
				TenHH = data.TenHh,
				DonGia = data.DonGia ?? 0,
				ChiTiet = data.MoTa ?? string.Empty,
				Hinh = data.Hinh ?? string.Empty,
				MoTaNgan = data.MoTaDonVi ?? string.Empty,
				TenLoai = data.MaLoaiNavigation != null ? data.MaLoaiNavigation.TenLoai : string.Empty,
				SoLuongTon = 10, // tính sau
				DiemDanhGia = 5 // check sau
			};

			chiTietHangHoaVM = result;

			return Page();
		}

	}
}
