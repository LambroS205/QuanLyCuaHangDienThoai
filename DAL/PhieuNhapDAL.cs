using QuanLyCuaHangDienThoai.DTO;
using System;
using System.Data;

namespace QuanLyCuaHangDienThoai.DAL
{
    public class PhieuNhapDAL
    {
        public string GetNewMaPN()
        {
            string query = "SELECT TOP 1 MaPN FROM PhieuNhap ORDER BY MaPN DESC";
            object result = DataProvider.Instance.ExecuteScalar(query);
            if (result == null)
            {
                return "PN001";
            }
            else
            {
                string lastMaPN = result.ToString();
                int number = int.Parse(lastMaPN.Substring(2));
                number++;
                return "PN" + number.ToString("D3");
            }
        }

        public bool TaoPhieuNhap(PhieuNhapDTO pn)
        {
            string pnQuery = "INSERT INTO PhieuNhap (MaPN, MaNCC, MaNV, NgayNhap, TongTien) VALUES ( @MaPN , @MaNCC , @MaNV , @NgayNhap , @TongTien )";
            int result = DataProvider.Instance.ExecuteNonQuery(pnQuery, new object[] { pn.MaPN, pn.MaNCC, pn.MaNV, pn.NgayNhap, pn.TongTien });

            if (result > 0)
            {
                foreach (var ct in pn.ChiTiet)
                {
                    string ctQuery = "INSERT INTO ChiTietPhieuNhap (MaPN, MaSP, SoLuong, DonGiaNhap) VALUES ( @MaPN_CT , @MaSP , @SoLuong , @DonGiaNhap )";
                    DataProvider.Instance.ExecuteNonQuery(ctQuery, new object[] { ct.MaPN, ct.MaSP, ct.SoLuong, ct.DonGiaNhap });

                    string updateSpQuery = "UPDATE SanPham SET SoLuongTon = SoLuongTon + @SoLuongNhap WHERE MaSP = @MaSP_Update";
                    DataProvider.Instance.ExecuteNonQuery(updateSpQuery, new object[] { ct.SoLuong, ct.MaSP });
                }
                return true;
            }
            return false;
        }

        // --- CÁC PHƯƠNG THỨC MỚI ---
        public DataTable GetLichSuPhieuNhap()
        {
            string query = @"
                SELECT pn.MaPN, pn.NgayNhap, ncc.TenNCC, nv.HoTen AS TenNhanVien, pn.TongTien
                FROM PhieuNhap pn
                JOIN NhaCungCap ncc ON pn.MaNCC = ncc.MaNCC
                JOIN NhanVien nv ON pn.MaNV = nv.MaNV
                ORDER BY pn.NgayNhap DESC";
            return DataProvider.Instance.ExecuteQuery(query);
        }

        public DataTable GetThongTinChiTietPhieuNhap(string maPN)
        {
            string query = @"
                SELECT 
                    pn.MaPN, pn.NgayNhap, pn.TongTien,
                    nv.HoTen AS TenNhanVien,
                    ncc.MaNCC, ncc.TenNCC, ncc.DiaChi, ncc.SoDienThoai
                FROM PhieuNhap pn
                JOIN NhanVien nv ON pn.MaNV = nv.MaNV
                JOIN NhaCungCap ncc ON pn.MaNCC = ncc.MaNCC
                WHERE pn.MaPN = @MaPN";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { maPN });
        }

        public DataTable GetDanhSachSanPhamCuaPhieuNhap(string maPN)
        {
            string query = @"
                SELECT 
                    sp.MaSP, sp.TenSP, ctpn.SoLuong, ctpn.DonGiaNhap, (ctpn.SoLuong * ctpn.DonGiaNhap) AS ThanhTien
                FROM ChiTietPhieuNhap ctpn
                JOIN SanPham sp ON ctpn.MaSP = sp.MaSP
                WHERE ctpn.MaPN = @MaPN";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { maPN });
        }
    }
}
