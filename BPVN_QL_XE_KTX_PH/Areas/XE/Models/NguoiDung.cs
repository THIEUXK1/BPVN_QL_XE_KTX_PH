using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BPVN_QL_XE_KTX_PH.Areas.XE.Models;

[Table("NguoiDung")]
public partial class NguoiDung
{
    [Key]
    [Column("id_nguoi_dung")]
    public int IdNguoiDung { get; set; }

    [Column("ho_ten")]
    [StringLength(255)]
    public string? HoTen { get; set; }

    [Column("email")]
    [StringLength(255)]
    public string? Email { get; set; }

    [Column("mat_khau")]
    [StringLength(255)]
    public string? MatKhau { get; set; }

    [Column("so_dien_thoai")]
    [StringLength(20)]
    public string? SoDienThoai { get; set; }

    [Column("phong_ban")]
    [StringLength(100)]
    public string? PhongBan { get; set; }

    [Column("vai_tro")]
    [StringLength(50)]
    public string? VaiTro { get; set; }

    [Column("trang_thai")]
    [StringLength(20)]
    public string? TrangThai { get; set; }

    [Column("ngay_tao")]
    public DateTime? NgayTao { get; set; }

    [Column("ngay_cap_nhat")]
    public DateTime? NgayCapNhat { get; set; }

    [InverseProperty("IdNguoiTaoNavigation")]
    public virtual ICollection<DonRaVao> DonRaVaos { get; set; } = new List<DonRaVao>();
}
