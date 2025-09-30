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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using QLSieuThi.ViewModel;
using QLSieuThi.Model;

namespace QLSieuThi.View
{
    /// <summary>
    /// Interaction logic for DangKy.xaml
    /// </summary>
    public partial class DangKy : INotifyPropertyChanged
    {


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MK = PasswordBox.Password;  // gán password vào property MK
            RegisterCommand.Execute(null);

        }
        private string _tk;
        public string TK
        {
            get => _tk;
            set { _tk = value; OnPropertyChanged(); }
        }

        private string _mk;
        public string MK
        {
            get => _mk;
            set { _mk = value; OnPropertyChanged(); }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        private QLSieuThiEntities _context;

        public DangKy()
        {
            InitializeComponent();
            this.DataContext = new Icommand();
            _context = new QLSieuThiEntities();
            RegisterCommand = new RelayCommand(Register);
        }

        public ICommand RegisterCommand { get; set; }

        private void Register(object obj)
        {
            if (string.IsNullOrWhiteSpace(TK) || string.IsNullOrWhiteSpace(MK))
            {
                ErrorMessage = "Vui lòng nhập đầy đủ Tài khoản và Mật khẩu.";
                return;
            }

            var existing = _context.TaiKhoans.FirstOrDefault(x => x.TenTK == TK);
            if (existing != null)
            {
                ErrorMessage = "Tài khoản đã tồn tại.";
                return;
            }

            var newAccount = new TaiKhoan { TenTK = TK, MatKhau = MK };
            _context.TaiKhoans.Add(newAccount);
            _context.SaveChanges();

            MessageBox.Show("Đăng ký thành công!");
            Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive)?.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    

    }
}

    



    

