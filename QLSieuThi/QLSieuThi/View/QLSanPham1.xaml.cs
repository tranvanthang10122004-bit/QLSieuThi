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
using System.Windows.Shapes;

namespace QLSieuThi.View
{
    /// <summary>
    /// Interaction logic for QLSanPham1.xaml
    /// </summary>
    public partial class QLSanPham1 : Window
    {
        ViewModel.QLSanPham qlsp = new ViewModel.QLSanPham();
        public QLSanPham1()
        {
            InitializeComponent();
        }
        

        private void ProductsDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "ChiTietHoaDons" || e.PropertyName == "DanhMuc")
            {
                e.Cancel = true; // Ẩn cột Navigation
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            qlsp.loadData(productsDataGrid);
            qlsp.LoadDanhMuc(cb_MaDM);
            qlsp.LoadDanhMuc(CategoryComboBox);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Tạo đối tượng sản phẩm mới
                Model.SanPham sp = new Model.SanPham
                {
                   MaSP = int.Parse(txt_MaSP.Text),
                    MaDM = (int)cb_MaDM.SelectedValue, // nhớ binding dữ liệu cho ComboBox
                TenSP = txt_TenSP.Text,
                   // DonGia = decimal.Parse(txt_DonGia.Text) // chuyển từ string sang decimal
                };

                // Gọi ViewModel để thêm
                ViewModel.QLSanPham spvm = new ViewModel.QLSanPham();
                spvm.add(sp);
                spvm.loadData(productsDataGrid);
                MessageBox.Show("Thêm sản phẩm thành công");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm sản phẩm: " + ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (productsDataGrid.SelectedItem is Model.SanPham spChon)
            {
                ViewModel.QLSanPham spvm = new ViewModel.QLSanPham();
                spvm.XoaSanPham(spChon);

                MessageBox.Show("Xóa sản phẩm thành công");

                // Load lại DataGrid
                productsDataGrid.ItemsSource = null;
                spvm.loadData(productsDataGrid);
                 // bạn có thể viết hàm GetAllSanPham trong ViewModel
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (productsDataGrid.SelectedItem is Model.SanPham spChon)
            {
                ViewModel.QLSanPham spvm = new ViewModel.QLSanPham();

                var spCapNhat = new Model.SanPham
                {
                    MaSP = spChon.MaSP,  // giữ nguyên khóa chính
                    TenSP = txt_TenSP.Text,
                    MaDM = cb_MaDM.SelectedValue != null ? (int)cb_MaDM.SelectedValue : 0
                     // ép kiểu int
                    //DonGia = decimal.Parse(txt_DonGia.Text)
                };

                spvm.Sua(spCapNhat);

                MessageBox.Show("Cập nhật sản phẩm thành công");

                // Load lại DataGrid
                productsDataGrid.ItemsSource = null;
                spvm.loadData(productsDataGrid);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần cập nhật");
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.QLSanPham spvm = new ViewModel.QLSanPham();
            string keyword = searchTextBox.Text.Trim();

            var ketQua = spvm.Search(keyword);

            productsDataGrid.ItemsSource = ketQua;
        }

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            var manageProductsWindow = new BangDieuKhien();
            manageProductsWindow.Show();
        }
    }
    }

