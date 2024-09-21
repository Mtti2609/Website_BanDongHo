using Tshop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using Tshop.Areas.Admin.Models.Authentication;

namespace Tshop.Areas.Admin.Controllers
{
	[Area("admin")]
    [Route("admin")]
    public class HomeAdminController : Controller
    {
        private readonly TshopContext _db;

        public HomeAdminController(TshopContext db)
        {
            _db = db;
        }

        [Route("")]
        [Route("Index")]
        [Authentication]
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult DangNhap()
        {
            if(HttpContext.Session.GetString("Email") ==null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult DangNhap(NhanVien nhanVien)
        {
            if(HttpContext.Session.GetString("Email") ==null)
            {
                var e=_db.NhanViens.Where(x=>x.Email.Equals(nhanVien.Email) && x.MatKhau.Equals(nhanVien.MatKhau)).FirstOrDefault();
                if(e!=null)
                {
                    HttpContext.Session.SetString("Email", e.Email.ToString());
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
        public IActionResult DangXuat(NhanVien nhanVien)
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("Email");
            return RedirectToAction("DangNhap","HomeAdmin");
        }
        [Route("DanhMucHangHoa")]
        [Authentication]
        public async Task<IActionResult> DanhMucHangHoaAsync()
        {
            
           
            return View(await _db.HangHoas.OrderByDescending(p => p.MaHh).ToListAsync());
        }
        [Route("DanhMuc")]
        [Authentication]
        public async Task<IActionResult> DanhMucAsync()
        {

            return View(await _db.Loais.OrderByDescending(p => p.MaLoai).ToListAsync());
        }
        [Route("DonHang")]
        [Authentication]
        public async Task<IActionResult> DonHang()
        {

            return View(await _db.HoaDons.OrderByDescending(p => p.MaHd).ToListAsync());
        }
        [Route("ThemHangHoaMoi")]
        [HttpGet]
        public IActionResult ThemHangHoaMoi()
        {
            
            ViewBag.MaLoai = new SelectList(_db.Loais, "MaLoai", "TenLoai");
            ViewBag.MaNcc = new SelectList(_db.NhaCungCaps, "MaNcc", "TenCongTy");
            return View();
        }

        [Route("ThemHangHoaMoi")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemHangHoaMoi(HangHoa hangHoa)
        {
            if (!ModelState.IsValid)
            {
                return View(hangHoa);
            }

            _db.HangHoas.Add(hangHoa);
            _db.SaveChanges();
            TempData["Message"] = "Sản phẩm đã thêm";
            return RedirectToAction("DanhMucHangHoa");
        }

        [Route("EditHangHoa")]
        [HttpGet]
        public IActionResult EditHangHoa(int maHh)
        {
            ViewBag.MaHh = new SelectList(_db.HangHoas, "MaHh", "MaHh");
            ViewBag.MaLoai = new SelectList(_db.Loais, "MaLoai", "TenLoai");
            ViewBag.MaNcc = new SelectList(_db.NhaCungCaps, "MaNcc", "TenCongTy");
            var hangHoa = _db.HangHoas.Find(maHh);
            if (hangHoa == null)
            {
                return NotFound();
            }
            return View(hangHoa);
        }

        [Route("EditHangHoa")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditHangHoa(HangHoa hangHoa)
        {
            if (!ModelState.IsValid)
            {
                return View(hangHoa);
            }

            _db.Entry(hangHoa).State = EntityState.Modified;
            _db.SaveChanges();
            TempData["Message"] = "Sản phẩm đã chỉnh sửa";
            return RedirectToAction("DanhMucHangHoa","HomeAdmin");
        }
        [Route("ThemDanhMucMoi")]
        [HttpGet]
        public IActionResult ThemDanhMucMoi()
        {
            
            return View();
        }

        [Route("ThemDanhMucMoi")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemDanhMucMoi(Loai loai)
        {
            if (!ModelState.IsValid)
            {
                return View(loai);
            }

            _db.Loais.Add(loai);
            _db.SaveChanges();
            TempData["Message"] = "Danh mục đã thêm";
            return RedirectToAction("DanhMuc");
        }
        [Route("EditDanhMuc")]
        [HttpGet]
        public IActionResult EditDanhMuc(int maloai)
        {
            
            var loai = _db.Loais.Find(maloai);
            if (loai == null)
            {
                return NotFound();
            }
            return View(loai);
        }

        [Route("EditDanhMuc")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditDanhMuc(Loai loai)
        {
            if (!ModelState.IsValid)
            {
                return View(loai);
            }

            _db.Entry(loai).State = EntityState.Modified;
            _db.SaveChanges();
            TempData["Message"] = "Danh mục đã chỉnh sửa";
            return RedirectToAction("DanhMuc", "HomeAdmin");
        }
        [Route("XoaSanPham")]
        [HttpGet]
        
        
        public IActionResult XoaSanPham(int maHh)
        {
            // Kiểm tra xem sản phẩm có tồn tại không
            var sanPham = _db.HangHoas.Where(x => x.MaLoai==maHh).ToList();
            if (sanPham.Count()>0)
            {
                TempData["Message"] = "Không xóa được";
                // Nếu không tìm thấy sản phẩm, chuyển hướng đến trang lỗi hoặc thông báo lỗi
                return RedirectToAction("DanhMucHangHoa","HomeAdmin");
            }
            var anhSanPhams=_db.HangHoas.Where(x=>x.MaLoai==maHh).ToList();
            if (anhSanPhams.Any()) _db.RemoveRange(anhSanPhams);
            _db.Remove(_db.HangHoas.Find(maHh));
            _db.SaveChanges();
            TempData["Message"] = "Sản phẩm đã được xóa";
            return RedirectToAction("DanhMucHangHoa", "HomeAdmin");
        }
        [Route("XoaDanhMuc")]
        [HttpGet]


        public IActionResult XoaDanhMuc(int maLoai)
        {
            // Kiểm tra xem sản phẩm có tồn tại không
            var sanPham = _db.Loais.Where(x => x.MaLoai == maLoai).ToList();
            if (sanPham.Count() > 0)
            {
                TempData["Message"] = "Không xóa được";
                // Nếu không tìm thấy sản phẩm, chuyển hướng đến trang lỗi hoặc thông báo lỗi
                return RedirectToAction("DanhMuc", "HomeAdmin");
            }
            var anhSanPhams = _db.Loais.Where(x => x.MaLoai == maLoai).ToList();
            if (anhSanPhams.Any()) _db.RemoveRange(anhSanPhams);
            _db.Remove(_db.Loais.Find(maLoai));
            _db.SaveChanges();
            TempData["Message"] = "Sản phẩm đã được xóa";
            return RedirectToAction("DanhMuc", "HomeAdmin");
        }

    }
}
