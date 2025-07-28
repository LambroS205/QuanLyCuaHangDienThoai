using iTextSharp.text;
using iTextSharp.text.pdf;
using QuanLyCuaHangDienThoai.BUS;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace QuanLyCuaHangDienThoai.GUI
{
    public partial class frmChiTietHoaDon : Form
    {
        private string maHD;
        private HoaDonBUS bus = new HoaDonBUS();
        private DataTable dtThongTinHD;
        private DataTable dtSanPham;

        public frmChiTietHoaDon(string maHD)
        {
            InitializeComponent();
            this.maHD = maHD;
        }

        private void frmChiTietHoaDon_Load(object sender, EventArgs e)
        {
            dtThongTinHD = bus.GetThongTinChiTietHoaDon(maHD);
            dtSanPham = bus.GetDanhSachSanPhamCuaHoaDon(maHD);

            if (dtThongTinHD.Rows.Count > 0)
            {
                DataRow row = dtThongTinHD.Rows[0];
                lblMaHD.Text = row["MaHD"].ToString();
                lblNgayLap.Text = Convert.ToDateTime(row["NgayLap"]).ToString("dd/MM/yyyy HH:mm:ss");
                lblNhanVien.Text = row["TenNhanVien"].ToString();
                lblTenKH.Text = row["TenKhachHang"].ToString();
                lblSdt.Text = row["SoDienThoai"].ToString();
                lblDiaChi.Text = row["DiaChi"].ToString();
                lblTongTien.Text = Convert.ToDecimal(row["TongTien"]).ToString("N0") + " VNĐ";
            }

            dgvSanPham.DataSource = dtSanPham;
        }

        private void btnInPDF_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
            saveFileDialog.FileName = $"HoaDon_{maHD}.pdf";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Document document = new Document(PageSize.A4, 20, 20, 20, 20);
                    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(saveFileDialog.FileName, FileMode.Create));
                    document.Open();

                    string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIAL.TTF");
                    BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                    Font fontTitle = new Font(baseFont, 16, iTextSharp.text.Font.BOLD);
                    Font fontHeader = new Font(baseFont, 12, iTextSharp.text.Font.BOLD);
                    Font fontNormal = new Font(baseFont, 11, iTextSharp.text.Font.NORMAL);
                    Font fontBold = new Font(baseFont, 11, iTextSharp.text.Font.BOLD);

                    Paragraph storeName = new Paragraph("CỬA HÀNG ĐIỆN THOẠI XYZ", fontHeader);
                    storeName.Alignment = Element.ALIGN_LEFT;
                    document.Add(storeName);

                    Paragraph storeAddress = new Paragraph("Địa chỉ: 123 ABC, Quận 1, TP. HCM", fontNormal);
                    storeAddress.Alignment = Element.ALIGN_LEFT;
                    document.Add(storeAddress);
                    document.Add(new Paragraph(" "));

                    Paragraph title = new Paragraph("HÓA ĐƠN BÁN HÀNG", fontTitle);
                    title.Alignment = Element.ALIGN_CENTER;
                    title.SpacingAfter = 20;
                    document.Add(title);

                    document.Add(new Paragraph($"Mã hóa đơn: {lblMaHD.Text}", fontNormal));
                    document.Add(new Paragraph($"Ngày lập: {lblNgayLap.Text}", fontNormal));
                    document.Add(new Paragraph($"Nhân viên: {lblNhanVien.Text}", fontNormal));
                    document.Add(new Paragraph($"Khách hàng: {lblTenKH.Text}", fontNormal));
                    document.Add(new Paragraph($"Số điện thoại: {lblSdt.Text}", fontNormal));
                    document.Add(new Paragraph($"Địa chỉ: {lblDiaChi.Text}", fontNormal));
                    document.Add(new Paragraph(" "));

                    PdfPTable table = new PdfPTable(5);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 1, 4, 1, 2, 2 });

                    table.AddCell(new Phrase("STT", fontHeader));
                    table.AddCell(new Phrase("Tên sản phẩm", fontHeader));
                    table.AddCell(new Phrase("SL", fontHeader));
                    table.AddCell(new Phrase("Đơn giá", fontHeader));
                    table.AddCell(new Phrase("Thành tiền", fontHeader));

                    int stt = 1;
                    foreach (DataRow row in dtSanPham.Rows)
                    {
                        table.AddCell(new Phrase(stt++.ToString(), fontNormal));
                        table.AddCell(new Phrase(row["TenSP"].ToString(), fontNormal));
                        table.AddCell(new Phrase(row["SoLuong"].ToString(), fontNormal));
                        table.AddCell(new Phrase(Convert.ToDecimal(row["DonGia"]).ToString("N0"), fontNormal));
                        table.AddCell(new Phrase(Convert.ToDecimal(row["ThanhTien"]).ToString("N0"), fontNormal));
                    }
                    document.Add(table);
                    document.Add(new Paragraph(" "));

                    Paragraph total = new Paragraph($"Tổng cộng: {lblTongTien.Text}", fontHeader);
                    total.Alignment = Element.ALIGN_RIGHT;
                    document.Add(total);
                    document.Add(new Paragraph(" "));

                    Paragraph footer = new Paragraph("Cảm ơn quý khách và hẹn gặp lại!", fontBold);
                    footer.Alignment = Element.ALIGN_CENTER;
                    footer.SpacingBefore = 20;
                    document.Add(footer);

                    document.Close();
                    MessageBox.Show("In hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi in hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}