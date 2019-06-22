using ReserveApp.Model;
using ReserveApp.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using System.Globalization;

namespace ReserveApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(Users user) // user is a param which sended from AuthViewModel [User or Admin]
        {
            InitializeComponent();
            this.DataContext = new MainViewModel(user, this);

            DatesDictionary = new List<DateSelector>();
            DateTime dateFirst = DateTime.Now; /*new DateTime(2019, 06, 01)*/
            listApps = new ReserveClassroomDBEntities().Applications.ToList();
            for (int i = 0; i < 31; i++)
            {
                DateSelector dateSelector = new DateSelector(user, listApps) { Date = dateFirst, IsChanged = false }; /*, ColorBrush = new SolidColorBrush(Color.FromRgb(103, 58, 183)) */
                DatesDictionary.Add(dateSelector);
                dateFirst = dateFirst.AddDays(1);
            }
            dates.ItemsSource = DatesDictionary;
        }
        List<DateSelector> DatesDictionary;
        List<Applications> listApps;
    }
}
