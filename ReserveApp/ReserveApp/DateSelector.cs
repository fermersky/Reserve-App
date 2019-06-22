using GalaSoft.MvvmLight;
using ReserveApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReserveApp
{
    class DateSelector : ViewModelBase
    {
        private Users user;
        List<Applications> listApps;
        public DateSelector(Users user, List<Applications> listApps)
        {
            this.user = user;
            this.listApps = listApps;
        }
        private DateTime date;
        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                RaisePropertyChanged("Date");
            }
        }
        private bool isChanged;
        public bool IsChanged
        {
            get { return isChanged; }
            set
            {
                var items = listApps.FirstOrDefault(a => a.Date == Date && a.StatusId == 3);
                if (items != null && user.Role == "admin")
                    isChanged = true;
                else
                    isChanged = false;
                RaisePropertyChanged("IsChanged");
            }
        }

        //private SolidColorBrush colorBrush;
        //public SolidColorBrush ColorBrush
        //{
        //    get { return colorBrush; }
        //    set
        //    { 
        //        if (isChanged)
        //            colorBrush = new SolidColorBrush(Colors.Yellow);
        //        else
        //            colorBrush = new SolidColorBrush(Color.FromRgb(103, 58, 183));
        //        RaisePropertyChanged("ColorBrush");
        //    }
        //} 
    }
}
