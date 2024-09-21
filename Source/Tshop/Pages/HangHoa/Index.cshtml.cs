using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Tshop.Data;
using Tshop.Helpers;
using Tshop.ViewModels;

namespace Tshop.Pages.HangHoa
{
    public class HangHoaModel : PageModel
    {
        private readonly TshopContext _db;

        public HangHoaModel(TshopContext context)
        {
            _db = context;

        }
        [BindProperty]
        public IList<HangHoaVM> HangHoaVMs { get; set; }

        public async Task<IActionResult> OnGetAsync(int? loai)
        {
            IQueryable<Tshop.Data.HangHoa> hangHoas = _db.HangHoas.AsQueryable();

            if (loai.HasValue)
            {
                hangHoas = hangHoas.Where(p => p.MaLoai == loai.Value);
            }

            HangHoaVMs = await hangHoas.Select(p => new HangHoaVM
            {
                MaHh = p.MaHh,
                TenHH = p.TenHh,
                DonGia = p.DonGia ?? 0,
                Hinh = p.Hinh ?? "",
                MoTaNgan = p.MoTaDonVi ?? "",
            }).ToListAsync();

            return Page();
        }
        public IActionResult OnPostAddToCart(int id) // Xử lý hành động "Thêm vào giỏ hàng" khi form được gửi
        {
            var gioHang = HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>(); // Lấy giỏ hàng từ Session
            var item = gioHang.SingleOrDefault(p => p.MaHh == id); // Tìm kiếm sản phẩm trong giỏ hàng

            if (item == null)
            {
                var hangHoa = _db.HangHoas.SingleOrDefault(p => p.MaHh == id); // Lấy thông tin sản phẩm từ cơ sở dữ liệu

                if (hangHoa == null)
                {
                    TempData["Message"] = $"Không tìm thấy hàng hóa có mã {id}";
                    return Redirect("/404");
                }

                item = new CartItem
                {
                    MaHh = hangHoa.MaHh,
                    TenHH = hangHoa.TenHh,
                    DonGia = hangHoa.DonGia ?? 0,
                    Hinh = hangHoa.Hinh ?? string.Empty,
                    SoLuong = 1, // Số lượng mặc định
                    ThanhTien = (hangHoa.DonGia ?? 0) * 1 // Tính ThanhTien
                };

                gioHang.Add(item);
            }
            else
            {
                item.SoLuong++; // Tăng số lượng nếu sản phẩm đã tồn tại trong giỏ hàng
                item.ThanhTien = item.SoLuong * item.DonGia; // Cập nhật ThanhTien
            }

            HttpContext.Session.Set(MySetting.CART_KEY, gioHang); // Lưu giỏ hàng vào Session

            return RedirectToPage("/Cart/index"); // Chuyển hướng đến trang chủ hoặc trang khác tùy ý
        }


    }
}

