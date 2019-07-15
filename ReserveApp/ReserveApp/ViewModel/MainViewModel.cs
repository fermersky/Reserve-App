using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.Entity;
using ReserveApp.Model;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Globalization;

namespace ReserveApp.ViewModel
{
    public class MainViewModel : ViewModelBase // класс, в котором уже реализованы RelayCommand и INotifyPropertyChanged
    {
        private MainWindow window;
        private Users user;

        List<Applications> listApps;
        List<Classrooms> listClassrooms;

        Dictionary<DateTime, bool> DatesDictionary { set; get; } = new Dictionary<DateTime, bool>();
        //private DateTime date { set; get; } = DateTime.Today; /*new DateTime(2019, 06, 01);*/
        private DateTime date { set; get; } = new DateTime(2019, 06, 19); 



        //private bool isChanged;
        //public bool IsChanged
        //{
        //    get
        //    {
        //        var items = listApps.FirstOrDefault(a => a.Date == date && a.StatusId == 3);
        //        if (items != null && user.Role == "admin")
        //            return true;
        //        else
        //            return false;
        //    }
        //    set
        //    {
        //        var items = listApps.FirstOrDefault(a => a.Date == date && a.StatusId == 3);
        //        if (items != null && user.Role == "admin")
        //            isChanged = true;
        //        else
        //            isChanged = false;
        //        RaisePropertyChanged("IsChanged");
        //    }
        //}

        private bool IsChanged(DateTime date)
        {
            var items = listApps.FirstOrDefault(a => a.Date == date && a.StatusId == 3);
            return (items != null && user.Role == "admin");
        }


        private void DateButtonsSet()
        {
            for (int i = 0; i < 31; i++)
            {
                DatesDictionary.Add(date, IsChanged(date));
                date = date.AddDays(1);
            }
            window.dates.ItemsSource = DatesDictionary;
        }

        private List<Applications> applications;

        public List<Applications> Applications
        {
            get { return applications; }
            set { Set(ref applications, value); }
        }

        public MainViewModel(Users user, MainWindow oldWindow)
        {
            using (var db = new ReserveClassroomDBEntities())
            {
                this.user = user;
                this.window = oldWindow;

                listApps = db.Applications.ToList();
                listClassrooms = db.Classrooms.ToList();
                window.classRoomNumber.ItemsSource = listClassrooms;

                if (DatesDictionary.Count == 0)
                    DateButtonsSet();
            }        
        }    
    }
    //public class DateTimeToColorConverter : IValueConverter
    //{
    //    List<Applications> listApps = new ReserveClassroomDBEntities().Applications.ToList();
    //    public BrushConverter bc = new BrushConverter(); 
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        var items = listApps.FirstOrDefault(a => a.Date == (DateTime)value && a.StatusId == 3);
    //        if (items != null && MainWindow.userWindow.Role == "admin")
    //            return (Brush)bc.ConvertFrom("LightYellow"); 
    //        else
    //            return (Brush)bc.ConvertFrom("#673AB7");
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        return (Brush)bc.ConvertFrom("#673AB7");
    //    }
    //}

}
