using QuanLyCuaHangDienThoai.DAL;
using QuanLyCuaHangDienThoai.DTO;
using System.Data;

namespace QuanLyCuaHangDienThoai.BUS
{
    public class PhieuNhapBUS
    {
        private PhieuNhapDAL dal = new PhieuNhapDAL();

        public string GetNewMaPN()
        {
            return dal.GetNewMaPN();
        }

        public bool TaoPhieuNhap(PhieuNhapDTO pn)
        {
            if (pn.ChiTiet.Count == 0 || string.IsNullOrEmpty(pn.MaNV) || string.IsNullOrEmpty(pn.MaNCC))
            {
                return false;
            }
            return dal.TaoPhieuNhap(pn);
        }

        // --- CÁC PHƯƠNG THỨC MỚI ---
        public DataTable GetLichSuPhieuNhap()
        {
            return dal.GetLichSuPhieuNhap();
        }

        public DataTable GetThongTinChiTietPhieuNhap(string maPN)
        {
            return dal.GetThongTinChiTietPhieuNhap(maPN);
        }

        public DataTable GetDanhSachSanPhamCuaPhieuNhap(string maPN)
        {
            return dal.GetDanhSachSanPhamCuaPhieuNhap(maPN);
        }
    }
}
