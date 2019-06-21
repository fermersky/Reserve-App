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
                    var applicationId = (int)obj; // get id of application was clicked

                    // MessageBox.Show(applicationId.ToString());

                    using (var db = new ReserveClassroomDBEntities())
                    {
                        var currentApp = applications.Where(a => a.Id == applicationId).First();

                        if (currentApp.StudentsCount < AvaliableSeatCount)
                        {
                            currentApp.StatusId = 2;

                            // update record in Application db table
                            db.Applications.AddOrUpdate(currentApp); 
                            await db.SaveChangesAsync();

                            // then refresh local collection Applications which binded
                            Applications = db.Applications.Include("Users").Include("Classrooms").Include("Groups").ToList();
                                
                            ApplicationView = CollectionViewSource.GetDefaultView(Applications);
                            ApplicationView.Filter = ApplicationRefresh;

                            // update property after accepting application
                            AvaliableSeatCount = getAvaliableSeatCount();

                            MessageBox.Show("Заявка одобрена!");

                        }

                        else
                            MessageBox.Show("Люди не влезут!");
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

                    // MessageBox.Show(applicationId.ToString());

                    using (var db = new ReserveClassroomDBEntities())
                    {
                        var appForRemove = db.Applications.FirstOrDefault(a => a.Id == applicationId);

                        // MessageBox.Show(listViewSelectedIndex.ToString());

                        db.Applications.Remove(appForRemove);
                        await db.SaveChangesAsync();

                        Applications = db.Applications.Include("Users").Include("Classrooms").Include("Groups").ToList();

                        ApplicationView = CollectionViewSource.GetDefaultView(Applications);
                        ApplicationView.Filter = ApplicationRefresh;

                        // update property after accepting application
                        AvaliableSeatCount = getAvaliableSeatCount();

                        // HideApplication?.Invoke();
                    }
                }));
            }
        }

        public AdminAcceptingViewModel(DateTime date, int classroomNumber, int lessonNumber)
        {
            using (var db = new ReserveClassroomDBEntities())
            {
                CurrentDate = date;
                LessonNumber = lessonNumber;
                ClassroomNumber = classroomNumber;

                // Load applications from db
                Applications = db.Applications.Include("Users").Include("Classrooms").Include("Groups").ToList();
                ApplicationView = CollectionViewSource.GetDefaultView(Applications);

                // select Accepted and OnProcess applications according to CurrentDate, LessonNumber and ClassroomNumber
                ApplicationView.Filter = ApplicationRefresh;

                // calculating count of free places in classroom
                AvaliableSeatCount = getAvaliableSeatCount();
            }
        }

        // Properties and Methods Helpers

        public int? TakenSeatCount { get; private set; }
        public int FreeSeatCount { get; private set; }

        private int? getTakenSeatCount()
        {
            return Applications // we take only accepted applications
                    .Where(a => a.LessonNumber == LessonNumber && a.Status.Type == "Accepted").Sum(a => a.StudentsCount);
        }

        private int getFreeSeatCount()
        {
            return Applications // take max capbility of classroom
                    .First(a => a.Classrooms.Number == ClassroomNumber).Classrooms.MaxPersonCount;
        }

        private int getAvaliableSeatCount()
        {
            var TakenSeatCount = getTakenSeatCount();
            var FreeSeatCount = getFreeSeatCount();

            // TakenSeatCount may be null if we don't have Accepted applications
            return (TakenSeatCount != null) ? FreeSeatCount - (int)TakenSeatCount : FreeSeatCount;
        }

        private bool ApplicationRefresh(object obj) // predicate for ApplicationView which bind to listview
        {
            var a = obj as Applications;

            return (a.Status.Type == "InProgress" || a.Status.Type == "Accepted")
                        && (a.Date == CurrentDate
                        && a.Classrooms.Number == ClassroomNumber
                        && a.LessonNumber == LessonNumber);
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

        private int avaliableSeatCount;

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

        public Action HideApplication { get; internal set; }
    }
}
