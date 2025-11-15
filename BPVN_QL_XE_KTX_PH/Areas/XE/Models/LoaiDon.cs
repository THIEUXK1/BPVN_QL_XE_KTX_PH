using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BPVN_QL_XE_KTX_PH.Areas.XE.Models;

[Table("LoaiDon")]
public partial class LoaiDon
{
    [Key]
    [Column("id_loai_don")]
    public int IdLoaiDon { get; set; }

    [Column("ten_loai_don")]
    [StringLength(100)]
    public string? TenLoaiDon { get; set; }

    [Column("mo_ta")]
    [StringLength(255)]
    public string? MoTa { get; set; }

    [Column("trang_thai")]
    [StringLength(20)]
    public string? TrangThai { get; set; }

    [Column("ngay_tao")]
    public DateTime? NgayTao { get; set; }

    [Column("ngay_cap_nhat")]
    public DateTime? NgayCapNhat { get; set; }

    [InverseProperty("IdLoaiDonNavigation")]
    public virtual ICollection<DonRaVao> DonRaVaos { get; set; } = new List<DonRaVao>();
}
