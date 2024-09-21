using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tshop.Data
{
    public partial class HangHoa
    {
        [Key]
        public int MaHh { get; set; }

        [Required(ErrorMessage = "Tên hàng hóa là bắt buộc")]
        public string TenHh { get; set; }

        public string TenAlias { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại hàng hóa")]
        public int MaLoai { get; set; }

        public string MoTaDonVi { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập đơn giá")]
        [Range(0, double.MaxValue, ErrorMessage = "Đơn giá không hợp lệ")]
        public double? DonGia { get; set; }
        
        public string Hinh { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày sản xuất")]
        [DataType(DataType.Date)]
        public DateTime NgaySx { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giảm giá")]
        [Range(0, 100, ErrorMessage = "Giảm giá phải nằm trong khoảng từ 0 đến 100")]
        public double GiamGia { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lần xem")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lần xem không hợp lệ")]
        public int SoLanXem { get; set; }

        public string MoTa { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn nhà cung cấp")]
        public string MaNcc { get; set; }

        [ForeignKey("MaLoai")]
        public virtual Loai? MaLoaiNavigation { get; set; }

        [ForeignKey("MaNcc")]
        public virtual NhaCungCap? MaNccNavigation { get; set; }

        public virtual ICollection<BanBe> BanBes { get; set; } = new List<BanBe>();

        public virtual ICollection<ChiTietHd> ChiTietHds { get; set; } = new List<ChiTietHd>();

        public virtual ICollection<YeuThich> YeuThiches { get; set; } = new List<YeuThich>();

        
       
    }
}
