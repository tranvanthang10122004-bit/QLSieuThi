using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSieuThi.ViewModel
{
    class Login
    {
        Model.QLSieuThiEntities db = new Model.QLSieuThiEntities();
        
        public bool DangNhap(string username, string password) {
            var user = db.TaiKhoans.FirstOrDefault(u => u.TenTK == username && u.MatKhau == password);
            return user != null;
        }
    }
}
