using QuanLyCuaHangDienThoai.DAL;
using QuanLyCuaHangDienThoai.DTO;
using System.Data;

namespace QuanLyCuaHangDienThoai.BUS
{
    public class HoaDonBUS
    {
        private HoaDonDAL dal = new HoaDonDAL();

        public string GetNewMaHD()
        {
            return dal.GetNewMaHD();
        }

        public bool TaoHoaDon(HoaDonDTO hd)
        {
            if (hd.ChiTiet.Count == 0 || string.IsNullOrEmpty(hd.MaNV))
            {
                return false;
            }
            return dal.TaoHoaDon(hd);
        }

        // --- PHƯƠNG THỨC MỚI ---
        public DataTable GetThongTinChiTietHoaDon(string maHD)
        {
            return dal.GetThongTinChiTietHoaDon(maHD);
        }

        // --- PHƯƠNG THỨC MỚI ---
        public DataTable GetDanhSachSanPhamCuaHoaDon(string maHD)
        {
            return dal.GetDanhSachSanPhamCuaHoaDon(maHD);
        }
    }
}
