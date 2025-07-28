using QuanLyCuaHangDienThoai.DAL;
using QuanLyCuaHangDienThoai.DTO;
using System.Data;

namespace QuanLyCuaHangDienThoai.BUS
{
    public class KhachHangBUS
    {
        private KhachHangDAL dal = new KhachHangDAL();

        public DataTable LayDanhSachKhachHang()
        {
            return dal.LayDanhSachKhachHang();
        }

        public bool ThemKhachHang(KhachHangDTO kh)
        {
            if (string.IsNullOrEmpty(kh.MaKH) || string.IsNullOrEmpty(kh.HoTen) || string.IsNullOrEmpty(kh.SoDienThoai))
                return false;
            return dal.ThemKhachHang(kh);
        }

        public bool SuaKhachHang(KhachHangDTO kh)
        {
            if (string.IsNullOrEmpty(kh.MaKH) || string.IsNullOrEmpty(kh.HoTen) || string.IsNullOrEmpty(kh.SoDienThoai))
                return false;
            return dal.SuaKhachHang(kh);
        }

        public DataTable TimKiemKhachHang(string keyword)
        {
            return dal.TimKiemKhachHang(keyword);
        }
    }
}
