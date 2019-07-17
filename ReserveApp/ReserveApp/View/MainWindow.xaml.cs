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
using ReserveApp.View;

namespace ReserveApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Users User { get; set; }
        public DateTime CurrentDate { get; set; } = DateTime.Today;
        public int ClassroomsCount { get; private set; }

        public Brush GetBackgroundAccordingToStatus(Applications app, int? freeSeats)
        {
            Color color;

            switch (app.Status.Type)
            {
                case "Sheduled": color = Color.FromRgb(98, 0, 234); break; // purple
                case "Accepted": color = Color.FromRgb(100, 221, 23); break; // green
                case "InProgress": color = Color.FromRgb(255, 171, 0); break; // yellow

                default: color = Colors.White; break;
            }

            if ((app.Status.Type == "Accepted" || app.Status.Type == "InProgress") && freeSeats == 0)
                color = Color.FromRgb(213, 0, 0); // red

            return new SolidColorBrush(color);
        }

        public void SetEmptyButton(int classroomNumber, int lessonNumber)
        {
            var btn = new Button()
            {
                Content = "-",
                Width = 80,
                Height = 71,
                VerticalAlignment = VerticalAlignment.Stretch,
                Tag = $"{classroomNumber}||{lessonNumber}||none",
                Background = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
            };
            var label = new Label() { Content = "", Margin = new Thickness(-20, -5, 0, 0), FontSize = 15, };

            btn.Click += BtnClickCallback;

            WrapApplicationsPanel.Children.Add(btn);
        }



        public MainWindow(Users user) // user is a param which sended from AuthViewModel [User or Admin]
        {
            InitializeComponent();

            this.DataContext = new MainViewModel(user, this);
            this.User = user;
            using (var db = new ReserveClassroomDBEntities())
            {
                this.ClassroomsCount = db.Classrooms.ToList().Count();
            };

            GenerateWindowBody();
        }

        private void GenerateWindowBody()
        {
            WrapApplicationsPanel.Children.Clear();

            using (var db = new ReserveClassroomDBEntities())
            {
                var localApps = db.Applications.Where(a => a.Date == this.CurrentDate).ToList();

                // Всего у нас 8 рядов (8 пар)
                for (int lessonNumber = 1; lessonNumber <= 8; lessonNumber++) 
                {
                    // Выбираем заявки или пары по каждой из 8 пар 
                    var applications = localApps.Where(a => a.LessonNumber == lessonNumber).ToList();

                    // Если есть записи с такой парой
                    if (applications.Count > 0) 
                    {
                        // Проверяем каждую аудитрию
                        for (int classroomNumber = 1; classroomNumber <= this.ClassroomsCount; classroomNumber++) 
                        {
                            var app = applications.FirstOrDefault(a => a.Classrooms.Number == classroomNumber);

                            // Если есть заявка на текщую итерируемую аудиторию
                            if (app != null)
                            {
                                // Количество заявок на одну пару + аудиторию
                                var countOfApplications = GetApplicationsCountForLesson(applications, classroomNumber);

                                // Считаем количество занятых мест с учетом пар по расписанию и утвежденных заявок
                                var countOfStudents = GetTakenSeatsForLesson(applications, classroomNumber);

                                // Считаем количество свободных мест
                                var freeSeats = app.Classrooms.MaxPersonCount - countOfStudents;


                                var btn = new Button()
                                {
                                    Content = freeSeats, // К-во свободных мест
                                    Width = 80,
                                    Height = 71,
                                    Tag = $"{classroomNumber}||{lessonNumber}||{app.Status.Type}",
                                    FontSize = 16,
                                    // Уставнавливаем bg в зависимости от статуса и количества свободных мест
                                    Background = GetBackgroundAccordingToStatus(app, freeSeats),
                                };

                                // Подписали метод на событие кнопки
                                btn.Click += BtnClickCallback;


                                var label = new Label()
                                {
                                    Content = countOfApplications, // К-во заявок
                                    Foreground = new SolidColorBrush(Colors.White),
                                    Margin = new Thickness(-20, -5, 0, 0),
                                    FontSize = 15,
                                };

                                // Добавили компоненты в MainWindow
                                WrapApplicationsPanel.Children.Add(btn);
                                WrapApplicationsPanel.Children.Add(label);
                            }
                            else
                                SetEmptyButton(classroomNumber, lessonNumber);
                        }
                    }
                    else // Если нет записей - ряд из пустых кнопкок
                        for (int i = 1; i <= this.ClassroomsCount; i++)
                            SetEmptyButton(i, lessonNumber);
                }
            }
        }

        // Method calculate count of taken seats for a one ceil
        private int? GetTakenSeatsForLesson(List<Applications> applications, int classroomNumber)
        {
            return applications
                    .Where(a => a.Classrooms.Number == classroomNumber 
                        && (a.Status.Type == "Accepted" 
                        || a.Status.Type == "Sheduled"))
                    .ToList()
                    .Sum(a => a.StudentsCount);
        }

        // Method calculate count of applications for a one ceil
        private int GetApplicationsCountForLesson(List<Applications> applications, int classrooomNumber)
        {
            // ugly linq, to get count of applications on one ceil lesson-classroom
            var app = from a in applications
                        where a.Classrooms.Number == classrooomNumber
                        group a by new { a.ClassroomId, a.LessonNumber } into a
                        select new { Count = a.Count() };

            return app.FirstOrDefault().Count;
        }

        // Method returns couple with parsed info about application from button tag
        (int classroom, int lesson, string type) ParseButtonTag(string tag)
        {
            string[] arr = tag.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);

            int classroom = int.Parse(arr[0]);
            int lesson = int.Parse(arr[1]);
            string type = arr[2];

            return (classroom, lesson, type);
        }


        // Method called by the click on ceil button with applications
        private void BtnClickCallback(object sender, RoutedEventArgs e)
        {
            // Button tag has a string with format "classroomNumber||lessonNumber||type"
            var appInfo = ParseButtonTag((sender as Button).Tag.ToString());

            // open the accepting window only if it is InProgress or Accepting application
            if (User.Role == "admin" && appInfo.type != "Sheduled")
            {
                var acceptingWindow = new AdminAccepting();
                var acceptingDataContext = new AdminAcceptingViewModel
                (
                    date: this.CurrentDate,
                    classroomNumber: appInfo.classroom,
                    lessonNumber: appInfo.lesson,
                    user: this.User
                );

                // Subscibe vm actions to the methods of AdminAccepting window
                acceptingDataContext.ShowSuccessMsg += acceptingWindow.ShowSuccessMsg;
                acceptingDataContext.ShowErrorMsg += acceptingWindow.ShowErrorMsg;

                acceptingWindow.DataContext = acceptingDataContext;

                // Show list of applications to apply or discard them
                acceptingWindow.ShowDialog();
            }
            else if (User.Role == "user" && appInfo.type != "Sheduled")
            {
                var reserveWindow = new ReserveWindow();
                var reserveDataContext = new ReserveViewModel
                (
                    date: this.CurrentDate,
                    classroomNumber: appInfo.classroom,
                    lessonNumber: appInfo.lesson,
                    user: this.User
                );

                // set datacontext
                reserveWindow.DataContext = reserveDataContext;

                // Subscibe vm actions to the methods of ReserveWindow
                reserveDataContext.CloseWindow += reserveWindow.CloseWindow;
                reserveDataContext.ShowErrorMsg += reserveWindow.ShowErrorMsg;

                reserveWindow.ShowDialog();
            }
        }


        private void Window_Activated(object sender, EventArgs e)
        {
            GenerateWindowBody();
        }

        private void DateChanged(object sender, RoutedEventArgs e)
        {
            // get date from button content property
            var dateStr = (sender as Button).Content.ToString();

            // set datetime variable from a string
            var newDate = DateTime.Parse(dateStr);
            this.CurrentDate = newDate;

            // update applications grid
            GenerateWindowBody();
        }
    }
}
