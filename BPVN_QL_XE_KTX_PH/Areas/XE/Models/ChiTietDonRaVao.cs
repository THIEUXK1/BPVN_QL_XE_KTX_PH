using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BPVN_QL_XE_KTX_PH.Areas.XE.Models;

[Table("ChiTietDonRaVao")]
public partial class ChiTietDonRaVao
{
    [Key]
    [Column("id_chi_tiet")]
    public int IdChiTiet { get; set; }

    [Column("id_don")]
    public int IdDon { get; set; }

    [Column("bien_so_xe")]
    [StringLength(50)]
    public string? BienSoXe { get; set; }

    [Column("ten_nguoi_lai_xe")]
    [StringLength(255)]
    public string? TenNguoiLaiXe { get; set; }

    [Column("hang_hoa")]
    [StringLength(255)]
    public string? HangHoa { get; set; }

    [Column("so_luong", TypeName = "decimal(18, 2)")]
    public decimal? SoLuong { get; set; }

    [Column("don_vi")]
    [StringLength(50)]
    public string? DonVi { get; set; }

    [Column("cong_ra")]
    [StringLength(50)]
    public string? CongRa { get; set; }

    [Column("thoi_gian_ra")]
    public DateTime? ThoiGianRa { get; set; }

    [Column("tinh_trang_hang")]
    [StringLength(255)]
    public string? TinhTrangHang { get; set; }

    [Column("ghi_chu")]
    [StringLength(255)]
    public string? GhiChu { get; set; }

    [Column("ngay_tao")]
    public DateTime? NgayTao { get; set; }

    [Column("ngay_cap_nhat")]
    public DateTime? NgayCapNhat { get; set; }

    [ForeignKey("IdDon")]
    [InverseProperty("ChiTietDonRaVaos")]
    public virtual DonRaVao IdDonNavigation { get; set; } = null!;
}
