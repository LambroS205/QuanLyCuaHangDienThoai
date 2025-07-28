using QuanLyCuaHangDienThoai.DAL;
using System;
using System.Data;

namespace QuanLyCuaHangDienThoai.DAL
{
    public class ThongKeDAL
    {
        public DataTable GetDoanhThu(DateTime fromDate, DateTime toDate)
        {
            string query = "SELECT CAST(NgayLap AS DATE) AS Ngay, SUM(TongTien) AS DoanhThu FROM HoaDon WHERE NgayLap BETWEEN @fromDate AND @toDate GROUP BY CAST(NgayLap AS DATE) ORDER BY Ngay";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { fromDate, toDate.AddDays(1) });
        }

        public DataTable GetTopSanPhamBanChay(DateTime fromDate, DateTime toDate)
        {
            string query = @"
                SELECT TOP 10 sp.MaSP, sp.TenSP, SUM(cthd.SoLuong) AS SoLuongBan
                FROM ChiTietHoaDon cthd
                JOIN SanPham sp ON cthd.MaSP = sp.MaSP
                JOIN HoaDon hd ON cthd.MaHD = hd.MaHD
                WHERE hd.NgayLap BETWEEN @fromDate AND @toDate
                GROUP BY sp.MaSP, sp.TenSP
                ORDER BY SoLuongBan DESC";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { fromDate, toDate.AddDays(1) });
        }

        public DataTable GetLichSuHoaDon(DateTime fromDate, DateTime toDate)
        {
            string query = @"
                SELECT hd.MaHD, hd.NgayLap, nv.HoTen AS TenNhanVien, ISNULL(kh.HoTen, N'Khách vãng lai') AS TenKhachHang, hd.TongTien
                FROM HoaDon hd
                JOIN NhanVien nv ON hd.MaNV = nv.MaNV
                LEFT JOIN KhachHang kh ON hd.MaKH = kh.MaKH
                WHERE hd.NgayLap BETWEEN @fromDate AND @toDate
                ORDER BY hd.NgayLap DESC";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { fromDate, toDate.AddDays(1) });
        }

        // --- PHƯƠNG THỨC MỚI ---
        public DataTable GetLichSuPhieuNhap(DateTime fromDate, DateTime toDate)
        {
            string query = @"
                SELECT pn.MaPN, pn.NgayNhap, ncc.TenNCC, nv.HoTen AS TenNhanVien, pn.TongTien
                FROM PhieuNhap pn
                JOIN NhaCungCap ncc ON pn.MaNCC = ncc.MaNCC
                JOIN NhanVien nv ON pn.MaNV = nv.MaNV
                WHERE pn.NgayNhap BETWEEN @fromDate AND @toDate
                ORDER BY pn.NgayNhap DESC";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { fromDate, toDate.AddDays(1) });
        }
    }
}
