using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace midterm_desktop
{
    public partial class Form1 : Form
    {
        private NhanVienService nhanVienService; 

        public Form1()
        {
            InitializeComponent();
            nhanVienService = new NhanVienService("dulieu.txt");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            btnLuu.Enabled = false;
            HienThiLenListView();
        }

        private void HienThiLenListView()
        {
            lstThongTinNhanVien.Items.Clear();
            foreach (var nv in nhanVienService.DanhSachNhanVien)
            {
                var item = new ListViewItem(nv.MaNV);
                item.SubItems.Add(nv.HoTen);
                item.SubItems.Add(nv.NgaySinh.ToString("dd/MM/yyyy"));
                item.SubItems.Add(nv.Email);
                lstThongTinNhanVien.Items.Add(item);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (btnThem.Text == "Thêm")
            {
                btnThem.Text = "Hủy";
                btnLuu.Enabled = true;
            }
            else
            {
                btnThem.Text = "Thêm";
                btnLuu.Enabled = false;
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Kiểm tra không để trống    
            if (string.IsNullOrWhiteSpace(txtMaSo.Text) ||
                string.IsNullOrWhiteSpace(txtHoTen.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra mã số không trùng    
            if (nhanVienService.MaSoDaTonTai(txtMaSo.Text))
            {
                MessageBox.Show("Mã số đã tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra định dạng email    
            try
            {
                var addr = new MailAddress(txtEmail.Text);
                if (addr.Address != txtEmail.Text)
                {
                    MessageBox.Show("Email không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Email không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra ngày sinh không lớn hơn ngày hiện tại    
            DateTime ngaySinh = dtpNgaySinh.Value.Date;
            if (ngaySinh > DateTime.Now.Date)
            {
                MessageBox.Show("Ngày sinh không được lớn hơn ngày hiện tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Thêm mới nhân viên vào danh sách    
            var nv = new NhanVien
            {
                MaNV = txtMaSo.Text,
                HoTen = txtHoTen.Text,
                NgaySinh = ngaySinh,
                Email = txtEmail.Text
            };
            nhanVienService.ThemNhanVien(nv);
            HienThiLenListView();
            btnThem.Text = "Thêm";
            btnLuu.Enabled = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                nhanVienService.SaveToFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu file: " + ex.Message);
            }
        }

        private void lstThongTinNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstThongTinNhanVien.SelectedItems.Count > 0)
            {
                var item = lstThongTinNhanVien.SelectedItems[0];
                txtMaSo.Text = item.SubItems[0].Text;
                txtHoTen.Text = item.SubItems[1].Text;
                dtpNgaySinh.Text = item.SubItems[2].Text;
                txtEmail.Text = item.SubItems[3].Text;
            }
        }
    }
}
