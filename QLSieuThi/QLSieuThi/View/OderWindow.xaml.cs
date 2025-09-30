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
using QLSieuThi.Model;

namespace QLSieuThi.View
{
    /// <summary>
    /// Interaction logic for OderWindow.xaml
    /// </summary>
    public partial class OderWindow : Window
    {
        Model.QLSieuThiEntities db = new Model.QLSieuThiEntities();
        public OderWindow()
        {
            InitializeComponent();
            loadData();
            LoadPaymentMethods();
            LoadAllCartData();
        }

        public void loadData()
        {
            CartDataGrid.ItemsSource = db.SanPhams.ToList();
        }

        private void PaymentMethodComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = PaymentMethodComboBox.SelectedItem as PhuongThucThanhToan;
        }
        private void LoadPaymentMethods()
        {
            PaymentMethodComboBox.ItemsSource = db.PhuongThucThanhToans.ToList();
            PaymentMethodComboBox.DisplayMemberPath = "TenPhuongThuc";
            PaymentMethodComboBox.SelectedValuePath = "MaPhuongThuc";
        }

        private void LoadAllCartData()
        {
            var data = from ct in db.ChiTietHoaDons
                       join sp in db.SanPhams on ct.MaSP equals sp.MaSP
                       join hd in db.HoaDons on ct.MaHD equals hd.MaHoaDon
                       select new
                       {
                           MaHD = hd.MaHoaDon,
                           TenSP = sp.TenSP,
                           SoLuong = ct.SoLuong,
                           DonGia = sp.DonGia,
                           ThanhTien = ct.SoLuong * sp.DonGia
                       };
            CartDataGrid.ItemsSource = data.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            decimal tong = 0;

            foreach (var item in CartDataGrid.Items)
            {
                dynamic row = item;
                tong += Convert.ToDecimal(row.ThanhTien);
            }

            TotalAmountTextBlock.Text = tong.ToString("N0");
        }

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            var manageProductsWindow = new Sales();
            manageProductsWindow.Show();
        }
    }
}
