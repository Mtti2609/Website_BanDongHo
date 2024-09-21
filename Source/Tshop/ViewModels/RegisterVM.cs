using System.ComponentModel.DataAnnotations;

namespace Tshop.ViewModels
{
    public class RegisterVM
    {
        [Key]
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "{0} không được để trống")]
        [MaxLength(20, ErrorMessage = "{0} tối đa 20 kí tự")]
        public string MaKh { get; set; }


        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "{0} không được để trống")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }

        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "{0} không được để trống")]
        [MaxLength(50, ErrorMessage = "{0} tối đa 50 kí tự")]
        public string HoTen { get; set; }

        [Display(Name = "Giới tính")]
        public bool GioiTinh { get; set; } = true; // Sử dụng string hoặc enum thay vì boolean

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? NgaySinh { get; set; }
    }
}
