using QuanLyCuaHangDienThoai.DAL;
using System;
using System.Data;

namespace QuanLyCuaHangDienThoai.BUS
{
    public class ThongKeBUS
    {
        private ThongKeDAL dal = new ThongKeDAL();

        public DataTable GetDoanhThu(DateTime fromDate, DateTime toDate)
        {
            return dal.GetDoanhThu(fromDate, toDate);
        }

        public DataTable GetTopSanPhamBanChay(DateTime fromDate, DateTime toDate)
        {
            return dal.GetTopSanPhamBanChay(fromDate, toDate);
        }

        public DataTable GetLichSuHoaDon(DateTime fromDate, DateTime toDate)
        {
            return dal.GetLichSuHoaDon(fromDate, toDate);
        }
    }
}