using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tshop.Data;
using Tshop.Helpers;
using Tshop.ViewModels;

namespace Tshop.Pages.KhachHang
{
    public class DangKyModel : PageModel
    {
        private readonly TshopContext _db;
        private readonly IMapper _mapper;

        public DangKyModel(TshopContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [BindProperty]
        public RegisterVM RegisterVM { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Kiểm tra ModelState trước hết
            if (!ModelState.IsValid)
            {
                // Nếu ModelState không hợp lệ, trả về trang đăng ký với các lỗi hiển thị
                return Page();
            }

            try
            {
                // Tạo một đối tượng KhachHang từ dữ liệu trong RegisterVM
                var khachHang = _mapper.Map<Tshop.Data.KhachHang>(RegisterVM);

                // Tạo một khóa ngẫu nhiên và mã hóa mật khẩu trước khi lưu vào cơ sở dữ liệu
                khachHang.RandomKey = MyUtil.GenerateRamdomKey();
                khachHang.MatKhau = RegisterVM.MatKhau.ToMd5Hash(khachHang.RandomKey);

                // Đặt các giá trị khác của khách hàng
                khachHang.HieuLuc = true;
                khachHang.VaiTro = 0;

                // Thêm khách hàng vào cơ sở dữ liệu và lưu thay đổi
                _db.Add(khachHang);
                await _db.SaveChangesAsync();

                // Chuyển hướng người dùng đến trang chính sau khi đăng ký thành công
                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ
                // Ví dụ: Ghi log ngoại lệ
                // LogHelper.LogError(ex);

                // Hiển thị thông báo lỗi cho người dùng
                ModelState.AddModelError("", "Đã xảy ra lỗi khi đăng ký. Vui lòng thử lại sau.");

                // Trả về trang đăng ký với thông báo lỗi
                return Page();
            }
        }
    }
}
