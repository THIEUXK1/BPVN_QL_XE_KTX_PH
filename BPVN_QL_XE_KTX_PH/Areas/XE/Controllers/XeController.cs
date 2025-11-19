using BPVN_QL_XE_KTX_PH.Areas.XE.Models;
using BPVN_QL_XE_KTX_PH.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BPVN_QL_XE_KTX_PH.Areas.XE.Controllers
{
    [Area("Xe")]
    public class XeController : Controller
    {
        public DBXeContext _context;
        public XeController()
        {
            _context = new DBXeContext();
        }
        #region Trang đăng nhập
        [HttpGet("/DonXetDuyet/DangNhap")]
        public IActionResult DangNhap()
        {
            return View();
        }

        [HttpPost("/DonXetDuyet/DangNhap")]
        public IActionResult DangNhap(string email, string matKhau, bool rememberMe = false)
        {
            // --- Kiểm tra nhập liệu ---
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(matKhau))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin.";
                return View();
            }

            // --- Kiểm tra tài khoản ---
            var user = _context.NguoiDungs
                .FirstOrDefault(u => u.Email == email && u.MatKhau == matKhau && u.TrangThai != "Khóa");

            if (user == null)
            {
                ViewBag.Error = "Tài khoản hoặc mật khẩu không đúng.";
                return View();
            }

            // --- Lưu Session ---
            HttpContext.Session.SetInt32("UserId", user.IdNguoiDung);
            HttpContext.Session.SetString("UserName", user.HoTen ?? "");
            HttpContext.Session.SetString("UserRole", user.VaiTro ?? "");
            HttpContext.Session.SetString("UserEmail", user.Email ?? "");

            // --- Lưu cookie ghi nhớ email nếu chọn "Ghi nhớ tài khoản" ---
            if (rememberMe)
            {
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(30),
                    HttpOnly = true
                };
                Response.Cookies.Append("RememberedEmail", email, cookieOptions);
            }
            else
            {
                Response.Cookies.Delete("RememberedEmail");
            }

            // --- Chuyển hướng về TrangChu ---
            return Redirect("/DonXetDuyet/TrangChu");
        }

        [HttpGet("/DonXetDuyet/DangXuat")]
        public IActionResult DangXuat()
        {
            HttpContext.Session.Clear();
            return Redirect("/DonXetDuyet/TrangChu");
        }
        #endregion

        #region Trang xem
        [HttpGet("/DonXetDuyet/TrangChu")]
        public IActionResult TrangChu()
        {
            var dons = _context.DonRaVaos.Where(c => c.TrangThaiNhap == 1)
                .Include(d => d.IdNguoiTaoNavigation)   // Người tạo
                .OrderByDescending(d => d.NgayTao)
                .Select(d => new DonViewModel
                {
                    IdDon = d.IdDon,
                    TieuDe = d.TieuDe ?? "",
                    BoPhanDangKy = d.BoPhanDangKy,
                    TenNguoiTao = d.IdNguoiTaoNavigation != null ? d.IdNguoiTaoNavigation.HoTen : "Không xác định",
                    Anh = d.Anh,
                    NgayTao = d.NgayTao,
                    TrangThai = string.IsNullOrEmpty(d.TrangThai) ? "Đang chờ xét duyệt" : d.TrangThai,
                    TenNguoiDuyet = (_context.NguoiDungs.FirstOrDefault(c => c.IdNguoiDung == d.NguoiDuyet).HoTen),
                    MucDich = d.MucDich,
                    CongVao = d.CongVao,
                    MaDon = d.MaDon,
                })
                .ToList();

            var trangThaiList = dons
                .Select(d => d.TrangThai.Trim())
                .Distinct()
                .ToList();

            ViewBag.TrangThaiList = trangThaiList;

            return View(dons);
        }

        // GET: /DonXetDuyet/ChiTietDon/{id}
        [HttpGet("/DonXetDuyet/ChiTietDon/{id}")]
        public IActionResult CTDon(int id)
        {
            var don = _context.DonRaVaos
                .Include(d => d.IdNguoiTaoNavigation)
                .Where(d => d.IdDon == id)
                .Select(d => new DonViewModel
                {
                    IdDon = d.IdDon,
                    TieuDe = d.TieuDe ?? "",
                    BoPhanDangKy = d.BoPhanDangKy,
                    TenNguoiTao = d.IdNguoiTaoNavigation != null ? d.IdNguoiTaoNavigation.HoTen : "Không xác định",
                    Anh = d.Anh,
                    NgayTao = d.NgayTao,
                    GioVaoDuKien = d.GioVaoDuKien,
                    ThoiGianVao = d.ThoiGianVao,
                    ThoiGianRa = d.ThoiGianRa,
                    TrangThai = string.IsNullOrEmpty(d.TrangThai) ? "Đang chờ xét duyệt" : d.TrangThai,
                    MucDich = d.MucDich,
                    TenNguoiDuyet = (_context.NguoiDungs.FirstOrDefault(c => c.IdNguoiDung == d.NguoiDuyet).HoTen),
                    CongVao = d.CongVao,
                    MaDon = d.MaDon,
                    DuongDan = d.DuongDan,
                })
                .FirstOrDefault();

            if (don == null)
                return NotFound();

            return View(don);
        }


        public class DonViewModel
        {
            public int IdDon { get; set; }
            public string TieuDe { get; set; } = string.Empty;
            public string? BoPhanDangKy { get; set; }
            public string? TenNguoiTao { get; set; }
            public byte[]? Anh { get; set; }
            public DateTime? NgayTao { get; set; }
            public DateTime? GioVaoDuKien { get; set; }
            public DateTime? ThoiGianVao { get; set; }
            public DateTime? ThoiGianRa { get; set; }
            public string TrangThai { get; set; } = "Đang chờ xét duyệt";
            public string? TenNguoiDuyet { get; set; }
            public string? MucDich { get; set; }
            public string? CongVao { get; set; }
            public int? TrangThaiNhap { get; set; }
            public int? IdNguoiXacNhanCho { get; set; }
            public string? MaDon { get; set; }
            public string? DuongDan { get; set; }

        }
        #endregion

        #region Trang tạo
        // GET: /DonXetDuyet/TrangTao
        [HttpGet("/DonXetDuyet/TrangTao")]
        public IActionResult TrangTao()
        {
            // --- Kiểm tra đăng nhập ---
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Redirect("/DonXetDuyet/DangNhap");
            }

            // --- Lấy danh sách loại đơn ---
            ViewBag.LoaiDonList = _context.LoaiDons
                .Where(x => x.TrangThai == "hoạt động")
                .ToList();

            return View();
        }

        [HttpPost("/DonXetDuyet/addTrangTao")]
        public IActionResult TrangTao(DonRaVao model, IFormFile AnhFile, IFormFile WordFile)
        {
            // Lấy UserId từ session
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Redirect("/DonXetDuyet/DangNhap");

            // Kiểm tra bắt buộc ảnh minh họa phải có
            if (AnhFile == null || AnhFile.Length == 0)
                return BadRequest("Vui lòng tải lên ảnh minh họa.");

            // Gán thông tin chung cho đơn
            model.MaDon = "BPVN-" + DateTime.Now.ToString("yyyyMMddHHmmss");
            model.IdNguoiTao = userId.Value;
            model.NgayTao = DateTime.Now;

            // Lưu ảnh minh họa
            using (var ms = new MemoryStream())
            {
                AnhFile.CopyTo(ms);
                model.Anh = ms.ToArray();
            }

            // Lưu file Word/Excel nếu có
            if (WordFile != null && WordFile.Length > 0)
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LuuDonXe");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var fileName = $"{model.MaDon}_{Path.GetFileName(WordFile.FileName)}";
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    WordFile.CopyTo(stream);
                }

                // Lưu đường dẫn tương đối vào database
                model.DuongDan = $"/LuuDonXe/{fileName}";
            }

            // Lưu dữ liệu vào database
            _context.DonRaVaos.Add(model);
            _context.SaveChanges();

            return Ok("Đã lưu đơn thành công");
        }

        #endregion

        #region Trang Chờ
        [HttpGet("/DonXetDuyet/TrangCho")]
        public IActionResult TrangCho()
        {
            // --- Kiểm tra đăng nhập ---
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Redirect("/DonXetDuyet/DangNhap");

            // --- Lấy danh sách đơn theo người tạo ---
            var dons = _context.DonRaVaos
                .Where(d => d.IdNguoiTao == userId)
                .Include(d => d.IdNguoiTaoNavigation)
                .OrderByDescending(d => d.NgayTao)
                .Select(d => new DonViewModel
                {
                    IdDon = d.IdDon,
                    TieuDe = d.TieuDe ?? "",
                    BoPhanDangKy = d.BoPhanDangKy,
                    TenNguoiTao = d.IdNguoiTaoNavigation != null ? d.IdNguoiTaoNavigation.HoTen : "Không xác định",
                    Anh = d.Anh,
                    NgayTao = d.NgayTao,
                    GioVaoDuKien = d.GioVaoDuKien,
                    ThoiGianVao = d.ThoiGianVao,
                    ThoiGianRa = d.ThoiGianRa,
                    TrangThai = string.IsNullOrEmpty(d.TrangThai) ? "Đang chờ xét duyệt" : d.TrangThai,
                    TenNguoiDuyet = (_context.NguoiDungs.FirstOrDefault(c => c.IdNguoiDung == d.NguoiDuyet).HoTen),
                    MucDich = d.MucDich,
                    CongVao = d.CongVao,
                    TrangThaiNhap = d.TrangThaiNhap,
                    IdNguoiXacNhanCho = d.IdNguoiXacNhanCho,
                    DuongDan = d.DuongDan
                })
                .ToList();

            // --- Lấy danh sách trạng thái duy nhất ---
            var trangThaiList = dons
                .Select(d => d.TrangThai.Trim())
                .Distinct()
                .ToList();

            ViewBag.TrangThaiList = trangThaiList;

            return View(dons);
        }

        [HttpPost("/DonXetDuyet/AddTrangCho")]
        public IActionResult AddTrangCho([FromBody] List<int> idDons)
        {
            // --- Kiểm tra đăng nhập ---
            var userId = HttpContext.Session.GetInt32("UserId");
            var userName = HttpContext.Session.GetString("UserName");

            if (userId == null || string.IsNullOrEmpty(userName))
            {
                return Json(new { success = false, message = "Bạn chưa đăng nhập" });
            }

            // --- Lấy các đơn hợp lệ chưa được duyệt ---
            var dons = _context.DonRaVaos
                        .Where(d => idDons.Contains(d.IdDon) && d.TrangThaiNhap != 1)
                        .ToList();

            if (!dons.Any())
                return Json(new { success = false, message = "Không có đơn hợp lệ để đẩy đi" });

            // --- Cập nhật trạng thái duyệt ---
            foreach (var don in dons)
            {
                don.TrangThaiNhap = 1;
                don.IdNguoiXacNhanCho = userId;
            }

            _context.SaveChanges();

            // --- Trả về JSON ---
            var updatedIds = dons.Select(d => new { d.IdDon, d.TrangThai }).ToList();
            return Json(new { success = true, updatedCount = dons.Count, updatedIds });
        }
        #endregion

        #region Trang Duyệt
        [HttpGet("/DonXetDuyet/TrangDuyet")]
        public IActionResult TrangDuyet()
        {
            // --- Kiểm tra đăng nhập ---
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Redirect("/DonXetDuyet/DangNhap");
            }

            // Chỉ cho phép "Quản Lý" xem trang này
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "QuanLy")
            {
                return Redirect("/DonXetDuyet/TrangChu");
            }

            // Lấy danh sách đơn với thông tin người tạo
            var dons = _context.DonRaVaos.Where(c => c.TrangThaiNhap == 1)
                .Include(d => d.IdNguoiTaoNavigation)
                .OrderByDescending(d => d.NgayTao)
                .Select(d => new DonViewModel
                {
                    IdDon = d.IdDon,
                    TieuDe = d.TieuDe ?? "",
                    BoPhanDangKy = d.BoPhanDangKy,
                    TenNguoiTao = d.IdNguoiTaoNavigation != null ? d.IdNguoiTaoNavigation.HoTen : "Không xác định",
                    Anh = d.Anh,
                    NgayTao = d.NgayTao,
                    GioVaoDuKien = d.GioVaoDuKien,
                    ThoiGianVao = d.ThoiGianVao,
                    ThoiGianRa = d.ThoiGianRa,
                    TrangThai = string.IsNullOrEmpty(d.TrangThai) ? "Đang chờ xét duyệt" : d.TrangThai,
                    TenNguoiDuyet = (_context.NguoiDungs.FirstOrDefault(c => c.IdNguoiDung == d.NguoiDuyet).HoTen),
                    MucDich = d.MucDich,
                    CongVao = d.CongVao,
                    MaDon = d.MaDon,
                    DuongDan = d.DuongDan
                })
                .ToList();

            // Lấy danh sách trạng thái duy nhất từ database
            var trangThaiList = dons
                .Select(d => d.TrangThai.Trim())
                .Distinct()
                .ToList();

            // Gửi trạng thái lên ViewBag để hiển thị filter
            ViewBag.TrangThaiList = trangThaiList;

            return View(dons);
        }

        // ViewModel


        [HttpPost("/DonXetDuyet/AddTrangDuyet")]
        public IActionResult AddTrangDuyet([FromBody] List<int> idDons)
        {
            // Lấy thông tin người duyệt từ session
            var userId = HttpContext.Session.GetInt32("UserId");
            var userName = HttpContext.Session.GetString("UserName");

            if (userId == null || string.IsNullOrEmpty(userName))
            {
                return Json(new { success = false, message = "Bạn chưa đăng nhập" });
            }

            // Lấy các đơn chưa duyệt
            var dons = _context.DonRaVaos
                        .Where(d => idDons.Contains(d.IdDon) && d.TrangThai != "Đã duyệt")
                        .ToList();

            if (!dons.Any())
            {
                return Json(new { success = false, message = "Không có đơn hợp lệ để duyệt" });
            }

            // Cập nhật trạng thái và người duyệt
            foreach (var don in dons)
            {
                don.TrangThai = "Đã duyệt";
                don.NguoiDuyet = userId;

            }

            _context.SaveChanges();

            // Trả về JSON với thông tin
            var updatedIds = dons.Select(d => new { d.IdDon, d.TrangThai }).ToList();

            return Json(new { success = true, updatedCount = dons.Count, updatedIds });
        }

        // Controller: DonXetController.cs
        [HttpGet("/DonXetDuyet/CTTrangDuyet/{id}")]
        public IActionResult CTTrangDuyet(int id)
        {
            // --- Kiểm tra đăng nhập ---
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Redirect("/DonXetDuyet/DangNhap");
            }

            // Lấy chi tiết đơn từ database
            var donEntity = _context.DonRaVaos
                .Include(d => d.IdNguoiTaoNavigation)
                .FirstOrDefault(d => d.IdDon == id);

            if (donEntity == null)
            {
                return NotFound("Đơn không tồn tại hoặc bạn không có quyền xem.");
            }

            // Lấy tên người duyệt (nếu có)
            string tenNguoiDuyet;
            if (donEntity.NguoiDuyet.HasValue)
            {
                tenNguoiDuyet = _context.NguoiDungs
                    .Where(c => c.IdNguoiDung == donEntity.NguoiDuyet.Value)
                    .Select(c => c.HoTen)
                    .FirstOrDefault() ?? "-";
            }
            else
            {
                tenNguoiDuyet = "-";
            }

            // Tạo ViewModel
            var don = new DonViewModel
            {
                IdDon = donEntity.IdDon,
                TieuDe = donEntity.TieuDe ?? "",
                BoPhanDangKy = donEntity.BoPhanDangKy,
                TenNguoiTao = donEntity.IdNguoiTaoNavigation?.HoTen ?? "Không xác định",
                Anh = donEntity.Anh,
                NgayTao = donEntity.NgayTao,
                GioVaoDuKien = donEntity.GioVaoDuKien,
                ThoiGianVao = donEntity.ThoiGianVao,
                ThoiGianRa = donEntity.ThoiGianRa,
                TrangThai = string.IsNullOrEmpty(donEntity.TrangThai) ? "Đang chờ xét duyệt" : donEntity.TrangThai,
                TenNguoiDuyet = tenNguoiDuyet,
                MucDich = donEntity.MucDich,
                CongVao = donEntity.CongVao,
                MaDon = donEntity.MaDon,
                DuongDan = donEntity.DuongDan // để view hiển thị link tải file
            };

            return View(don);
        }



        #endregion
    }
}
