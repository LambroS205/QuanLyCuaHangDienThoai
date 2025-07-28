using System;
using System.Collections.Generic;

namespace QuanLyCuaHangDienThoai.DTO
{
    public class HoaDonDTO
    {
        public string MaHD { get; set; }
        public string MaNV { get; set; }
        public string MaKH { get; set; }
        public DateTime NgayLap { get; set; }
        public decimal TongTien { get; set; }
        public List<ChiTietHoaDonDTO> ChiTiet { get; set; }

        public HoaDonDTO()
        {
            ChiTiet = new List<ChiTietHoaDonDTO>();
        }
    }
}
