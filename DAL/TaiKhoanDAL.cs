using QuanLyCuaHangDienThoai.DTO;
using System.Data;

namespace QuanLyCuaHangDienThoai.DAL
{
    public class TaiKhoanDAL
    {
        /// <summary>
        /// Lấy thông tin tài khoản và họ tên nhân viên tương ứng.
        /// </summary>
        /// <param name="tenDangNhap">Tên đăng nhập.</param>
        /// <param name="matKhau">Mật khẩu.</param>
        /// <returns>DataTable chứa thông tin tài khoản nếu hợp lệ.</returns>
        public DataTable KiemTraDangNhap(string tenDangNhap, string matKhau)
        {
            string query = "SELECT tk.*, nv.HoTen FROM TaiKhoan tk JOIN NhanVien nv ON tk.MaNV = nv.MaNV WHERE tk.TenDangNhap = @TenDangNhap AND tk.MatKhau = @MatKhau AND tk.TrangThai = 1";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { tenDangNhap, matKhau });
        }
    }
}