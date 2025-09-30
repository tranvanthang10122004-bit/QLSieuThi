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
    /// Interaction logic for BangDieuKhien.xaml
    /// </summary>
    public partial class BangDieuKhien : Window
    {
        public BangDieuKhien()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var manageProductsWindow = new QLSanPham1();
            manageProductsWindow.Show();

            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var manageProductsWindow = new ManagerCustomer();
            manageProductsWindow.Show();

            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var manageProductsWindow = new Sales();
            manageProductsWindow.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var manageProductsWindow = new ManagerOders();
            manageProductsWindow.Show();
        }
    }
}
