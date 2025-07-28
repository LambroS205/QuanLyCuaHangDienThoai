using QuanLyCuaHangDienThoai.BUS;
using QuanLyCuaHangDienThoai.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyCuaHangDienThoai.GUI
{
    public partial class ucNhapHang : UserControl
    {
        private SanPhamBUS spBus = new SanPhamBUS();
        private NhaCungCapBUS nccBus = new NhaCungCapBUS();
        private PhieuNhapBUS pnBus = new PhieuNhapBUS();
        private TaiKhoanDTO currentUser;
        private List<ChiTietPhieuNhapDTO> danhSachNhap = new List<ChiTietPhieuNhapDTO>();

        public ucNhapHang(TaiKhoanDTO user)
        {
            InitializeComponent();
            this.currentUser = user;
        }

        private void ucNhapHang_Load(object sender, EventArgs e)
        {
            LoadComboBoxes();
            NewImportSlip();
        }

        private void LoadComboBoxes()
        {
            cboSanPham.DataSource = spBus.LayDanhSachSanPham();
            cboSanPham.DisplayMember = "TenSP";
            cboSanPham.ValueMember = "MaSP";

            cboNhaCungCap.DataSource = nccBus.LayDanhSachNhaCungCap();
            cboNhaCungCap.DisplayMember = "TenNCC";
            cboNhaCungCap.ValueMember = "MaNCC";

            cboSanPham.SelectedIndex = -1;
            cboNhaCungCap.SelectedIndex = -1;
        }

        private void NewImportSlip()
        {
            lblMaPN.Text = pnBus.GetNewMaPN();
            lblNhanVien.Text = $"{currentUser.HoTen} ({currentUser.MaNV})";
            danhSachNhap.Clear();
            UpdateDanhSachNhap();
            cboNhaCungCap.SelectedIndex = -1;
            cboSanPham.SelectedIndex = -1;
            numSoLuong.Value = 1;
            numDonGia.Value = 0;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (cboSanPham.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maSP = cboSanPham.SelectedValue.ToString();
            DataRowView selectedProductRow = (DataRowView)cboSanPham.SelectedItem;

            var itemInList = danhSachNhap.FirstOrDefault(item => item.MaSP == maSP);
            if (itemInList != null)
            {
                MessageBox.Show("Sản phẩm này đã có trong danh sách nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ChiTietPhieuNhapDTO newItem = new ChiTietPhieuNhapDTO
            {
                MaPN = lblMaPN.Text,
                MaSP = maSP,
                TenSP = selectedProductRow["TenSP"].ToString(),
                SoLuong = (int)numSoLuong.Value,
                DonGiaNhap = numDonGia.Value,
                ThanhTien = (int)numSoLuong.Value * numDonGia.Value
            };
            danhSachNhap.Add(newItem);
            UpdateDanhSachNhap();
        }

        private void UpdateDanhSachNhap()
        {
            dgvDanhSachNhap.DataSource = null;
            dgvDanhSachNhap.DataSource = danhSachNhap;
            SetupDataGridView();

            decimal tongTien = danhSachNhap.Sum(item => item.ThanhTien);
            lblTongTien.Text = tongTien.ToString("N0") + " VNĐ";
        }

        private void SetupDataGridView()
        {
            if (dgvDanhSachNhap.Columns.Count > 0)
            {
                dgvDanhSachNhap.Columns["MaPN"].Visible = false;
                dgvDanhSachNhap.Columns["MaSP"].HeaderText = "Mã SP";
                dgvDanhSachNhap.Columns["TenSP"].HeaderText = "Tên Sản Phẩm";
                dgvDanhSachNhap.Columns["SoLuong"].HeaderText = "Số Lượng";
                dgvDanhSachNhap.Columns["DonGiaNhap"].HeaderText = "Đơn Giá Nhập";
                dgvDanhSachNhap.Columns["ThanhTien"].HeaderText = "Thành Tiền";

                dgvDanhSachNhap.Columns["DonGiaNhap"].DefaultCellStyle.Format = "N0";
                dgvDanhSachNhap.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";

                dgvDanhSachNhap.Columns["TenSP"].FillWeight = 200;
                dgvDanhSachNhap.Columns["MaSP"].FillWeight = 80;
                dgvDanhSachNhap.Columns["SoLuong"].FillWeight = 60;
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDanhSachNhap.SelectedRows.Count > 0)
            {
                string maSP = dgvDanhSachNhap.SelectedRows[0].Cells["MaSP"].Value.ToString();
                var itemToRemove = danhSachNhap.FirstOrDefault(item => item.MaSP == maSP);
                if (itemToRemove != null)
                {
                    danhSachNhap.Remove(itemToRemove);
                    UpdateDanhSachNhap();
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (danhSachNhap.Count == 0 || cboNhaCungCap.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng thêm sản phẩm và chọn nhà cung cấp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PhieuNhapDTO newPN = new PhieuNhapDTO
            {
                MaPN = lblMaPN.Text,
                MaNV = currentUser.MaNV,
                MaNCC = cboNhaCungCap.SelectedValue.ToString(),
                NgayNhap = DateTime.Now,
                TongTien = danhSachNhap.Sum(item => item.ThanhTien),
                ChiTiet = danhSachNhap
            };

            if (pnBus.TaoPhieuNhap(newPN))
            {
                MessageBox.Show("Tạo phiếu nhập thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                NewImportSlip();
            }
            else
            {
                MessageBox.Show("Tạo phiếu nhập thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTaoMoi_Click(object sender, EventArgs e)
        {
            NewImportSlip();
        }
    }
}
