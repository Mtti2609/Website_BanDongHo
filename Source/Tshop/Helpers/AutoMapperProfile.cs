using AutoMapper;
using Tshop.Data;
using Tshop.ViewModels;

namespace Tshop.Helpers
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<RegisterVM, KhachHang>();
			//.ForMember(kh => kh.HoTen, option => option.MapFrom(RegisterVM => RegisterVM.HoTen))
			//.ReverseMap();
		}
	}
}
