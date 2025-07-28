using QuanLyCuaHangDienThoai.DAL;
using QuanLyCuaHangDienThoai.DTO;
using System.Data;

namespace QuanLyCuaHangDienThoai.BUS
{
    public class SanPhamBUS
    {
        private SanPhamDAL dal = new SanPhamDAL();

        public DataTable LayDanhSachSanPham()
        {
            return dal.LayDanhSachSanPham();
        }

        public DataTable LayDanhSachHang()
        {
            return dal.LayDanhSachHang();
        }

        public DataTable LayDanhSachLoaiSP()
        {
            return dal.LayDanhSachLoaiSP();
        }

        public bool ThemSanPham(SanPhamDTO sp)
        {
            if (string.IsNullOrEmpty(sp.MaSP) || string.IsNullOrEmpty(sp.TenSP))
                return false;
            return dal.ThemSanPham(sp);
        }

        public bool SuaSanPham(SanPhamDTO sp)
        {
            if (string.IsNullOrEmpty(sp.MaSP) || string.IsNullOrEmpty(sp.TenSP))
                return false;
            return dal.SuaSanPham(sp);
        }

        public bool XoaSanPham(string maSP)
        {
            if (string.IsNullOrEmpty(maSP))
                return false;
            return dal.XoaSanPham(maSP);
        }

        public DataTable TimKiemSanPham(string keyword)
        {
            return dal.TimKiemSanPham(keyword);
        }

        // --- PHƯƠNG THỨC MỚI ---
        public DataTable GetLichSuBanHang(string maSP)
        {
            return dal.GetLichSuBanHang(maSP);
        }
    }
}
