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
using System.Windows.Shapes;

namespace ReserveApp.View
{
    /// <summary>
    /// Interaction logic for AdminAccepting.xaml
    /// </summary>
    public partial class AdminAccepting : Window
    {
        public AdminAccepting()
        {
            InitializeComponent();
            this.DataContext = new AdminAcceptingViewModel(date: new DateTime(2019, 6, 19), classroomNumber: 4, lessonNumber: 4);

            // ApplicationsList.ItemsSource = (this.DataContext as AdminAcceptingViewModel).ApplicationView;
        }
    }
}
