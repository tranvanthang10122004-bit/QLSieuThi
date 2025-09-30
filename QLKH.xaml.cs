using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace QLKH
{
    public partial class MainWindow : Window
    {
        // Sửa tên server cho đúng môi trường của bạn
        private readonly string connectionString =
            @"Data Source=.\SQLEXPRESS;Initial Catalog=ManageShop;Integrated Security=True;TrustServerCertificate=True";

        private int _customerRoleId = -1;

        public MainWindow()
        {
            InitializeComponent();
        }

        // ===== Khởi động =====
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this)) return;

            try
            {
                _customerRoleId = GetCustomerRoleId();
                LoadCustomers("", "");
                SetStatus("Sẵn sàng.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo: " + ex.Message, "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                SetStatus("Lỗi khởi tạo.");
            }
        }

        // ===== Data Access =====
        private int GetCustomerRoleId()
        {
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(
                "SELECT TOP 1 MaVaiTro FROM VaiTro WHERE TenVaiTro = N'Khách hàng';", con))
            {
                con.Open();
                object val = cmd.ExecuteScalar();
                if (val == null)
                    throw new InvalidOperationException("Không tìm thấy vai trò 'Khách hàng'.");
                return Convert.ToInt32(val);
            }
        }

        private void LoadCustomers(string nameKw, string cccdKw)
        {
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(@"
                SELECT MaND, HoTen, CCCD, SoDT
                FROM NguoiDung
                WHERE MaVaiTro = @roleId
                  AND (@name = '' OR HoTen LIKE N'%'+@name+'%')
                  AND (@cccd = '' OR CCCD LIKE '%'+@cccd+'%')
                ORDER BY MaND DESC;", con))
            {
                cmd.Parameters.AddWithValue("@roleId", _customerRoleId);
                cmd.Parameters.AddWithValue("@name", nameKw ?? "");
                cmd.Parameters.AddWithValue("@cccd", cccdKw ?? "");

                con.Open();
                var dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                dgCustomers.ItemsSource = dt.DefaultView;
                SetStatus("Đang hiển thị " + dt.Rows.Count + " khách hàng.");
            }
        }

        private bool HasInvoices(int maNd)
        {
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(
                "SELECT CASE WHEN EXISTS(SELECT 1 FROM HoaDon WHERE MaND=@id) THEN 1 ELSE 0 END;", con))
            {
                cmd.Parameters.AddWithValue("@id", maNd);
                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) == 1;
            }
        }

        private bool IsDuplicateCCCD(string cccd, int? ignoreId)
        {
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(@"
                SELECT COUNT(1)
                FROM NguoiDung
                WHERE CCCD = @cccd
                  AND MaVaiTro = @roleId
                  AND (@ignore IS NULL OR MaND <> @ignore);", con))
            {
                cmd.Parameters.AddWithValue("@cccd", cccd);
                cmd.Parameters.AddWithValue("@roleId", _customerRoleId);
                cmd.Parameters.AddWithValue("@ignore", (object)ignoreId ?? DBNull.Value);

                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        // ===== Helpers =====
        private void SetStatus(string text) { txtStatus.Text = text; }

        private void ClearInputs()
        {
            txtMaND.Text = "";
            txtHoTen.Text = "";
            txtCCCD.Text = "";
            txtSoDT.Text = "";
            txtHoTen.Focus();
        }

        private bool ValidateInput(out string message)
        {
            message = "";
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
                message = "Vui lòng nhập họ tên.";
            else if (!Regex.IsMatch(txtCCCD.Text.Trim(), @"^\d{9,12}$"))
                message = "CCCD/CMND phải là 9–12 chữ số.";
            else if (!Regex.IsMatch(txtSoDT.Text.Trim(), @"^\d{9,11}$"))
                message = "Số điện thoại phải là 9–11 chữ số.";

            return message == "";
        }

        private void SelectRowById(int id)
        {
            var dv = dgCustomers.ItemsSource as DataView;
            if (dv == null) return;

            foreach (DataRowView row in dv)
            {
                if (row["MaND"] != DBNull.Value && Convert.ToInt32(row["MaND"]) == id)
                {
                    dgCustomers.SelectedItem = row;
                    dgCustomers.ScrollIntoView(row);
                    break;
                }
            }
        }

        // ===== Sự kiện =====
        private void SearchBoxes_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadCustomers(txtSearchName.Text.Trim(), txtSearchCCCD.Text.Trim());
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            LoadCustomers(txtSearchName.Text.Trim(), txtSearchCCCD.Text.Trim());
        }

        private void BtnReload_Click(object sender, RoutedEventArgs e)
        {
            txtSearchName.Text = "";
            txtSearchCCCD.Text = "";
            LoadCustomers("", "");
        }

        private void DgCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var r = dgCustomers.SelectedItem as DataRowView;
            if (r == null) return;

            txtMaND.Text = r["MaND"] == DBNull.Value ? "" : r["MaND"].ToString();
            txtHoTen.Text = r["HoTen"] == DBNull.Value ? "" : r["HoTen"].ToString();
            txtCCCD.Text = r["CCCD"] == DBNull.Value ? "" : r["CCCD"].ToString();
            txtSoDT.Text = r["SoDT"] == DBNull.Value ? "" : r["SoDT"].ToString();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string err;
            if (!ValidateInput(out err))
            {
                MessageBox.Show(err, "Thiếu/không hợp lệ",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (IsDuplicateCCCD(txtCCCD.Text.Trim(), null))
            {
                MessageBox.Show("CCCD đã tồn tại.", "Không thể thêm",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(@"
                INSERT INTO NguoiDung (HoTen, CCCD, SoDT, MaVaiTro)
                OUTPUT INSERTED.MaND
                VALUES (@HoTen, @CCCD, @SoDT, @MaVaiTro);", con))
            {
                cmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text.Trim());
                cmd.Parameters.AddWithValue("@CCCD", txtCCCD.Text.Trim());
                cmd.Parameters.AddWithValue("@SoDT", txtSoDT.Text.Trim());
                cmd.Parameters.AddWithValue("@MaVaiTro", _customerRoleId);

                con.Open();
                int newId = Convert.ToInt32(cmd.ExecuteScalar());
                LoadCustomers("", "");
                SelectRowById(newId);
                ClearInputs();
                SetStatus("Đã thêm khách hàng.");
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            int id;
            if (!int.TryParse(txtMaND.Text, out id))
            {
                MessageBox.Show("Chưa chọn khách hàng.", "Cập nhật",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            string err;
            if (!ValidateInput(out err))
            {
                MessageBox.Show(err, "Thiếu/không hợp lệ",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (IsDuplicateCCCD(txtCCCD.Text.Trim(), id))
            {
                MessageBox.Show("CCCD đã thuộc về khách hàng khác.", "Không thể cập nhật",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(@"
                UPDATE NguoiDung
                   SET HoTen=@HoTen, CCCD=@CCCD, SoDT=@SoDT
                 WHERE MaND=@MaND AND MaVaiTro=@MaVaiTro;", con))
            {
                cmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text.Trim());
                cmd.Parameters.AddWithValue("@CCCD", txtCCCD.Text.Trim());
                cmd.Parameters.AddWithValue("@SoDT", txtSoDT.Text.Trim());
                cmd.Parameters.AddWithValue("@MaND", id);
                cmd.Parameters.AddWithValue("@MaVaiTro", _customerRoleId);

                con.Open();
                int aff = cmd.ExecuteNonQuery();
                if (aff == 0)
                {
                    MessageBox.Show("Không tìm thấy khách hàng để cập nhật.", "Cập nhật",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                LoadCustomers("", "");
                SelectRowById(id);
                SetStatus("Đã cập nhật khách hàng.");
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            int id;
            if (!int.TryParse(txtMaND.Text, out id))
            {
                MessageBox.Show("Chưa chọn khách hàng.", "Xoá",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (HasInvoices(id))
            {
                MessageBox.Show("Khách hàng đã có hoá đơn nên không thể xoá.", "Ràng buộc dữ liệu",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (MessageBox.Show("Bạn chắc chắn muốn xoá khách hàng này?", "Xác nhận xoá",
                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;

            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(
                "DELETE FROM NguoiDung WHERE MaND=@MaND AND MaVaiTro=@MaVaiTro;", con))
            {
                cmd.Parameters.AddWithValue("@MaND", id);
                cmd.Parameters.AddWithValue("@MaVaiTro", _customerRoleId);

                con.Open();
                cmd.ExecuteNonQuery();

                LoadCustomers("", "");
                ClearInputs();
                SetStatus("Đã xoá khách hàng.");
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearInputs();
            SetStatus("Đã làm mới form.");
        }
    }
}
