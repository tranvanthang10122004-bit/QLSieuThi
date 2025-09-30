using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace QLSieuThi.ViewModel
{
    class HoaDon
    {
        Model.QLSieuThiEntities db = new Model.QLSieuThiEntities();

        public void loadData(DataGrid dg)
        {
            dg.Items.Clear();
            dg.ItemsSource = db.HoaDons.ToList();
        }
    }
}
