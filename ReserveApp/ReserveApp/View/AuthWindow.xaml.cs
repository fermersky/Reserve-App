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
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
            this.DataContext = new AuthViewModel();

            var dc = (AuthViewModel)this.DataContext;
            dc.ShowErrorMsg += ShowErrorMsg;
            dc.CloseWindow += CloseWindow;
        }

        public void ShowErrorMsg(string msg) // maniulates with error msg label
        {
            var anim = new ThicknessAnimationUsingKeyFrames();

            errorTb.Text = msg;


            anim.KeyFrames.Add(new LinearThicknessKeyFrame(new Thickness(0, 60, 0, 0), KeyTime.FromPercent(0.4)));
            anim.Duration = TimeSpan.FromSeconds(0.8);
            anim.AutoReverse = true;

            errorTb.BeginAnimation(TextBlock.MarginProperty, anim);
        }

        public void CloseWindow()
        {
            this.Close();
        }
    }
}
