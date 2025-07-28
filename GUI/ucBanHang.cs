using QuanLyCuaHangDienThoai.BUS;
using QuanLyCuaHangDienThoai.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyCuaHangDienThoai.GUI
{
    public partial class ucBanHang : UserControl
    {
        private SanPhamBUS spBus = new SanPhamBUS();
        private KhachHangBUS khBus = new KhachHangBUS();
        private HoaDonBUS hdBus = new HoaDonBUS();
        private TaiKhoanDTO currentUser;
        private List<ChiTietHoaDonDTO> gioHang = new List<ChiTietHoaDonDTO>();
        private string lastInvoiceId = ""; // Lưu mã hóa đơn vừa tạo

        public ucBanHang(TaiKhoanDTO user)
        {
            InitializeComponent();
            this.currentUser = user;
        }

        private void ucBanHang_Load(object sender, EventArgs e)
        {
            LoadComboBoxes();
            NewInvoice();
        }

        private void LoadComboBoxes()
        {
            cboSanPham.DataSource = spBus.LayDanhSachSanPham();
            cboSanPham.DisplayMember = "TenSP";
            cboSanPham.ValueMember = "MaSP";

            cboKhachHang.DataSource = khBus.LayDanhSachKhachHang();
            cboKhachHang.DisplayMember = "HoTen";
            cboKhachHang.ValueMember = "MaKH";

            cboSanPham.SelectedIndex = -1;
            cboKhachHang.SelectedIndex = -1;
        }

        private void NewInvoice()
        {
            lblMaHD.Text = hdBus.GetNewMaHD();
            lblNhanVien.Text = $"{currentUser.HoTen} ({currentUser.MaNV})";
            gioHang.Clear();
            UpdateGioHang();
            cboKhachHang.SelectedIndex = -1;
            cboSanPham.SelectedIndex = -1;
            numSoLuong.Value = 1;

            lastInvoiceId = "";

            SetControlsState(true);
            btnInHoaDon.Enabled = false;
        }

        private void btnThemVaoGio_Click(object sender, EventArgs e)
        {
            if (cboSanPham.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maSP = cboSanPham.SelectedValue.ToString();
            DataRowView selectedProductRow = (DataRowView)cboSanPham.SelectedItem;

            int soLuongMua = (int)numSoLuong.Value;
            int soLuongTon = Convert.ToInt32(selectedProductRow["SoLuongTon"]);

            if (soLuongMua > soLuongTon)
            {
                MessageBox.Show($"Số lượng tồn kho không đủ. Chỉ còn {soLuongTon} sản phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var itemInCart = gioHang.FirstOrDefault(item => item.MaSP == maSP);
            if (itemInCart != null)
            {
                itemInCart.SoLuong += soLuongMua;
                itemInCart.ThanhTien = itemInCart.SoLuong * itemInCart.DonGia;
            }
            else
            {
                ChiTietHoaDonDTO newItem = new ChiTietHoaDonDTO
                {
                    MaHD = lblMaHD.Text,
                    MaSP = maSP,
                    TenSP = selectedProductRow["TenSP"].ToString(),
                    SoLuong = soLuongMua,
                    DonGia = Convert.ToDecimal(selectedProductRow["DonGiaBan"]),
                    ThanhTien = soLuongMua * Convert.ToDecimal(selectedProductRow["DonGiaBan"])
                };
                gioHang.Add(newItem);
            }
            UpdateGioHang();
        }

        private void UpdateGioHang()
        {
            dgvGioHang.DataSource = null;
            dgvGioHang.DataSource = gioHang;

            // Cấu hình các cột sau khi gán DataSource
            if (dgvGioHang.Columns.Count > 0)
            {
                dgvGioHang.Columns["MaHD"].Visible = false;
                dgvGioHang.Columns["MaSP"].HeaderText = "Mã SP";
                dgvGioHang.Columns["TenSP"].HeaderText = "Tên Sản Phẩm";
                dgvGioHang.Columns["SoLuong"].HeaderText = "Số Lượng";
                dgvGioHang.Columns["DonGia"].HeaderText = "Đơn Giá";
                dgvGioHang.Columns["ThanhTien"].HeaderText = "Thành Tiền";

                dgvGioHang.Columns["DonGia"].DefaultCellStyle.Format = "N0";
                dgvGioHang.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";

                dgvGioHang.Columns["TenSP"].FillWeight = 200;
                dgvGioHang.Columns["MaSP"].FillWeight = 80;
                dgvGioHang.Columns["SoLuong"].FillWeight = 60;
            }

            decimal tongTien = gioHang.Sum(item => item.ThanhTien);
            lblTongTien.Text = tongTien.ToString("N0") + " VNĐ";
        }

        private void btnXoaKhoiGio_Click(object sender, EventArgs e)
        {
            if (dgvGioHang.SelectedRows.Count > 0)
            {
                string maSP = dgvGioHang.SelectedRows[0].Cells["MaSP"].Value.ToString();
                var itemToRemove = gioHang.FirstOrDefault(item => item.MaSP == maSP);
                if (itemToRemove != null)
                {
                    gioHang.Remove(itemToRemove);
                    UpdateGioHang();
                }
            }
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (gioHang.Count == 0)
            {
                MessageBox.Show("Giỏ hàng đang trống. Vui lòng thêm sản phẩm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            HoaDonDTO newHD = new HoaDonDTO
            {
                MaHD = lblMaHD.Text,
                MaNV = currentUser.MaNV,
                MaKH = cboKhachHang.SelectedValue?.ToString(),
                NgayLap = DateTime.Now,
                TongTien = gioHang.Sum(item => item.ThanhTien),
                ChiTiet = gioHang
            };

            if (hdBus.TaoHoaDon(newHD))
            {
                MessageBox.Show("Tạo hóa đơn thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lastInvoiceId = newHD.MaHD;

                SetControlsState(false);
                btnInHoaDon.Enabled = true;
            }
            else
            {
                MessageBox.Show("Tạo hóa đơn thất bại. Đã có lỗi xảy ra.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTaoMoi_Click(object sender, EventArgs e)
        {
            NewInvoice();
        }

        private void btnInHoaDon_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lastInvoiceId))
            {
                frmChiTietHoaDon f = new frmChiTietHoaDon(lastInvoiceId);
                f.ShowDialog();
            }
            else
            {
                MessageBox.Show("Chưa có hóa đơn nào để in.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SetControlsState(bool enabled)
        {
            cboSanPham.Enabled = enabled;
            numSoLuong.Enabled = enabled;
            btnThemVaoGio.Enabled = enabled;
            cboKhachHang.Enabled = enabled;
            btnXoaKhoiGio.Enabled = enabled;
            btnThanhToan.Enabled = enabled;
        }
    }
}
