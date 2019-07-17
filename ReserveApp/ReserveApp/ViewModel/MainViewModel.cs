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
    public class MainViewModel : ViewModelBase
    {
        private MainWindow window;
        private Users user;

        List<Applications> listApps;
        List<Classrooms> listClassrooms;

        Dictionary<DateTime, bool> DatesDictionary { set; get; } = new Dictionary<DateTime, bool>();
        private DateTime date { set; get; } = DateTime.Today; 
        

        // method returns true if there are "InProgress" applications on Date
        private bool IsChanged(DateTime date)
        {
            var items = listApps.FirstOrDefault(a => a.Date == date && a.Status.Type == "InProgress");
            return (items != null && user.Role == "admin");
        }


        // method adds buttons with date in top of MainWindow
        private void DateButtonsSet()
        {
            for (int i = 0; i < 31; i++)
            {
                DatesDictionary.Add(date, IsChanged(date));
                date = date.AddDays(1);
            }
            window.dates.ItemsSource = DatesDictionary;
        }

        public MainViewModel(Users user, MainWindow oldWindow)
        {
            using (var db = new ReserveClassroomDBEntities())
            {
                this.user = user;
                this.window = oldWindow;

                listApps = db.Applications.ToList(); // load applications
                listClassrooms = db.Classrooms.ToList(); // load classrooms
                window.classRoomNumber.ItemsSource = listClassrooms; // bind MainWindow container to local collection

                if (DatesDictionary.Count == 0)
                    DateButtonsSet();
            }        
        }    
    }
}
