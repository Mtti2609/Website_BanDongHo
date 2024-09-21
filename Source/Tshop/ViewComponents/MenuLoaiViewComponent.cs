using Tshop.Data;
using Tshop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Tshop.ViewComponents
{
	public class MenuLoaiViewComponent : ViewComponent
	{
		private readonly TshopContext db;

		public MenuLoaiViewComponent(TshopContext context) => db = context;

		public IViewComponentResult Invoke()
		{
			var data = db.Loais.Select(lo => new MenuLoaiVM
			{
				MaLoai = (int)lo.MaLoai,
				TenLoai = lo.TenLoai,
				SoLuong = lo.HangHoas.Count
			}).OrderBy(p => p.TenLoai);

			return View(data); // Default.cshtml
			//return View("Default", data);
		}
	}
}
