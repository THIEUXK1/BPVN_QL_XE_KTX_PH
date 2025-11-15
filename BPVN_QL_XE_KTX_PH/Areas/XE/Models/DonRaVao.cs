using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BPVN_QL_XE_KTX_PH.Areas.XE.Models;

[Table("DonRaVao")]
public partial class DonRaVao
{
    [Key]
    [Column("id_don")]
    public int IdDon { get; set; }

    [Column("id_loai_don")]
    public int? IdLoaiDon { get; set; }

    [Column("id_nguoi_tao")]
    public int? IdNguoiTao { get; set; }

    [Column("stt")]
    public int? Stt { get; set; }

    [Column("ngay_dang_ky")]
    public DateTime? NgayDangKy { get; set; }

    [Column("bo_phan_dang_ky")]
    [StringLength(100)]
    public string? BoPhanDangKy { get; set; }

    [Column("ten_nha_thau")]
    [StringLength(255)]
    public string? TenNhaThau { get; set; }

    [Column("bien_so_xe")]
    [StringLength(50)]
    public string? BienSoXe { get; set; }

    [Column("ten_nguoi_lai_xe")]
    [StringLength(255)]
    public string? TenNguoiLaiXe { get; set; }

    [Column("hang_hoa_mang_vao")]
    [StringLength(255)]
    public string? HangHoaMangVao { get; set; }

    [Column("muc_dich")]
    [StringLength(255)]
    public string? MucDich { get; set; }

    [Column("cong_vao")]
    [StringLength(50)]
    public string? CongVao { get; set; }

    [Column("gio_vao_du_kien")]
    public DateTime? GioVaoDuKien { get; set; }

    [Column("thoi_gian_vao")]
    public DateTime? ThoiGianVao { get; set; }

    [Column("thoi_gian_ra")]
    public DateTime? ThoiGianRa { get; set; }

    [Column("trang_thai")]
    [StringLength(50)]
    public string? TrangThai { get; set; }

    [Column("nguoi_duyet")]
    public int? NguoiDuyet { get; set; }

    [Column("ghi_chu_duyet")]
    public string? GhiChuDuyet { get; set; }

    [Column("tieu_de")]
    [StringLength(255)]
    public string? TieuDe { get; set; }

    [Column("noi_dung")]
    public string? NoiDung { get; set; }

    [Column("ngay_tao")]
    public DateTime? NgayTao { get; set; }

    [Column("ngay_cap_nhat")]
    public DateTime? NgayCapNhat { get; set; }

    public byte[]? Anh { get; set; }

    [StringLength(500)]
    public string? DuongDan { get; set; }

    [Column("Ma_don")]
    [StringLength(50)]
    public string? MaDon { get; set; }

    [Column("trang_thai_nhap")]
    public int? TrangThaiNhap { get; set; }

    [Column("id_nguoi_xac_nhan_cho")]
    public int? IdNguoiXacNhanCho { get; set; }

    [InverseProperty("IdDonNavigation")]
    public virtual ICollection<ChiTietDonRaVao> ChiTietDonRaVaos { get; set; } = new List<ChiTietDonRaVao>();

    [ForeignKey("IdLoaiDon")]
    [InverseProperty("DonRaVaos")]
    public virtual LoaiDon? IdLoaiDonNavigation { get; set; }

    [ForeignKey("IdNguoiTao")]
    [InverseProperty("DonRaVaos")]
    public virtual NguoiDung? IdNguoiTaoNavigation { get; set; }
}
