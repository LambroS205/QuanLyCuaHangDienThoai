﻿namespace QuanLyCuaHangDienThoai.DTO
{
    public class ChiTietHoaDonDTO
    {
        public string MaHD { get; set; }
        public string MaSP { get; set; }
        public string TenSP { get; set; } // Thêm để hiển thị
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
    }
}
