using QuanLyCuaHangDienThoai.DAL;
using QuanLyCuaHangDienThoai.DTO;
using System.Data;

namespace QuanLyCuaHangDienThoai.BUS
{
    public class NhanVienBUS
    {
        private NhanVienDAL dal = new NhanVienDAL();

        public DataTable LayDanhSachNhanVien()
        {
            return dal.LayDanhSachNhanVien();
        }

        public bool ThemNhanVien(NhanVienDTO nv, TaiKhoanDTO tk)
        {
            if (string.IsNullOrEmpty(nv.MaNV) || string.IsNullOrEmpty(nv.HoTen) || string.IsNullOrEmpty(tk.TenDangNhap) || string.IsNullOrEmpty(tk.MatKhau))
                return false;

            return dal.ThemNhanVien(nv, tk);
        }

        public bool SuaNhanVien(NhanVienDTO nv, TaiKhoanDTO tk)
        {
            if (string.IsNullOrEmpty(nv.MaNV) || string.IsNullOrEmpty(nv.HoTen))
                return false;

            return dal.SuaNhanVien(nv, tk);
        }

        public bool VoHieuHoaNhanVien(string maNV)
        {
            if (string.IsNullOrEmpty(maNV))
                return false;
            return dal.VoHieuHoaNhanVien(maNV);
        }

        public DataTable TimKiemNhanVien(string keyword)
        {
            return dal.TimKiemNhanVien(keyword);
        }
    }
}
