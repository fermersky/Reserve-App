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
            this.DataContext = new MainViewModel(user);
            DateTime dateFirst = DateTime.Now;
            DateTime buf = dateFirst;
            DateTime dateLast = DateTime.Now.AddDays(31);
            int j = 0;
            for (int i = 0; i < 31; i++)
            {
                Button btn = new Button();
                btn.Content = buf.ToString("dd.MM");
                btn.FontSize = 9;
                btn.Width = 54;
                btn.Margin = new Thickness(3.5, 0, 2, 0);
                btn.Height = 30;
                stackPanel1.Children.Add(btn);
                buf = buf.AddDays(1);
            }
        }
        }
}
