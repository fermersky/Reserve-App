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
using System.Windows.Media.Animation;
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
            var dc = this.DataContext as AdminAcceptingViewModel;
            dc.HideApplication += this.HideApplication;
            // ApplicationsList.ItemsSource = (this.DataContext as AdminAcceptingViewModel).ApplicationView;
        }

        public void HideApplication()
        {
            var anim = new ThicknessAnimationUsingKeyFrames();

            var listViewItem = ApplicationsList.SelectedItem as ListViewItem;

            anim.KeyFrames.Add(new LinearThicknessKeyFrame(new Thickness(0, -18, 0, 0), KeyTime.FromPercent(0.30)));
            anim.KeyFrames.Add(new LinearThicknessKeyFrame(new Thickness(0, -20, 0, 0), KeyTime.FromPercent(1.00)));
            anim.Duration = TimeSpan.FromSeconds(1);
            anim.AutoReverse = true;

            listViewItem.BeginAnimation(TextBlock.MarginProperty, anim);
        }
    }
}
