using QuanLyCuaHangDienThoai.DTO;
using System.Data;

namespace QuanLyCuaHangDienThoai.DAL
{
    public class SanPhamDAL
    {
        public DataTable LayDanhSachSanPham()
        {
            string query = "SELECT MaSP, TenSP, MaHang, MaLoai, DonGiaBan, SoLuongTon, ThoiGianBaoHanh, HinhAnh, MoTa, TrangThai FROM SanPham";
            return DataProvider.Instance.ExecuteQuery(query);
        }

        public DataTable LayDanhSachHang()
        {
            string query = "SELECT MaHang, TenHang FROM HangSanXuat WHERE TrangThai = 1";
            return DataProvider.Instance.ExecuteQuery(query);
        }

        public DataTable LayDanhSachLoaiSP()
        {
            string query = "SELECT MaLoai, TenLoai FROM LoaiSanPham WHERE TrangThai = 1";
            return DataProvider.Instance.ExecuteQuery(query);
        }

        public bool ThemSanPham(SanPhamDTO sp)
        {
            string query = "INSERT INTO SanPham (MaSP, TenSP, MaHang, MaLoai, DonGiaBan, SoLuongTon, ThoiGianBaoHanh, MoTa, TrangThai, HinhAnh) VALUES ( @MaSP , @TenSP , @MaHang , @MaLoai , @DonGiaBan , @SoLuongTon , @ThoiGianBaoHanh , @MoTa , @TrangThai , @HinhAnh )";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { sp.MaSP, sp.TenSP, sp.MaHang, sp.MaLoai, sp.DonGiaBan, sp.SoLuongTon, sp.ThoiGianBaoHanh, sp.MoTa, sp.TrangThai, sp.HinhAnh });
            return result > 0;
        }

        public bool SuaSanPham(SanPhamDTO sp)
        {
            string query = "UPDATE SanPham SET TenSP = @TenSP , MaHang = @MaHang , MaLoai = @MaLoai , DonGiaBan = @DonGiaBan , SoLuongTon = @SoLuongTon , ThoiGianBaoHanh = @ThoiGianBaoHanh , MoTa = @MoTa , TrangThai = @TrangThai , HinhAnh = @HinhAnh WHERE MaSP = @MaSP";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { sp.TenSP, sp.MaHang, sp.MaLoai, sp.DonGiaBan, sp.SoLuongTon, sp.ThoiGianBaoHanh, sp.MoTa, sp.TrangThai, sp.HinhAnh, sp.MaSP });
            return result > 0;
        }

        public bool XoaSanPham(string maSP)
        {
            string checkChiTietHD = "SELECT COUNT(*) FROM ChiTietHoaDon WHERE MaSP = @maSP";
            int countHD = (int)DataProvider.Instance.ExecuteScalar(checkChiTietHD, new object[] { maSP });

            string checkChiTietPN = "SELECT COUNT(*) FROM ChiTietPhieuNhap WHERE MaSP = @maSP";
            int countPN = (int)DataProvider.Instance.ExecuteScalar(checkChiTietPN, new object[] { maSP });

            if (countHD > 0 || countPN > 0)
            {
                return false;
            }

            string query = "DELETE FROM SanPham WHERE MaSP = @maSP";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { maSP });
            return result > 0;
        }

        public DataTable TimKiemSanPham(string keyword)
        {
            string query = "SELECT MaSP, TenSP, MaHang, MaLoai, DonGiaBan, SoLuongTon, ThoiGianBaoHanh, HinhAnh, MoTa, TrangThai FROM SanPham WHERE MaSP LIKE @keyword OR TenSP LIKE @keyword";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { "%" + keyword + "%" });
        }

        // --- PHƯƠNG THỨC MỚI ---
        public DataTable GetLichSuBanHang(string maSP)
        {
            string query = @"
                SELECT hd.MaHD, hd.NgayLap, nv.HoTen AS TenNhanVien, ISNULL(kh.HoTen, N'Khách vãng lai') AS TenKhachHang, cthd.SoLuong, hd.TongTien
                FROM HoaDon hd
                JOIN ChiTietHoaDon cthd ON hd.MaHD = cthd.MaHD
                JOIN NhanVien nv ON hd.MaNV = nv.MaNV
                LEFT JOIN KhachHang kh ON hd.MaKH = kh.MaKH
                WHERE cthd.MaSP = @MaSP
                ORDER BY hd.NgayLap DESC";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { maSP });
        }
    }
}
