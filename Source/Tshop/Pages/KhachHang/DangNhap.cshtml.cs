using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tshop.Data;
using Tshop.Helpers;
using Tshop.ViewModels;

namespace Tshop.Pages.KhachHang
{
    public class DangNhapModel : PageModel
    {
        private readonly TshopContext _db;
        private readonly IMapper _mapper;

        public DangNhapModel(TshopContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }

        [BindProperty]
        public LoginVM LoginVM { get; set; }

        public string ReturnUrl { get; set; }

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var khachHang = _db.KhachHangs.SingleOrDefault(kh => kh.MaKh == LoginVM.UserName);
                if (khachHang == null)
                {
                    ModelState.AddModelError("loi", "Không có khách hàng này");
                }
                else
                {
                    if (!khachHang.HieuLuc)
                    {
                        ModelState.AddModelError("loi", "Tài khoản đã bị khóa. Vui lòng liên hệ Admin.");
                    }
                    else
                    {
                        if (khachHang.MatKhau != LoginVM.Password.ToMd5Hash(khachHang.RandomKey))
                        {
                            ModelState.AddModelError("loi", "Sai thông tin đăng nhập");
                        }
                        else
                        {
                            var claims = new List<Claim> {
                        
                        new Claim(ClaimTypes.Name, khachHang.HoTen),
                        new Claim(MySetting.CLAIM_CUSTOMERID, khachHang.MaKh),

                        //claim - role động
                        new Claim(ClaimTypes.Role, "Customer")
                    };

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                            await HttpContext.SignInAsync(claimsPrincipal);

                            if (Url.IsLocalUrl(ReturnUrl))
                            {
                                return LocalRedirect(ReturnUrl);
                            }
                            else
                            {
                                return RedirectToPage("/Index");
                            }
                        }
                    }
                }
            }
            return Page();
        }

    }
}
