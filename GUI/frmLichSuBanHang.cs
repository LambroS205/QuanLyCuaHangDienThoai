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
            SetupDataGridView();
        }

        private void SetupDataGridView()
        {
            if (dgvLichSu.Columns.Count > 0)
            {
                dgvLichSu.Columns["MaHD"].HeaderText = "Mã HĐ";
                dgvLichSu.Columns["NgayLap"].HeaderText = "Ngày Lập";
                dgvLichSu.Columns["TenNhanVien"].HeaderText = "Nhân Viên";
                dgvLichSu.Columns["TenKhachHang"].HeaderText = "Khách Hàng";
                dgvLichSu.Columns["SoLuong"].HeaderText = "Số Lượng Bán";
                dgvLichSu.Columns["TongTien"].HeaderText = "Tổng Tiền HĐ";

                dgvLichSu.Columns["NgayLap"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                dgvLichSu.Columns["TongTien"].DefaultCellStyle.Format = "N0";

                dgvLichSu.Columns["TenNhanVien"].FillWeight = 150;
                dgvLichSu.Columns["TenKhachHang"].FillWeight = 150;
            }
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