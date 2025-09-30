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
    /// Interaction logic for DangNhap.xaml
    /// </summary>
    public partial class DangNhap : Window
    {
        public DangNhap()
        {
            InitializeComponent();
        }
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var vm = new ViewModel.Login();

            string user = usernameTextBox.Text;
            string pass = passwordBox.Password;

            if (vm.DangNhap(user, pass))
            {
                MessageBox.Show("Đăng nhập thành công!");

                var mainWindow = new BangDieuKhien();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show($"Sai tài khoản hoặc mật khẩu! ({user}/{pass})");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var mainWindow = new DangKy();
            mainWindow.Show();
        }
    }
}
