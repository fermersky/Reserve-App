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

        public DateTime Today { get; set; } = new DateTime(2019, 06, 19);

        public Brush GetBackgroundAccordingToStatus(Applications app)
        {
            Color color;

            switch (app.Status.Type)
            {
                case "Sheduled": color = Colors.Blue; break;
                case "Accepted": color = Colors.Green; break;
                case "InProgress": color = Color.FromRgb(255, 228, 0); break;

                default: color = Colors.White; break;
            }

            return new SolidColorBrush(color);
        }

        public void SetEmptyButton()
        {
            var btn = new Button()
            {
                Content = "-",
                Width = 80,
                Height = 71,
                Background = new SolidColorBrush(Color.FromRgb(255, 255, 255))
            };
            var label = new Label() { Content = "", Margin = new Thickness(-20, -5, 0, 0), FontSize = 15, };

            WrapApplicationsPanel.Children.Add(btn);
        }

        public MainWindow(Users user) // user is a param which sended from AuthViewModel [User or Admin]
        {
            InitializeComponent();
            //userWindow = user;
            this.DataContext = new MainViewModel(user, this);



            using (var db = new ReserveClassroomDBEntities())
            {
                var localApps = db.Applications.Where(a => a.Date == this.Today).ToList();

                for (int lessonNumber = 1; lessonNumber <= 8; lessonNumber++) // всего у нас 8 рядов (8 пар)
                {
                    var applications = localApps.Where(a => a.LessonNumber == lessonNumber).ToList(); // выбираем заявки или пары по каждой из 8 пар 

                    if (applications.Count > 0) // если есть записи с такой парой
                    {
                        for (int classrooomNumber = 1; classrooomNumber <= 15; classrooomNumber++) // проверяем каждую аудитрию
                        {
                            var app = applications.FirstOrDefault(a => a.Classrooms.Number == classrooomNumber);

                            if (app != null)
                            {
                                // уродский linq, чтоб вытянуть количество заявок на одну ячейку пара-аудитория
                                var countOfApplications = 
                                    from a in applications
                                    where a.Classrooms.Number == classrooomNumber
                                    group a by new { a.ClassroomId, a.LessonNumber } into a
                                    select new { Count = a.Count() }; // вот его и берем

                                var countOfStudents = applications
                                    .Where(a => a.Classrooms.Number == classrooomNumber)
                                    .ToList()
                                    .Sum(a => a.StudentsCount);

                                var freeSeats = app.Classrooms.MaxPersonCount - countOfStudents;

                                var btn = new Button()
                                {
                                    Content = freeSeats, // к-во свободных мест
                                    Width = 80,
                                    Height = 71,
                                    //Background = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                                    Background = GetBackgroundAccordingToStatus(app), // уставнавливаем bg в зависимости от статуса
                                    Tag = "loh",
                                };

                                btn.Click += (sender, e) => 
                                {
                                    MessageBox.Show((sender as Button).Tag.ToString());
                                };

                                var label = new Label()
                                {
                                    Content = countOfApplications.FirstOrDefault().Count, // к-во заявок
                                    Foreground = new SolidColorBrush(Colors.White),
                                    Margin = new Thickness(-20, -5, 0, 0),
                                    FontSize = 15,
                                };

                                WrapApplicationsPanel.Children.Add(btn);
                                WrapApplicationsPanel.Children.Add(label);
                            }
                            else
                                SetEmptyButton();
                        }
                    }
                    else // если нет записей - ряд из пустых кнопкок
                        for (int i = 1; i <= 15; i++)
                            SetEmptyButton();
                }
            }
        }

    }
}
