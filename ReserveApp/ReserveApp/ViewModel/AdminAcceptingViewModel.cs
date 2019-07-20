using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ReserveApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Data.Entity.Migrations;
using System.Windows.Threading;
using ReserveApp.View;

namespace ReserveApp.ViewModel
{
    public class AdminAcceptingViewModel : ViewModelBase
    {
        private RelayCommand<object> acceptApplicationCommand;

        public RelayCommand<object> AcceptApplicationCommand
        {
            get
            {
                return acceptApplicationCommand ?? (acceptApplicationCommand = new RelayCommand<object>(async (obj) =>
                {
                    // get id of application was clicked
                    var applicationId = (int)obj; 

                    using (var db = new ReserveClassroomDBEntities())
                    {
                        var currentApp = applications.Where(a => a.Id == applicationId).FirstOrDefault();

                        if (currentApp.StudentsCount <= AvaliableSeatCount)
                        {
                            currentApp.StatusId = db.Status.First(s => s.Type == "Accepted").Id;

                            // update record in Applications db table

                            try
                            {
                                db.Applications.AddOrUpdate(currentApp);
                                await db.SaveChangesAsync();
                            }
                            catch { ShowErrorMsg("Что-то не так ;("); }

                            // then refresh local Applications collection which binded
                            RefreshLocalApplicationView();

                            ShowSuccessMsg("Заявка одобрена!");

                            // then refresh Applications table in MainWindow
                            UpdateMainWindowBody();
                        }

                        else
                            ShowErrorMsg("Люди не влезут!");
                    } 
                })); 
            }
        }

        //

        private RelayCommand<object> deleteApplicationCommand;

        public RelayCommand<object> DeleteApplicationCommand
        {
            get
            {
                return deleteApplicationCommand ?? (deleteApplicationCommand = new RelayCommand<object>(async (obj) =>
                {
                    var applicationId = (int)obj; // get id of application was clicked

                    using (var db = new ReserveClassroomDBEntities())
                    {
                        var appForRemove = db.Applications.FirstOrDefault(a => a.Id == applicationId);

                        try
                        {
                            db.Applications.Remove(appForRemove);
                            await db.SaveChangesAsync();
                        }
                        catch { ShowErrorMsg("Что-то не так ;("); }

                        RefreshLocalApplicationView();

                        ShowSuccessMsg("Заявка удалена");

                        // then refresh Applications table in MainWindow
                        UpdateMainWindowBody();

                    }
                }));
            }
        }

        //

        private RelayCommand<object> openReserveWindowCommand;

        public RelayCommand<object> OpenReserveWindowCommand
        {
            get
            {
                return openReserveWindowCommand ?? (openReserveWindowCommand = new RelayCommand<object>((obj) =>
                {
                    var reserveWindow = new ReserveWindow();
                    var reserveDataContext = new ReserveViewModel
                    (
                        date: this.CurrentDate,
                        classroomNumber: this.ClassroomNumber,
                        lessonNumber: this.LessonNumber,
                        user: this.User
                    );

                    // set owner of ReserveWindow - MainWindow
                    // to update MainWindow grid after reserved application
                    reserveWindow.Owner = this.OwnedWindow.Owner;

                    // set datacontext
                    reserveWindow.DataContext = reserveDataContext;

                    // Subscibe vm actions to the methods of ReserveWindow
                    reserveDataContext.CloseWindow += reserveWindow.CloseWindow;
                    reserveDataContext.ShowErrorMsg += reserveWindow.ShowErrorMsg;
                    reserveDataContext.UpdateMainWindowBody += reserveWindow.UpdateMainWindowBody;

                    reserveWindow.ShowDialog();
                }));
            }
        }

