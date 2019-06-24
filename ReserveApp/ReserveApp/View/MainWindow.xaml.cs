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
        //internal static Users userWindow;

        public MainWindow(Users user) // user is a param which sended from AuthViewModel [User or Admin]
        {
            InitializeComponent();
            //userWindow = user;
            this.DataContext = new MainViewModel(user, this);
        }
    }
}
