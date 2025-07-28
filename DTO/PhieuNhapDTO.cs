using System;
using System.Collections.Generic;

namespace QuanLyCuaHangDienThoai.DTO
{
    public class PhieuNhapDTO
    {
        public string MaPN { get; set; }
        public string MaNCC { get; set; }
        public string MaNV { get; set; }
        public DateTime NgayNhap { get; set; }
        public decimal TongTien { get; set; }
        public List<ChiTietPhieuNhapDTO> ChiTiet { get; set; }

        public PhieuNhapDTO()
        {
            ChiTiet = new List<ChiTietPhieuNhapDTO>();
        }
    }
}
