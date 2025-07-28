using QuanLyCuaHangDienThoai.DTO;
using System.Data;

namespace QuanLyCuaHangDienThoai.DAL
{
    public class NhaCungCapDAL
    {
        public DataTable LayDanhSachNhaCungCap()
        {
            string query = "SELECT MaNCC, TenNCC, DiaChi, SoDienThoai, Email, TrangThai FROM NhaCungCap";
            return DataProvider.Instance.ExecuteQuery(query);
        }

        public bool ThemNhaCungCap(NhaCungCapDTO ncc)
        {
            string query = "INSERT INTO NhaCungCap (MaNCC, TenNCC, DiaChi, SoDienThoai, Email, TrangThai) VALUES ( @MaNCC , @TenNCC , @DiaChi , @SoDienThoai , @Email , @TrangThai )";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { ncc.MaNCC, ncc.TenNCC, ncc.DiaChi, ncc.SoDienThoai, ncc.Email, ncc.TrangThai });
            return result > 0;
        }

        public bool SuaNhaCungCap(NhaCungCapDTO ncc)
        {
            string query = "UPDATE NhaCungCap SET TenNCC = @TenNCC , DiaChi = @DiaChi , SoDienThoai = @SoDienThoai , Email = @Email , TrangThai = @TrangThai WHERE MaNCC = @MaNCC";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { ncc.TenNCC, ncc.DiaChi, ncc.SoDienThoai, ncc.Email, ncc.TrangThai, ncc.MaNCC });
            return result > 0;
        }

        public DataTable TimKiemNhaCungCap(string keyword)
        {
            string query = "SELECT MaNCC, TenNCC, DiaChi, SoDienThoai, Email, TrangThai FROM NhaCungCap WHERE MaNCC LIKE @keyword OR TenNCC LIKE @keyword OR SoDienThoai LIKE @keyword";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { "%" + keyword + "%" });
        }
    }
}
