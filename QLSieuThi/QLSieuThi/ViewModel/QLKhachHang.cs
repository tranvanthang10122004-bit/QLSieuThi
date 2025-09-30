using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace QLSieuThi.ViewModel
{
    class QLKhachHang
    {
        Model.QLSieuThiEntities db = new Model.QLSieuThiEntities();

        public void loadData(DataGrid dg)
        {
            dg.Items.Clear();
            dg.ItemsSource = db.NguoiDungs.ToList();
        }

        public List<Model.NguoiDung> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return db.NguoiDungs.ToList(); // trả về tất cả khi không có từ khóa
            }

            return db.NguoiDungs
                     .Where(sp => sp.HoTen.Contains(keyword)) // tìm tất cả tên có chứa keyword
                     .ToList();
        }
    }
}