        public AdminAcceptingViewModel(DateTime date, int classroomNumber, int lessonNumber, Users user, AdminAccepting ownedWindow)
        {
            using (var db = new ReserveClassroomDBEntities())
            {
                this.CurrentDate = date;
                this.LessonNumber = lessonNumber;
                this.ClassroomNumber = classroomNumber;
                this.User = user;
                this.OwnedWindow = ownedWindow;

                // Load applications from db
                Applications = db.Applications.Include("Users").Include("Classrooms").Include("Groups")
                    .Where(a => a.Date == CurrentDate
                        && a.Classrooms.Number == ClassroomNumber
                        && a.LessonNumber == LessonNumber).ToList();

                // Display only "Accepted" and "InProgress" Applications
                ApplicationView = CollectionViewSource.GetDefaultView(Applications
                    .Where(a => a.Status.Type == "InProgress" || a.Status.Type == "Accepted").ToList());
                AvaliableSeatCount = getAvaliableSeatCount();

            }
        }



        // Properties and Methods Helpers

        public Users User { get; set; }
        public AdminAccepting OwnedWindow { get; set; }
        public int? TakenSeatCount { get; private set; } = 0;
        public int FreeSeatCount { get; private set; } = 0;

        private int? getTakenSeatCount()
        {
             // take "Accepted" applications and "Sheduled" lessons             
             return Applications.Where(a => a.LessonNumber == this.LessonNumber &&
                        (a.Status.Type == "Accepted" || a.Status.Type == "Sheduled"))
                    .Sum(a => a.StudentsCount);
        }

        private int getFreeSeatCount()
        {
            // take max capbility of classroom
            using (var db = new ReserveClassroomDBEntities())
                return db.Classrooms.FirstOrDefault(g => g.Number == this.ClassroomNumber).MaxPersonCount;
        }

        private int getAvaliableSeatCount()
        {
            if (Applications.Count > 0) // if local Applications collection is not empty
            {
                var TakenSeatCount = getTakenSeatCount();
                var FreeSeatCount = getFreeSeatCount();

                // TakenSeatCount may be null if we don't have Accepted applications
                return (TakenSeatCount != null) 
                    ? FreeSeatCount - (int)TakenSeatCount 
                    : FreeSeatCount;
            }
            else
                return getFreeSeatCount();
        }

        public void RefreshLocalApplicationView()
        {
            using (var db = new ReserveClassroomDBEntities())
            {
                this.Applications = db.Applications.Include("Users").Include("Classrooms").Include("Groups")
                             .Where(a => a.Date == CurrentDate
                                 && a.Classrooms.Number == ClassroomNumber
                                 && a.LessonNumber == LessonNumber).ToList();

                this.ApplicationView = CollectionViewSource.GetDefaultView(Applications
                    .Where(a => (a.Status.Type == "InProgress" || a.Status.Type == "Accepted")).ToList());
                this.AvaliableSeatCount = getAvaliableSeatCount();
            }
        }
   

        // Properties to Bind

        private List<Applications> applications;

        public List<Applications> Applications
        {
            get { return applications; }
            set { Set(ref applications, value); }
        }

        private DateTime _currentDate;

        public DateTime CurrentDate
        {
            get { return _currentDate; }
            set { Set(ref _currentDate, value); }
        }

        //

        private int _lessonNumber;

        public int LessonNumber
        {
            get { return _lessonNumber; }
            set { Set(ref _lessonNumber, value); }
        }

        //

        private int _classroomNumber;

        public int ClassroomNumber
        {
            get { return _classroomNumber; }
            set { Set(ref _classroomNumber, value); }
        }

        // 

        private int avaliableSeatCount = 0;

        public int AvaliableSeatCount
        {
            get { return avaliableSeatCount; }
            set { Set(ref avaliableSeatCount, value); }
        }

        //

        private int listViewSelectedIndex;

        public int ListViewSelectedIndex
        {
            get { return listViewSelectedIndex; }
            set { Set(ref listViewSelectedIndex, value); }
        }


        private ICollectionView applicationView;
        public ICollectionView ApplicationView
        {
            set
            {
                Set(ref applicationView, value);
            }
            get { return applicationView; }
        }

        //

        public Action<string> ShowErrorMsg { get; internal set; }
        public Action<string> ShowSuccessMsg { get; internal set; }
        public Action UpdateMainWindowBody { get; internal set; }
    }
}
