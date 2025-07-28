using QuanLyCuaHangDienThoai.DTO;
using System.Data;

namespace QuanLyCuaHangDienThoai.DAL
{
    public class NhanVienDAL
    {
        public DataTable LayDanhSachNhanVien()
        {
            string query = "SELECT nv.MaNV, nv.HoTen, nv.GioiTinh, nv.NgaySinh, nv.SoDienThoai, nv.DiaChi, nv.Email, nv.HinhAnh, nv.TrangThai, tk.TenDangNhap, tk.Quyen FROM NhanVien nv LEFT JOIN TaiKhoan tk ON nv.MaNV = tk.MaNV";
            return DataProvider.Instance.ExecuteQuery(query);
        }

        public bool ThemNhanVien(NhanVienDTO nv, TaiKhoanDTO tk)
        {
            string nvQuery = "INSERT INTO NhanVien (MaNV, HoTen, GioiTinh, NgaySinh, SoDienThoai, DiaChi, Email, HinhAnh, TrangThai) VALUES ( @MaNV , @HoTen , @GioiTinh , @NgaySinh , @SoDienThoai , @DiaChi , @Email , @HinhAnh , @TrangThai )";
            int nvResult = DataProvider.Instance.ExecuteNonQuery(nvQuery, new object[] { nv.MaNV, nv.HoTen, nv.GioiTinh, nv.NgaySinh, nv.SoDienThoai, nv.DiaChi, nv.Email, nv.HinhAnh, nv.TrangThai });

            if (nvResult > 0)
            {
                string tkQuery = "INSERT INTO TaiKhoan (TenDangNhap, MatKhau, MaNV, Quyen, TrangThai) VALUES ( @TenDangNhap , @MatKhau , @MaNV_TK , @Quyen , @TrangThai_TK )";
                int tkResult = DataProvider.Instance.ExecuteNonQuery(tkQuery, new object[] { tk.TenDangNhap, tk.MatKhau, tk.MaNV, tk.Quyen, tk.TrangThai });
                return tkResult > 0;
            }
            return false;
        }

        public bool SuaNhanVien(NhanVienDTO nv, TaiKhoanDTO tk)
        {
            string nvQuery = "UPDATE NhanVien SET HoTen = @HoTen , GioiTinh = @GioiTinh , NgaySinh = @NgaySinh , SoDienThoai = @SoDienThoai , DiaChi = @DiaChi , Email = @Email , HinhAnh = @HinhAnh , TrangThai = @TrangThai WHERE MaNV = @MaNV";
            int nvResult = DataProvider.Instance.ExecuteNonQuery(nvQuery, new object[] { nv.HoTen, nv.GioiTinh, nv.NgaySinh, nv.SoDienThoai, nv.DiaChi, nv.Email, nv.HinhAnh, nv.TrangThai, nv.MaNV });

            string tkQuery;
            int tkResult;
            if (!string.IsNullOrEmpty(tk.MatKhau))
            {
                tkQuery = "UPDATE TaiKhoan SET MatKhau = @MatKhau , Quyen = @Quyen , TrangThai = @TrangThai_TK WHERE MaNV = @MaNV_TK";
                tkResult = DataProvider.Instance.ExecuteNonQuery(tkQuery, new object[] { tk.MatKhau, tk.Quyen, tk.TrangThai, tk.MaNV });
            }
            else
            {
                tkQuery = "UPDATE TaiKhoan SET Quyen = @Quyen , TrangThai = @TrangThai_TK WHERE MaNV = @MaNV_TK";
                tkResult = DataProvider.Instance.ExecuteNonQuery(tkQuery, new object[] { tk.Quyen, tk.TrangThai, tk.MaNV });
            }

            return nvResult > 0 && tkResult > 0;
        }

        public bool VoHieuHoaNhanVien(string maNV)
        {
            string nvQuery = "UPDATE NhanVien SET TrangThai = 0 WHERE MaNV = @MaNV";
            int nvResult = DataProvider.Instance.ExecuteNonQuery(nvQuery, new object[] { maNV });

            string tkQuery = "UPDATE TaiKhoan SET TrangThai = 0 WHERE MaNV = @MaNV_TK";
            int tkResult = DataProvider.Instance.ExecuteNonQuery(tkQuery, new object[] { maNV });

            return nvResult > 0 && tkResult > 0;
        }

        public DataTable TimKiemNhanVien(string keyword)
        {
            string query = "SELECT nv.MaNV, nv.HoTen, nv.GioiTinh, nv.NgaySinh, nv.SoDienThoai, nv.DiaChi, nv.Email, nv.HinhAnh, nv.TrangThai, tk.TenDangNhap, tk.Quyen FROM NhanVien nv LEFT JOIN TaiKhoan tk ON nv.MaNV = tk.MaNV WHERE nv.MaNV LIKE @keyword OR nv.HoTen LIKE @keyword OR nv.SoDienThoai LIKE @keyword OR tk.TenDangNhap LIKE @keyword";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { "%" + keyword + "%" });
        }
    }
}
