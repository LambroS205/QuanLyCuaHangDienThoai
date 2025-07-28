using QuanLyCuaHangDienThoai.BUS;
using System;
using System.Windows.Forms;

namespace QuanLyCuaHangDienThoai.GUI
{
    public partial class frmLichSuBanHang : Form
    {
        private string maSP;
        private string tenSP;
        private SanPhamBUS bus = new SanPhamBUS();

        public frmLichSuBanHang(string maSP, string tenSP)
        {
            InitializeComponent();
            this.maSP = maSP;
            this.tenSP = tenSP;
        }

        private void frmLichSuBanHang_Load(object sender, EventArgs e)
        {
            this.Text = $"Lịch Sử Bán Hàng - {tenSP} ({maSP})";
            dgvLichSu.DataSource = bus.GetLichSuBanHang(maSP);
        }

        private void dgvLichSu_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string maHD = dgvLichSu.Rows[e.RowIndex].Cells["MaHD"].Value.ToString();
                frmChiTietHoaDon f = new frmChiTietHoaDon(maHD);
                f.ShowDialog();
            }
        }
    }
}