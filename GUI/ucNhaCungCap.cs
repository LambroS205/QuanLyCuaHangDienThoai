using QuanLyCuaHangDienThoai.BUS;
using QuanLyCuaHangDienThoai.DTO;
using System;
using System.Windows.Forms;

namespace QuanLyCuaHangDienThoai.GUI
{
    public partial class ucNhaCungCap : UserControl
    {
        private NhaCungCapBUS bus = new NhaCungCapBUS();

        public ucNhaCungCap()
        {
            InitializeComponent();
        }

        private void ucNhaCungCap_Load(object sender, EventArgs e)
        {
            LoadData();
            ClearFields();
        }

        private void LoadData()
        {
            dgvNhaCungCap.DataSource = bus.LayDanhSachNhaCungCap();
            SetupDataGridView();
        }

        private void SetupDataGridView()
        {
            if (dgvNhaCungCap.Columns.Count > 0)
            {
                dgvNhaCungCap.Columns["MaNCC"].HeaderText = "Mã NCC";
                dgvNhaCungCap.Columns["TenNCC"].HeaderText = "Tên Nhà Cung Cấp";
                dgvNhaCungCap.Columns["DiaChi"].HeaderText = "Địa Chỉ";
                dgvNhaCungCap.Columns["SoDienThoai"].HeaderText = "Số Điện Thoại";
                dgvNhaCungCap.Columns["Email"].HeaderText = "Email";
                dgvNhaCungCap.Columns["TrangThai"].HeaderText = "Trạng Thái";

                dgvNhaCungCap.Columns["TenNCC"].FillWeight = 150;
                dgvNhaCungCap.Columns["DiaChi"].FillWeight = 200;
                dgvNhaCungCap.Columns["Email"].FillWeight = 150;
            }
        }

        private void ClearFields()
        {
            txtMaNCC.Text = "";
            txtTenNCC.Text = "";
            txtSdt.Text = "";
            txtEmail.Text = "";
            txtDiaChi.Text = "";
            chkTrangThai.Checked = true;
            txtMaNCC.Enabled = true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                NhaCungCapDTO ncc = new NhaCungCapDTO
                {
                    MaNCC = txtMaNCC.Text.Trim(),
                    TenNCC = txtTenNCC.Text.Trim(),
                    SoDienThoai = txtSdt.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    DiaChi = txtDiaChi.Text.Trim(),
                    TrangThai = chkTrangThai.Checked
                };

                if (bus.ThemNhaCungCap(ncc))
                {
                    MessageBox.Show("Thêm nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Thêm nhà cung cấp thất bại. Vui lòng kiểm tra lại thông tin.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                NhaCungCapDTO ncc = new NhaCungCapDTO
                {
                    MaNCC = txtMaNCC.Text.Trim(),
                    TenNCC = txtTenNCC.Text.Trim(),
                    SoDienThoai = txtSdt.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    DiaChi = txtDiaChi.Text.Trim(),
                    TrangThai = chkTrangThai.Checked
                };

                if (bus.SuaNhaCungCap(ncc))
                {
                    MessageBox.Show("Cập nhật nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Cập nhật nhà cung cấp thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvNhaCungCap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvNhaCungCap.Rows[e.RowIndex];
                txtMaNCC.Text = row.Cells["MaNCC"].Value.ToString();
                txtTenNCC.Text = row.Cells["TenNCC"].Value.ToString();
                txtSdt.Text = row.Cells["SoDienThoai"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value.ToString();
                chkTrangThai.Checked = Convert.ToBoolean(row.Cells["TrangThai"].Value);
                txtMaNCC.Enabled = false;
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            dgvNhaCungCap.DataSource = bus.TimKiemNhaCungCap(keyword);
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearFields();
            LoadData();
        }
    }
}
