using System;
using System.Collections.Generic;
using BPVN_QL_XE_KTX_PH.Areas.XE.Models;
using Microsoft.EntityFrameworkCore;

namespace BPVN_QL_XE_KTX_PH.Context;

public partial class DBXeContext : DbContext
{
    public DBXeContext()
    {
    }

    public DBXeContext(DbContextOptions<DBXeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChiTietDonRaVao> ChiTietDonRaVaos { get; set; }

    public virtual DbSet<DonRaVao> DonRaVaos { get; set; }

    public virtual DbSet<LoaiDon> LoaiDons { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=10.0.60.33;Initial Catalog=DBDonXet;Persist Security Info=True;User ID=sa;Password=BestP@cific;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietDonRaVao>(entity =>
        {
            entity.HasKey(e => e.IdChiTiet).HasName("PK__ChiTietD__6D3DC8DC191BCF56");

            entity.Property(e => e.NgayCapNhat).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.NgayTao).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.IdDonNavigation).WithMany(p => p.ChiTietDonRaVaos).HasConstraintName("fk_chitiet_don");
        });

        modelBuilder.Entity<DonRaVao>(entity =>
        {
            entity.HasKey(e => e.IdDon).HasName("PK__DonRaVao__D5EAB2177FBCDD00");

            entity.Property(e => e.NgayCapNhat).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.NgayTao).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.IdLoaiDonNavigation).WithMany(p => p.DonRaVaos)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_don_loaidon");

            entity.HasOne(d => d.IdNguoiTaoNavigation).WithMany(p => p.DonRaVaos)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_don_nguoidung");
        });

        modelBuilder.Entity<LoaiDon>(entity =>
        {
            entity.HasKey(e => e.IdLoaiDon).HasName("PK__LoaiDon__EF9A58DC24A2BD22");

            entity.Property(e => e.NgayCapNhat).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.NgayTao).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.IdNguoiDung).HasName("PK__NguoiDun__75D6A11EC4E32E39");

            entity.ToTable("NguoiDung", tb => tb.HasTrigger("trg_NguoiDung_UpdateNgayCapNhat"));

            entity.Property(e => e.NgayCapNhat).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.NgayTao).HasDefaultValueSql("(sysutcdatetime())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
