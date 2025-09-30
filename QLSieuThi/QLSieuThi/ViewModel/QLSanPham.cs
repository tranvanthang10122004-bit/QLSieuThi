using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace QLSieuThi.ViewModel
{
    class QLSanPham
    {
        Model.QLSieuThiEntities db = new Model.QLSieuThiEntities();

        public void loadData(DataGrid dg)
        {
            dg.Items.Clear();
            dg.ItemsSource = db.SanPhams.ToList();
        }
        public void LoadDanhMuc(ComboBox cb)//using System.Windows.Controls;
        {
            cb.ItemsSource = db.DanhMucs.ToList();
            cb.DisplayMemberPath = "TenDM";
            cb.SelectedValuePath = "MaDM";
        }
        public void add(Model.SanPham sp)
        {
            db.SanPhams.Add(sp);
            db.SaveChanges();
        }

        public void Sua(Model.SanPham spCapNhat)
        {
            Model.SanPham sp = db.SanPhams.Find(spCapNhat.MaSP);
            if (sp != null)
            {
                sp.TenSP = spCapNhat.TenSP;
                sp.MaDM = spCapNhat.MaDM;
                //sp.DonGia = spCapNhat.DonGia;

                //db.SaveChanges();
            }
        }

        public void XoaSanPham(Model.SanPham spXoa)
        {
            Model.SanPham sp = db.SanPhams.Find(spXoa.MaSP);
            if (sp != null)
            {
                db.SanPhams.Remove(sp);
                db.SaveChanges();
            }
        }

        public List<Model.SanPham> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return db.SanPhams.ToList(); // nếu để trống thì trả hết
            }
            return db.SanPhams
                     .Where(sp => sp.TenSP.Contains(keyword))
                     .ToList();
        }

    }
}
