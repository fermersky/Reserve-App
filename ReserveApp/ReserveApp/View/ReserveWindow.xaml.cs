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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ReserveApp.View
{
    /// <summary>
    /// Interaction logic for ReserveWindow.xaml
    /// </summary>
    public partial class ReserveWindow : Window
    {
        // Color theme for Success and Error message
        private Color ErrorBackgroundColor = Color.FromRgb(245, 208, 213);
        private Color ErrorForegroundColor = Color.FromRgb(103, 19, 34);
        private Color SuccessBackgroundColor = Color.FromRgb(198, 227, 253);
        private Color SuccessForegroundColor = Color.FromRgb(7, 61, 120);


        public ReserveWindow()
        {
            InitializeComponent();

            this.DataContext = new ReserveViewModel(date: new DateTime(2019, 6, 19), 
                classroomNumber: 4, lessonNumber: 4, 
                user: new ReserveClassroomDBEntities().Users.FirstOrDefault(u => u.Id == 2));

            var dc = this.DataContext as ReserveViewModel;
            dc.CloseWindow += CloseWindow;
            dc.ShowErrorMsg += ShowErrorMsg;
        }

        public void CloseWindow()
        {
            this.Close();
        }

        public void ShowErrorMsg(string msg) // maniulates with error msg label
        {
            var anim = new ThicknessAnimationUsingKeyFrames();

            if (errorTb.Margin == (new Thickness(0, 70, 0, 0)))
            {

                errorTb.Text = msg;
                errorTb.Background = new SolidColorBrush(ErrorBackgroundColor);
                errorTb.Foreground = new SolidColorBrush(ErrorForegroundColor);

                anim.KeyFrames.Add(new LinearThicknessKeyFrame(new Thickness(0, 8, 0, 0), KeyTime.FromPercent(0.4)));
                anim.Duration = TimeSpan.FromSeconds(0.8);
                anim.AutoReverse = true;

                errorTb.BeginAnimation(TextBlock.MarginProperty, anim);
            }
        }
    }
}
