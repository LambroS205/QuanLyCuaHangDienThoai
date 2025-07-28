using QuanLyCuaHangDienThoai.DTO;
using System;
using System.Data;

namespace QuanLyCuaHangDienThoai.DAL
{
    public class HoaDonDAL
    {
        public string GetNewMaHD()
        {
            string query = "SELECT TOP 1 MaHD FROM HoaDon ORDER BY MaHD DESC";
            object result = DataProvider.Instance.ExecuteScalar(query);
            if (result == null)
            {
                return "HD001";
            }
            else
            {
                string lastMaHD = result.ToString();
                int number = int.Parse(lastMaHD.Substring(2));
                number++;
                return "HD" + number.ToString("D3");
            }
        }

        public bool TaoHoaDon(HoaDonDTO hd)
        {
            string hdQuery = "INSERT INTO HoaDon (MaHD, MaNV, MaKH, NgayLap, TongTien) VALUES ( @MaHD , @MaNV , @MaKH , @NgayLap , @TongTien )";
            int result = DataProvider.Instance.ExecuteNonQuery(hdQuery, new object[] { hd.MaHD, hd.MaNV, hd.MaKH, hd.NgayLap, hd.TongTien });

            if (result > 0)
            {
                foreach (var ct in hd.ChiTiet)
                {
                    string ctQuery = "INSERT INTO ChiTietHoaDon (MaHD, MaSP, SoLuong, DonGia) VALUES ( @MaHD_CT , @MaSP , @SoLuong , @DonGia )";
                    DataProvider.Instance.ExecuteNonQuery(ctQuery, new object[] { ct.MaHD, ct.MaSP, ct.SoLuong, ct.DonGia });

                    string updateSpQuery = "UPDATE SanPham SET SoLuongTon = SoLuongTon - @SoLuongBan WHERE MaSP = @MaSP_Update";
                    DataProvider.Instance.ExecuteNonQuery(updateSpQuery, new object[] { ct.SoLuong, ct.MaSP });
                }
                return true;
            }
            return false;
        }

        // --- PHƯƠNG THỨC MỚI ---
        public DataTable GetThongTinChiTietHoaDon(string maHD)
        {
            string query = @"
                SELECT 
                    hd.MaHD, hd.NgayLap, hd.TongTien,
                    nv.HoTen AS TenNhanVien,
                    kh.MaKH, kh.HoTen AS TenKhachHang, kh.DiaChi, kh.SoDienThoai
                FROM HoaDon hd
                JOIN NhanVien nv ON hd.MaNV = nv.MaNV
                LEFT JOIN KhachHang kh ON hd.MaKH = kh.MaKH
                WHERE hd.MaHD = @MaHD";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { maHD });
        }

        // --- PHƯƠNG THỨC MỚI ---
        public DataTable GetDanhSachSanPhamCuaHoaDon(string maHD)
        {
            string query = @"
                SELECT 
                    sp.MaSP, sp.TenSP, cthd.SoLuong, cthd.DonGia, (cthd.SoLuong * cthd.DonGia) AS ThanhTien
                FROM ChiTietHoaDon cthd
                JOIN SanPham sp ON cthd.MaSP = sp.MaSP
                WHERE cthd.MaHD = @MaHD";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { maHD });
        }
    }
}
