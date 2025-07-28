using QuanLyCuaHangDienThoai.BUS;
using QuanLyCuaHangDienThoai.DTO;
using System;
using System.Windows.Forms;

namespace QuanLyCuaHangDienThoai.GUI
{
    public partial class ucKhachHang : UserControl
    {
        private KhachHangBUS bus = new KhachHangBUS();

        public ucKhachHang()
        {
            InitializeComponent();
        }

        private void ucKhachHang_Load(object sender, EventArgs e)
        {
            LoadData();
            ClearFields();
        }

        private void LoadData()
        {
            dgvKhachHang.DataSource = bus.LayDanhSachKhachHang();
            SetupDataGridView();
        }

        private void SetupDataGridView()
        {
            if (dgvKhachHang.Columns.Count > 0)
            {
                dgvKhachHang.Columns["MaKH"].HeaderText = "Mã KH";
                dgvKhachHang.Columns["HoTen"].HeaderText = "Họ Tên";
                dgvKhachHang.Columns["SoDienThoai"].HeaderText = "Số Điện Thoại";
                dgvKhachHang.Columns["DiaChi"].HeaderText = "Địa Chỉ";
                dgvKhachHang.Columns["DiemTichLuy"].HeaderText = "Điểm Tích Lũy";

                dgvKhachHang.Columns["DiemTichLuy"].DefaultCellStyle.Format = "N0";

                dgvKhachHang.Columns["HoTen"].FillWeight = 150;
                dgvKhachHang.Columns["DiaChi"].FillWeight = 200;
            }
        }

        private void ClearFields()
        {
            txtMaKH.Text = "";
            txtHoTen.Text = "";
            txtSdt.Text = "";
            txtDiaChi.Text = "";
            numDiem.Value = 0;
            txtMaKH.Enabled = true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                KhachHangDTO kh = new KhachHangDTO
                {
                    MaKH = txtMaKH.Text.Trim(),
                    HoTen = txtHoTen.Text.Trim(),
                    SoDienThoai = txtSdt.Text.Trim(),
                    DiaChi = txtDiaChi.Text.Trim(),
                    DiemTichLuy = (int)numDiem.Value
                };

                if (bus.ThemKhachHang(kh))
                {
                    MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Thêm khách hàng thất bại. Vui lòng kiểm tra lại thông tin.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                KhachHangDTO kh = new KhachHangDTO
                {
                    MaKH = txtMaKH.Text.Trim(),
                    HoTen = txtHoTen.Text.Trim(),
                    SoDienThoai = txtSdt.Text.Trim(),
                    DiaChi = txtDiaChi.Text.Trim(),
                    DiemTichLuy = (int)numDiem.Value
                };

                if (bus.SuaKhachHang(kh))
                {
                    MessageBox.Show("Cập nhật khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Cập nhật khách hàng thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvKhachHang.Rows[e.RowIndex];
                txtMaKH.Text = row.Cells["MaKH"].Value.ToString();
                txtHoTen.Text = row.Cells["HoTen"].Value.ToString();
                txtSdt.Text = row.Cells["SoDienThoai"].Value.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value.ToString();
                numDiem.Value = Convert.ToInt32(row.Cells["DiemTichLuy"].Value);
                txtMaKH.Enabled = false;
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            dgvKhachHang.DataSource = bus.TimKiemKhachHang(keyword);
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearFields();
            LoadData();
        }
    }
}
