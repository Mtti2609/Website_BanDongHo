using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tshop.Data;
using Tshop.ViewModels;

namespace Tshop.Pages.HangHoa
{
	public class SearchModel : PageModel
	{
		private readonly TshopContext _db;

		public SearchModel(TshopContext context)
		{
			_db = context;
		}

		[BindProperty]
		public string Query { get; set; }
		public IList<HangHoaVM> SearchResults { get; set; }

		public async Task OnGetAsync(string query)
		{
			Query = query;
			SearchResults = await _db.HangHoas
				.Include(p => p.MaLoaiNavigation)
				.Where(p => string.IsNullOrEmpty(query) || p.TenHh.Contains(query))
				.Select(p => new HangHoaVM
				{
					MaHh = p.MaHh,
					TenHH = p.TenHh,
					DonGia = p.DonGia ?? 0,
					Hinh = p.Hinh ?? "",
					MoTaNgan = p.MoTaDonVi ?? ""
				})
				.ToListAsync();
		}
	}
}
