using QuanLyCuaHangDienThoai.BUS;
using QuanLyCuaHangDienThoai.DTO;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace QuanLyCuaHangDienThoai.GUI
{
    public partial class ucSanPham : UserControl
    {
        private SanPhamBUS bus = new SanPhamBUS();
        private string currentImagePath = "";

        public ucSanPham()
        {
            InitializeComponent();
        }

        private void ucSanPham_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadComboBoxes();
            ClearFields();
        }

        private void LoadData()
        {
            dgvSanPham.DataSource = bus.LayDanhSachSanPham();
            SetupDataGridView();
        }

        private void SetupDataGridView()
        {
            if (dgvSanPham.Columns.Count > 0)
            {
                dgvSanPham.Columns["MaSP"].HeaderText = "Mã SP";
                dgvSanPham.Columns["TenSP"].HeaderText = "Tên Sản Phẩm";
                dgvSanPham.Columns["MaHang"].HeaderText = "Hãng";
                dgvSanPham.Columns["MaLoai"].HeaderText = "Loại";
                dgvSanPham.Columns["DonGiaBan"].HeaderText = "Đơn Giá";
                dgvSanPham.Columns["SoLuongTon"].HeaderText = "Tồn Kho";
                dgvSanPham.Columns["ThoiGianBaoHanh"].HeaderText = "Bảo Hành (tháng)";
                dgvSanPham.Columns["TrangThai"].HeaderText = "Trạng Thái";

                dgvSanPham.Columns["HinhAnh"].Visible = false;
                dgvSanPham.Columns["MoTa"].Visible = false;

                dgvSanPham.Columns["DonGiaBan"].DefaultCellStyle.Format = "N0";

                dgvSanPham.Columns["TenSP"].FillWeight = 200;
            }
        }

        private void LoadComboBoxes()
        {
            cboHang.DataSource = bus.LayDanhSachHang();
            cboHang.DisplayMember = "TenHang";
            cboHang.ValueMember = "MaHang";

            cboLoaiSP.DataSource = bus.LayDanhSachLoaiSP();
            cboLoaiSP.DisplayMember = "TenLoai";
            cboLoaiSP.ValueMember = "MaLoai";
        }

        private void ClearFields()
        {
            txtMaSP.Text = "";
            txtTenSP.Text = "";
            cboHang.SelectedIndex = -1;
            cboLoaiSP.SelectedIndex = -1;
            numDonGia.Value = 0;
            numSoLuong.Value = 0;
            numBaoHanh.Value = 0;
            txtMoTa.Text = "";
            chkTrangThai.Checked = true;
            picHinhAnh.Image = null;
            currentImagePath = "";
            txtMaSP.Enabled = true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                SanPhamDTO sp = new SanPhamDTO
                {
                    MaSP = txtMaSP.Text.Trim(),
                    TenSP = txtTenSP.Text.Trim(),
                    MaHang = cboHang.SelectedValue.ToString(),
                    MaLoai = cboLoaiSP.SelectedValue.ToString(),
                    DonGiaBan = numDonGia.Value,
                    SoLuongTon = (int)numSoLuong.Value,
                    ThoiGianBaoHanh = (int)numBaoHanh.Value,
                    MoTa = txtMoTa.Text.Trim(),
                    TrangThai = chkTrangThai.Checked,
                    HinhAnh = SaveImage(currentImagePath)
                };

                if (bus.ThemSanPham(sp))
                {
                    MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Thêm sản phẩm thất bại. Mã sản phẩm có thể đã tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                SanPhamDTO sp = new SanPhamDTO
                {
                    MaSP = txtMaSP.Text.Trim(),
                    TenSP = txtTenSP.Text.Trim(),
                    MaHang = cboHang.SelectedValue.ToString(),
                    MaLoai = cboLoaiSP.SelectedValue.ToString(),
                    DonGiaBan = numDonGia.Value,
                    SoLuongTon = (int)numSoLuong.Value,
                    ThoiGianBaoHanh = (int)numBaoHanh.Value,
                    MoTa = txtMoTa.Text.Trim(),
                    TrangThai = chkTrangThai.Checked,
                    HinhAnh = SaveImage(currentImagePath)
                };

                if (bus.SuaSanPham(sp))
                {
                    MessageBox.Show("Cập nhật sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Cập nhật sản phẩm thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maSP = txtMaSP.Text.Trim();
            if (string.IsNullOrEmpty(maSP))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm để xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if (bus.XoaSanPham(maSP))
                {
                    MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Xóa sản phẩm thất bại. Sản phẩm có thể đã được sử dụng trong hóa đơn hoặc phiếu nhập.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSanPham.Rows[e.RowIndex];
                txtMaSP.Text = row.Cells["MaSP"].Value.ToString();
                txtTenSP.Text = row.Cells["TenSP"].Value.ToString();
                cboHang.SelectedValue = row.Cells["MaHang"].Value.ToString();
                cboLoaiSP.SelectedValue = row.Cells["MaLoai"].Value.ToString();
                numDonGia.Value = Convert.ToDecimal(row.Cells["DonGiaBan"].Value);
                numSoLuong.Value = Convert.ToInt32(row.Cells["SoLuongTon"].Value);
                numBaoHanh.Value = Convert.ToInt32(row.Cells["ThoiGianBaoHanh"].Value);
                txtMoTa.Text = row.Cells["MoTa"].Value.ToString();
                chkTrangThai.Checked = (bool)row.Cells["TrangThai"].Value;

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

                txtMaSP.Enabled = false;
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            dgvSanPham.DataSource = bus.TimKiemSanPham(keyword);
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

        private void btnLichSuBan_Click(object sender, EventArgs e)
        {
            if (dgvSanPham.SelectedRows.Count > 0)
            {
                string maSP = dgvSanPham.SelectedRows[0].Cells["MaSP"].Value.ToString();
                string tenSP = dgvSanPham.SelectedRows[0].Cells["TenSP"].Value.ToString();
                frmLichSuBanHang f = new frmLichSuBanHang(maSP, tenSP);
                f.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để xem lịch sử.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
