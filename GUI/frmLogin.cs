using QuanLyCuaHangDienThoai.BUS;
using QuanLyCuaHangDienThoai.DTO;
using System;
using System.Windows.Forms;

namespace QuanLyCuaHangDienThoai.GUI
{
    public partial class frmLogin : Form
    {
        private TaiKhoanBUS taiKhoanBUS = new TaiKhoanBUS();

        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string tenDangNhap = txtTenDangNhap.Text.Trim();
            string matKhau = txtMatKhau.Text.Trim();

            TaiKhoanDTO loggedInUser = taiKhoanBUS.KiemTraDangNhap(tenDangNhap, matKhau);

            if (loggedInUser != null)
            {
                MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Mở form chính và truyền thông tin người dùng
                frmMain f = new frmMain(loggedInUser);
                this.Hide();
                f.ShowDialog();
                this.Close(); // Đóng form login sau khi form main đóng
            }
            else
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTenDangNhap.Focus();
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtMatKhau.PasswordChar = chkShowPassword.Checked ? '\0' : '●';
        }
    }
}
