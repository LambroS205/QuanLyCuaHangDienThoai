using QuanLyCuaHangDienThoai.DTO;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyCuaHangDienThoai.GUI
{
    public partial class frmMain : Form
    {
        private TaiKhoanDTO currentUser;
        private Button currentButton; // Lưu lại button đang được chọn
        private Panel leftBorderBtn; // Thanh chỉ báo bên trái button

        public frmMain(TaiKhoanDTO user)
        {
            InitializeComponent();
            this.currentUser = user;

            // Khởi tạo thanh chỉ báo
            leftBorderBtn = new Panel();
            leftBorderBtn.Size = new Size(7, 49);
            panelMenu.Controls.Add(leftBorderBtn);
        }

        // Phương thức chung để kích hoạt một button
        private void ActivateButton(object senderBtn)
        {
            if (senderBtn != null)
            {
                // Hủy kích hoạt button cũ
                DisableButton();

                // Kích hoạt button mới
                currentButton = (Button)senderBtn;
                currentButton.BackColor = Color.FromArgb(46, 51, 73);
                currentButton.ForeColor = Color.FromArgb(0, 126, 249);
                currentButton.TextAlign = ContentAlignment.MiddleCenter;
                currentButton.TextImageRelation = TextImageRelation.TextBeforeImage;
                currentButton.ImageAlign = ContentAlignment.MiddleRight;

                // Di chuyển thanh chỉ báo
                leftBorderBtn.BackColor = Color.FromArgb(0, 126, 249);
                leftBorderBtn.Location = new Point(0, currentButton.Location.Y);
                leftBorderBtn.Visible = true;
                leftBorderBtn.BringToFront();
            }
        }

        // Phương thức để hủy kích hoạt một button
        private void DisableButton()
        {
            if (currentButton != null)
            {
                currentButton.BackColor = Color.FromArgb(24, 30, 54);
                currentButton.ForeColor = Color.FromArgb(0, 126, 249);
                currentButton.TextAlign = ContentAlignment.MiddleLeft;
                currentButton.TextImageRelation = TextImageRelation.ImageBeforeText;
                currentButton.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }

        // Một phương thức chung để thêm UserControl vào panel chính
        private void addUserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            panelMain.Controls.Clear();
            panelMain.Controls.Add(userControl);
            userControl.BringToFront();
        }

        private void btnBanHang_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            ucBanHang uc = new ucBanHang(this.currentUser);
            addUserControl(uc);
            lblTitle.Text = "Bán Hàng";
        }

        private void btnNhapHang_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            ucNhapHang uc = new ucNhapHang(this.currentUser);
            addUserControl(uc);
            lblTitle.Text = "Nhập Hàng";
        }

        private void btnSanPham_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            ucSanPham uc = new ucSanPham();
            addUserControl(uc);
            lblTitle.Text = "Quản Lý Sản Phẩm";
        }

        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            ucKhachHang uc = new ucKhachHang();
            addUserControl(uc);
            lblTitle.Text = "Quản Lý Khách Hàng";
        }

        private void btnNhaCungCap_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            ucNhaCungCap uc = new ucNhaCungCap();
            addUserControl(uc);
            lblTitle.Text = "Quản Lý Nhà Cung Cấp";
        }

        private void btnNhanVien_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            ucNhanVien uc = new ucNhanVien();
            addUserControl(uc);
            lblTitle.Text = "Quản Lý Nhân Viên";
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            ucThongKe uc = new ucThongKe();
            addUserControl(uc);
            lblTitle.Text = "Báo Cáo - Thống Kê";
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            if (currentUser != null)
            {
                lblUsername.Text = currentUser.HoTen;
                PhanQuyen();
            }
            timer1.Start();
            // Mặc định kích hoạt chức năng bán hàng khi mở
            btnBanHang.PerformClick();
        }

        private void PhanQuyen()
        {
            if (!currentUser.Quyen.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                btnNhanVien.Visible = false;
                btnThongKe.Visible = false;
                btnNhaCungCap.Visible = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
            lblDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất và thoát chương trình?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}