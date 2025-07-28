using QuanLyCuaHangDienThoai.DTO;
using System;

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
            // Sử dụng transaction ở đây là rất quan trọng
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
    }
}