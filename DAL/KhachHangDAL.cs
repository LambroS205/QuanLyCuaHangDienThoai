using QuanLyCuaHangDienThoai.DTO;
using System.Data;

namespace QuanLyCuaHangDienThoai.DAL
{
    public class KhachHangDAL
    {
        public DataTable LayDanhSachKhachHang()
        {
            string query = "SELECT MaKH, HoTen, SoDienThoai, DiaChi, DiemTichLuy FROM KhachHang";
            return DataProvider.Instance.ExecuteQuery(query);
        }

        public bool ThemKhachHang(KhachHangDTO kh)
        {
            string query = "INSERT INTO KhachHang (MaKH, HoTen, SoDienThoai, DiaChi, DiemTichLuy) VALUES ( @MaKH , @HoTen , @SoDienThoai , @DiaChi , @DiemTichLuy )";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { kh.MaKH, kh.HoTen, kh.SoDienThoai, kh.DiaChi, kh.DiemTichLuy });
            return result > 0;
        }

        public bool SuaKhachHang(KhachHangDTO kh)
        {
            string query = "UPDATE KhachHang SET HoTen = @HoTen , SoDienThoai = @SoDienThoai , DiaChi = @DiaChi , DiemTichLuy = @DiemTichLuy WHERE MaKH = @MaKH";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { kh.HoTen, kh.SoDienThoai, kh.DiaChi, kh.DiemTichLuy, kh.MaKH });
            return result > 0;
        }

        public DataTable TimKiemKhachHang(string keyword)
        {
            string query = "SELECT MaKH, HoTen, SoDienThoai, DiaChi, DiemTichLuy FROM KhachHang WHERE MaKH LIKE @keyword OR HoTen LIKE @keyword OR SoDienThoai LIKE @keyword";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { "%" + keyword + "%" });
        }
    }
}
