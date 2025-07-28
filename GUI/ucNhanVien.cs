using QuanLyCuaHangDienThoai.BUS;
using QuanLyCuaHangDienThoai.DTO;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace QuanLyCuaHangDienThoai.GUI
{
    public partial class ucNhanVien : UserControl
    {
        private NhanVienBUS bus = new NhanVienBUS();
        private string currentImagePath = "";

        public ucNhanVien()
        {
            InitializeComponent();
        }

        private void ucNhanVien_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadComboBoxes();
            ClearFields();
        }

        private void LoadData()
        {
            dgvNhanVien.DataSource = bus.LayDanhSachNhanVien();
            SetupDataGridView();
        }

        private void SetupDataGridView()
        {
            if (dgvNhanVien.Columns.Count > 0)
            {
                dgvNhanVien.Columns["MaNV"].HeaderText = "Mã NV";
                dgvNhanVien.Columns["HoTen"].HeaderText = "Họ Tên";
                dgvNhanVien.Columns["GioiTinh"].HeaderText = "Giới Tính";
                dgvNhanVien.Columns["NgaySinh"].HeaderText = "Ngày Sinh";
                dgvNhanVien.Columns["SoDienThoai"].HeaderText = "Số Điện Thoại";
                dgvNhanVien.Columns["DiaChi"].HeaderText = "Địa Chỉ";
                dgvNhanVien.Columns["TenDangNhap"].HeaderText = "Tên Đăng Nhập";
                dgvNhanVien.Columns["Quyen"].HeaderText = "Quyền";
                dgvNhanVien.Columns["TrangThai"].HeaderText = "Trạng Thái";

                dgvNhanVien.Columns["Email"].Visible = false;
                dgvNhanVien.Columns["HinhAnh"].Visible = false;

                dgvNhanVien.Columns["NgaySinh"].DefaultCellStyle.Format = "dd/MM/yyyy";

                dgvNhanVien.Columns["HoTen"].FillWeight = 150;
                dgvNhanVien.Columns["DiaChi"].FillWeight = 200;
            }
        }

        private void LoadComboBoxes()
        {
            cboGioiTinh.Items.Clear();
            cboGioiTinh.Items.AddRange(new string[] { "Nam", "Nữ", "Khác" });

            cboQuyen.Items.Clear();
            cboQuyen.Items.AddRange(new string[] { "Admin", "NhanVien" });
        }

        private void ClearFields()
        {
            txtMaNV.Text = "";
            txtHoTen.Text = "";
            cboGioiTinh.SelectedIndex = -1;
            dtpNgaySinh.Value = DateTime.Now;
            txtSdt.Text = "";
            txtDiaChi.Text = "";
            txtEmail.Text = "";
            chkTrangThai.Checked = true;
            txtTenDangNhap.Text = "";
            txtMatKhau.Text = "";
            cboQuyen.SelectedIndex = -1;
            picHinhAnh.Image = null;
            currentImagePath = "";
            txtMaNV.Enabled = true;
            txtTenDangNhap.Enabled = true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                NhanVienDTO nv = new NhanVienDTO
                {
                    MaNV = txtMaNV.Text.Trim(),
                    HoTen = txtHoTen.Text.Trim(),
                    GioiTinh = cboGioiTinh.SelectedItem.ToString(),
                    NgaySinh = dtpNgaySinh.Value,
                    SoDienThoai = txtSdt.Text.Trim(),
                    DiaChi = txtDiaChi.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    TrangThai = chkTrangThai.Checked,
                    HinhAnh = SaveImage(currentImagePath)
                };

                TaiKhoanDTO tk = new TaiKhoanDTO(txtTenDangNhap.Text.Trim(), txtMatKhau.Text.Trim(), txtMaNV.Text.Trim(), txtHoTen.Text.Trim(), cboQuyen.SelectedItem.ToString(), chkTrangThai.Checked);

                if (bus.ThemNhanVien(nv, tk))
                {
                    MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Thêm nhân viên thất bại. Vui lòng kiểm tra lại thông tin (Mã NV, Tên đăng nhập, Mật khẩu không được trống).", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                NhanVienDTO nv = new NhanVienDTO
                {
                    MaNV = txtMaNV.Text.Trim(),
                    HoTen = txtHoTen.Text.Trim(),
                    GioiTinh = cboGioiTinh.SelectedItem.ToString(),
                    NgaySinh = dtpNgaySinh.Value,
                    SoDienThoai = txtSdt.Text.Trim(),
                    DiaChi = txtDiaChi.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    TrangThai = chkTrangThai.Checked,
                    HinhAnh = SaveImage(currentImagePath)
                };

                TaiKhoanDTO tk = new TaiKhoanDTO(txtTenDangNhap.Text.Trim(), txtMatKhau.Text.Trim(), txtMaNV.Text.Trim(), txtHoTen.Text.Trim(), cboQuyen.SelectedItem.ToString(), chkTrangThai.Checked);

                if (bus.SuaNhanVien(nv, tk))
                {
                    MessageBox.Show("Cập nhật nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Cập nhật nhân viên thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maNV = txtMaNV.Text.Trim();
            if (string.IsNullOrEmpty(maNV))
            {
                MessageBox.Show("Vui lòng chọn nhân viên để vô hiệu hóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn vô hiệu hóa nhân viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if (bus.VoHieuHoaNhanVien(maNV))
                {
                    MessageBox.Show("Vô hiệu hóa nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Thao tác thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];
                txtMaNV.Text = row.Cells["MaNV"].Value.ToString();
                txtHoTen.Text = row.Cells["HoTen"].Value.ToString();
                cboGioiTinh.SelectedItem = row.Cells["GioiTinh"].Value.ToString();
                dtpNgaySinh.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);
                txtSdt.Text = row.Cells["SoDienThoai"].Value.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                chkTrangThai.Checked = Convert.ToBoolean(row.Cells["TrangThai"].Value);
                txtTenDangNhap.Text = row.Cells["TenDangNhap"].Value.ToString();
                cboQuyen.SelectedItem = row.Cells["Quyen"].Value.ToString();
                txtMatKhau.Text = ""; // Không hiển thị mật khẩu cũ

                string imageName = row.Cells["HinhAnh"].Value.ToString();
                if (!string.IsNullOrEmpty(imageName))
                {
                    string imagePath = Path.Combine(Application.StartupPath, "Images", imageName);
                    if (File.Exists(imagePath))
                    {
                        picHinhAnh.Image = Image.FromFile(imagePath);
                        currentImagePath = imagePath;
                    }
                    else
                    {
                        picHinhAnh.Image = null;
                    }
                }
                else
                {
                    picHinhAnh.Image = null;
                }

                txtMaNV.Enabled = false;
                txtTenDangNhap.Enabled = false;
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            dgvNhanVien.DataSource = bus.TimKiemNhanVien(keyword);
        }

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            if (open.ShowDialog() == DialogResult.OK)
            {
                picHinhAnh.Image = new Bitmap(open.FileName);
                currentImagePath = open.FileName;
            }
        }

        private string SaveImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath)) return "";

            string imagesFolder = Path.Combine(Application.StartupPath, "Images");
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }

            string fileName = Path.GetFileName(imagePath);
            string destPath = Path.Combine(imagesFolder, fileName);

            if (imagePath != destPath)
            {
                File.Copy(imagePath, destPath, true);
            }

            return fileName;
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearFields();
        }
    }
}