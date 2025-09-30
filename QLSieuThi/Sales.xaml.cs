using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity.SqlServer;

namespace QuanLySieuThi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Sales : Window
    {
        private QuanLySieuThiEntities _context;

        public Sales()
        {
            InitializeComponent();
            _context = new QuanLySieuThiEntities();
            TaiDanhMuc();
            TaiSanPhamTheoDanhMuc();
        }

        private void TaiDanhMuc()
        {
            var danhMucs = _context.DanhMucs.ToList();

            // Tạo mục "Tất cả"
            var tatCa = new DanhMuc { MaDM = 0, TenDM = "Tất cả" };

            var danhSach = new List<DanhMuc> { tatCa };
            danhSach.AddRange(danhMucs);

            DanhMucComboBox.ItemsSource = danhSach;
            DanhMucComboBox.DisplayMemberPath = "TenDM";
            DanhMucComboBox.SelectedValuePath = "MaDM";
        }

        private void TaiSanPhamTheoDanhMuc(int? maDM = null)
        {
            IQueryable<SanPham> query = _context.SanPhams
                .Include(p => p.DanhMuc);

            if (maDM.HasValue && maDM.Value != 0)
            {
                query = query.Where(p => p.MaDM == maDM.Value);
            }

            DanhSachSanPham.ItemsSource = query.ToList();
        }

        private void TimKiemSanPhamTheoTen(string tuKhoa = null)
        {
            IQueryable<SanPham> query = _context.SanPhams
                .Include(p => p.DanhMuc);

            if (!string.IsNullOrEmpty(tuKhoa))
            {
                query = query.Where(p => SqlFunctions.PatIndex("%" + tuKhoa + "%", p.TenSP) > 0);
            }

            DanhSachSanPham.ItemsSource = query.ToList();
        }

        private void DanhMucComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var maDM = (int?)DanhMucComboBox.SelectedValue;
            TaiSanPhamTheoDanhMuc(maDM);
        }

        private void TimKiemTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tuKhoa = TimKiemTextBox.Text;
            TimKiemSanPhamTheoTen(tuKhoa);
        }

        private void DanhSachSanPham_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var spChon = DanhSachSanPham.SelectedItem as SanPham;

            if (spChon != null)
            {
                TenSanPhamTextBox.Text = spChon.TenSP;
                SoLuongTextBox.Text = "1";
            }
        }

        private void NutThemGioHang_Click(object sender, RoutedEventArgs e)
        {
            var spChon = DanhSachSanPham.SelectedItem as SanPham;
            if (spChon == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm.");
                return;
            }

            if (!int.TryParse(SoLuongTextBox.Text, out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng không hợp lệ.");
                return;
            }

            decimal donGia = 10000;
            decimal thanhTien = soLuong * donGia;

            var chiTiet = new ChiTietHoaDon
            {
                MaSP = spChon.MaSP,
                SoLuong = soLuong,
                DonGia = donGia,
                ThanhTien = thanhTien,
                SanPham = spChon
            };

            ThemHoacCapNhatGioHang(chiTiet);
            CapNhatTongTien();
        }

        private void ThemHoacCapNhatGioHang(ChiTietHoaDon moi)
        {
            var gioHang = DanhSachGioHang.ItemsSource as List<ChiTietHoaDon>;
            if (gioHang == null) gioHang = new List<ChiTietHoaDon>();

            var tonTai = gioHang.FirstOrDefault(i => i.MaSP == moi.MaSP);
            if (tonTai != null)
            {
                tonTai.SoLuong += moi.SoLuong;
                tonTai.ThanhTien = tonTai.SoLuong * tonTai.DonGia;
            }
            else
            {
                gioHang.Add(moi);
            }

            DanhSachGioHang.ItemsSource = null;
            DanhSachGioHang.ItemsSource = gioHang;
        }

        private void CapNhatTongTien()
        {
            var gioHang = DanhSachGioHang.ItemsSource as List<ChiTietHoaDon>;
            if (gioHang != null)
            {
                decimal tong = gioHang.Sum(i => i.ThanhTien);
                TongTienTextBlock.Text = tong.ToString("F2");
            }
        }

        private void DanhSachGioHang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var chon = DanhSachGioHang.SelectedItem as ChiTietHoaDon;
            if (chon != null)
            {
                TenSanPhamTextBox.Text = chon.SanPham.TenSP;
                SoLuongTextBox.Text = chon.SoLuong.ToString();
            }
        }

        private void NutSuaSanPham_Click(object sender, RoutedEventArgs e)
        {
            var chon = DanhSachGioHang.SelectedItem as ChiTietHoaDon;
            if (chon == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm trong giỏ.");
                return;
            }

            if (!int.TryParse(SoLuongTextBox.Text, out int soLuongMoi) || soLuongMoi <= 0)
            {
                MessageBox.Show("Số lượng không hợp lệ.");
                return;
            }

            chon.SoLuong = soLuongMoi;
            chon.ThanhTien = soLuongMoi * chon.DonGia;

            var gioHang = DanhSachGioHang.ItemsSource as List<ChiTietHoaDon>;
            DanhSachGioHang.ItemsSource = null;
            DanhSachGioHang.ItemsSource = gioHang;

            CapNhatTongTien();
        }

        private void NutXoaSanPham_Click(object sender, RoutedEventArgs e)
        {
            var chon = DanhSachGioHang.SelectedItem as ChiTietHoaDon;
            if (chon == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm trong giỏ.");
                return;
            }

            var gioHang = DanhSachGioHang.ItemsSource as List<ChiTietHoaDon>;
            if (gioHang != null)
            {
                gioHang.Remove(chon);
                DanhSachGioHang.ItemsSource = null;
                DanhSachGioHang.ItemsSource = gioHang;

                CapNhatTongTien();
            }
        }

        private void NutTiepTuc_Click(object sender, RoutedEventArgs e)
        {
            var gioHang = DanhSachGioHang.ItemsSource as List<ChiTietHoaDon>;
            if (gioHang == null || !gioHang.Any())
            {
                MessageBox.Show("Giỏ hàng trống.");
                return;
            }

            MessageBox.Show("Chuyển sang bước thanh toán...");
            // ở đây bạn có thể mở cửa sổ mới OrderWindow
        }
    }

}

