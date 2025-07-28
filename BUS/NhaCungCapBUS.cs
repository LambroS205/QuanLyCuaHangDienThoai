using QuanLyCuaHangDienThoai.DAL;
using QuanLyCuaHangDienThoai.DTO;
using System.Data;

namespace QuanLyCuaHangDienThoai.BUS
{
    public class NhaCungCapBUS
    {
        private NhaCungCapDAL dal = new NhaCungCapDAL();

        public DataTable LayDanhSachNhaCungCap()
        {
            return dal.LayDanhSachNhaCungCap();
        }

        public bool ThemNhaCungCap(NhaCungCapDTO ncc)
        {
            if (string.IsNullOrEmpty(ncc.MaNCC) || string.IsNullOrEmpty(ncc.TenNCC))
                return false;
            return dal.ThemNhaCungCap(ncc);
        }

        public bool SuaNhaCungCap(NhaCungCapDTO ncc)
        {
            if (string.IsNullOrEmpty(ncc.MaNCC) || string.IsNullOrEmpty(ncc.TenNCC))
                return false;
            return dal.SuaNhaCungCap(ncc);
        }

        public DataTable TimKiemNhaCungCap(string keyword)
        {
            return dal.TimKiemNhaCungCap(keyword);
        }
    }
}