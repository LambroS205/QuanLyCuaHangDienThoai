namespace QuanLyCuaHangDienThoai.DTO
{
    public class TaiKhoanDTO
    {
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string MaNV { get; set; }
        public string HoTen { get; set; } // Đảm bảo có thuộc tính này
        public string Quyen { get; set; }
        public bool TrangThai { get; set; }

        public TaiKhoanDTO() { }

        public TaiKhoanDTO(string tenDangNhap, string matKhau, string maNV, string hoTen, string quyen, bool trangThai)
        {
            TenDangNhap = tenDangNhap;
            MatKhau = matKhau;
            MaNV = maNV;
            HoTen = hoTen;
            Quyen = quyen;
            TrangThai = trangThai;
        }
    }
}