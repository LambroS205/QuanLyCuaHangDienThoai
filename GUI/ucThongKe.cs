using QuanLyCuaHangDienThoai.BUS;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace QuanLyCuaHangDienThoai.GUI
{
    public partial class ucThongKe : UserControl
    {
        private ThongKeBUS bus = new ThongKeBUS();

        public ucThongKe()
        {
            InitializeComponent();
        }

        private void ucThongKe_Load(object sender, EventArgs e)
        {
            dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpTo.Value = DateTime.Now;
            btnThongKe.PerformClick();
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            DateTime fromDate = dtpFrom.Value.Date;
            DateTime toDate = dtpTo.Value.Date;

            if (fromDate > toDate)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LoadDoanhThu(fromDate, toDate);
            LoadTopSanPham(fromDate, toDate);
            LoadLichSuHoaDon(fromDate, toDate);
            LoadLichSuNhapHang(fromDate, toDate); // Gọi phương thức mới
        }

        private void LoadDoanhThu(DateTime fromDate, DateTime toDate)
        {
            DataTable dt = bus.GetDoanhThu(fromDate, toDate);
            chartDoanhThu.Series["DoanhThu"].Points.Clear();
            decimal tongDoanhThu = 0;

            foreach (DataRow row in dt.Rows)
            {
                DateTime ngay = Convert.ToDateTime(row["Ngay"]);
                decimal doanhThu = Convert.ToDecimal(row["DoanhThu"]);
                chartDoanhThu.Series["DoanhThu"].Points.AddXY(ngay, doanhThu);
                tongDoanhThu += doanhThu;
            }

            lblTongDoanhThu.Text = tongDoanhThu.ToString("N0") + " VNĐ";
        }

        private void LoadTopSanPham(DateTime fromDate, DateTime toDate)
        {
            DataTable dt = bus.GetTopSanPhamBanChay(fromDate, toDate);
            dgvTopSanPham.DataSource = dt;
            SetupTopSanPhamGridView();
        }

        private void SetupTopSanPhamGridView()
        {
            if (dgvTopSanPham.Columns.Count > 0)
            {
                dgvTopSanPham.Columns["MaSP"].HeaderText = "Mã SP";
                dgvTopSanPham.Columns["TenSP"].HeaderText = "Tên Sản Phẩm";
                dgvTopSanPham.Columns["SoLuongBan"].HeaderText = "Số Lượng Bán";
                dgvTopSanPham.Columns["TenSP"].FillWeight = 250;
            }
        }

        private void LoadLichSuHoaDon(DateTime fromDate, DateTime toDate)
        {
            DataTable dt = bus.GetLichSuHoaDon(fromDate, toDate);
            dgvLichSuHoaDon.DataSource = dt;
            SetupLichSuHoaDonGridView();
        }

        private void SetupLichSuHoaDonGridView()
        {
            if (dgvLichSuHoaDon.Columns.Count > 0)
            {
                dgvLichSuHoaDon.Columns["MaHD"].HeaderText = "Mã HĐ";
                dgvLichSuHoaDon.Columns["NgayLap"].HeaderText = "Ngày Lập";
                dgvLichSuHoaDon.Columns["TenNhanVien"].HeaderText = "Nhân Viên";
                dgvLichSuHoaDon.Columns["TenKhachHang"].HeaderText = "Khách Hàng";
                dgvLichSuHoaDon.Columns["TongTien"].HeaderText = "Tổng Tiền";

                dgvLichSuHoaDon.Columns["NgayLap"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                dgvLichSuHoaDon.Columns["TongTien"].DefaultCellStyle.Format = "N0";

                dgvLichSuHoaDon.Columns["TenNhanVien"].FillWeight = 150;
                dgvLichSuHoaDon.Columns["TenKhachHang"].FillWeight = 150;
            }
        }

        private void dgvLichSuHoaDon_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string maHD = dgvLichSuHoaDon.Rows[e.RowIndex].Cells["MaHD"].Value.ToString();
                frmChiTietHoaDon f = new frmChiTietHoaDon(maHD);
                f.ShowDialog();
            }
        }

        // --- CÁC PHƯƠNG THỨC MỚI ---
        private void LoadLichSuNhapHang(DateTime fromDate, DateTime toDate)
        {
            DataTable dt = bus.GetLichSuPhieuNhap(fromDate, toDate);
            dgvLichSuNhapHang.DataSource = dt;
            SetupLichSuNhapHangGridView();
        }

        private void SetupLichSuNhapHangGridView()
        {
            if (dgvLichSuNhapHang.Columns.Count > 0)
            {
                dgvLichSuNhapHang.Columns["MaPN"].HeaderText = "Mã Phiếu Nhập";
                dgvLichSuNhapHang.Columns["NgayNhap"].HeaderText = "Ngày Nhập";
                dgvLichSuNhapHang.Columns["TenNCC"].HeaderText = "Nhà Cung Cấp";
                dgvLichSuNhapHang.Columns["TenNhanVien"].HeaderText = "Nhân Viên";
                dgvLichSuNhapHang.Columns["TongTien"].HeaderText = "Tổng Tiền";

                dgvLichSuNhapHang.Columns["NgayNhap"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                dgvLichSuNhapHang.Columns["TongTien"].DefaultCellStyle.Format = "N0";

                dgvLichSuNhapHang.Columns["TenNCC"].FillWeight = 150;
                dgvLichSuNhapHang.Columns["TenNhanVien"].FillWeight = 150;
            }
        }

        private void dgvLichSuNhapHang_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string maPN = dgvLichSuNhapHang.Rows[e.RowIndex].Cells["MaPN"].Value.ToString();
                frmChiTietPhieuNhap f = new frmChiTietPhieuNhap(maPN);
                f.ShowDialog();
            }
        }
    }
}
