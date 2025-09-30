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
    /// Interaction logic for ManagerCustomer.xaml
    /// </summary>
    public partial class ManagerCustomer : Window
    {
        ViewModel.QLKhachHang qlkh = new ViewModel.QLKhachHang();
        public ManagerCustomer()
        {
            InitializeComponent();
        }

        private void CustomersDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            qlkh.loadData(customersDataGrid);
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.QLKhachHang spvm = new ViewModel.QLKhachHang();
            string keyword = searchTextBox.Text.Trim();

            var ketQua = spvm.Search(keyword);

            customersDataGrid.ItemsSource = ketQua;
        }

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            var manageProductsWindow = new BangDieuKhien();
            manageProductsWindow.Show();
        }
    }
    }

