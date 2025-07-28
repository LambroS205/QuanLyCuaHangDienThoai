using QuanLyCuaHangDienThoai.DAL;
using QuanLyCuaHangDienThoai.DTO;
using System.Data;

namespace QuanLyCuaHangDienThoai.BUS
{
    public class TaiKhoanBUS
    {
        private TaiKhoanDAL taiKhoanDAL = new TaiKhoanDAL();

        /// <summary>
        /// Xử lý logic kiểm tra đăng nhập.
        /// </summary>
        /// <param name="tenDangNhap">Tên đăng nhập.</param>
        /// <param name="matKhau">Mật khẩu.</param>
        /// <returns>Đối tượng TaiKhoanDTO nếu thành công, ngược lại trả về null.</returns>
        public TaiKhoanDTO KiemTraDangNhap(string tenDangNhap, string matKhau)
        {
            if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau))
            {
                return null;
            }

            DataTable dt = taiKhoanDAL.KiemTraDangNhap(tenDangNhap, matKhau);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                TaiKhoanDTO account = new TaiKhoanDTO
                {
                    TenDangNhap = row["TenDangNhap"].ToString(),
                    MatKhau = row["MatKhau"].ToString(),
                    MaNV = row["MaNV"].ToString(),
                    HoTen = row["HoTen"].ToString(),
                    Quyen = row["Quyen"].ToString(),
                    TrangThai = (bool)row["TrangThai"]
                };
                return account;
            }

            return null;
        }
    }
}
