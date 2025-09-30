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
    /// Interaction logic for ManagerOders.xaml
    /// </summary>
    public partial class ManagerOders : Window
    {
        private Model.QLSieuThiEntities db = new Model.QLSieuThiEntities();
        public ManagerOders()
        {
            InitializeComponent();
            LoadOrders();
            LoadStaff();
        }

        private void StartDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StartDatePicker.SelectedDate.HasValue)
            {
                DateTime startDate = StartDatePicker.SelectedDate.Value;
                MessageBox.Show($"Ngày bắt đầu: {startDate.ToShortDateString()}");
            }
        }

        private void EndDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EndDatePicker.SelectedDate.HasValue)
            {
                DateTime endDate = EndDatePicker.SelectedDate.Value;
                MessageBox.Show($"Ngày kết thúc: {endDate.ToShortDateString()}");
            }
        }

        private void StaffComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StaffComboBox.SelectedItem != null)
            {
                var staff = StaffComboBox.SelectedItem.ToString();
                MessageBox.Show($"Nhân viên chọn: {staff}");
            }
        }

        public class Order
        {
            public DateTime NgayBan { get; set; }
            public string TenKhachHang { get; set; }
            public string TaiKhoanNhanVien { get; set; }
            public decimal TongTien { get; set; }
            public string PhuongThucThanhToan { get; set; }
            public string StaffName { get; set; }
        }

        private void LoadOrders()
        {
            // Demo dữ liệu giả lập
            var orders = new List<Order>
            {
                new Order
                {
                    NgayBan = DateTime.Now,
                    TenKhachHang = "Nguyễn Văn A",
                    TaiKhoanNhanVien = "nv001",
                    TongTien = 150000,
                    PhuongThucThanhToan = "Tiền mặt"
                },
                new Order
                {
                    NgayBan = DateTime.Now.AddDays(-1),
                    TenKhachHang = "Trần Thị B",
                    TaiKhoanNhanVien = "nv002",
                    TongTien = 320000,
                    PhuongThucThanhToan = "Chuyển khoản"
                },
                new Order
        {
            NgayBan = DateTime.Now.AddDays(-2),
            TenKhachHang = "Phạm Văn C",        // Khách hàng
            TaiKhoanNhanVien = "nv003",         // Nhân viên bán
            TongTien = 210000,
            PhuongThucThanhToan = "Thẻ tín dụng"
        }
                  };


            ordersListView.ItemsSource = orders;

        }

        private void LoadStaff()
        {
            var staffList = new List<string>
    {
        "nv001 - Nguyễn Văn A",
        "nv002 - Trần Thị B",
        "nv003 - Lê Văn C"
    };

            StaffComboBox.ItemsSource = staffList;
        }

        private void OrdersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ordersListView.SelectedItem is Order selectedOrder)
            {
                OrderIdTextBlock.Text = selectedOrder.NgayBan.ToString("dd/MM/yyyy");
                CustomerNameTextBlock.Text = selectedOrder.TenKhachHang;
                EmployeeNameTextBlock.Text = selectedOrder.TaiKhoanNhanVien;
                TotalAmountTextBlock.Text = selectedOrder.TongTien.ToString("N0") + " VNĐ";
                PaymentMethodTextBlock.Text = selectedOrder.PhuongThucThanhToan;
            }
        }
    }
}
