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

namespace ReserveApp.ViewModel
{
    public class AdminAcceptingViewModel : ViewModelBase
    {
        private Applications selectedApplication;

        public Applications SelectedApplication
        {
            get { return selectedApplication; }
            set { Set(ref selectedApplication, value); }
        }


        private string text = "log";

        public string Text
        {
            get { return text = "loh"; }
            set { Set(ref text, value); }
        }


        private List<Applications> applications;

        public List<Applications> Applications
        {
            get { return applications; }
            set
            {
                Set(ref applications, value);
            }
        }

        private RelayCommand<object> acceptApplicationCommand;

        public RelayCommand<object> AcceptApplicationCommand
        {
            get
            {
                return acceptApplicationCommand ?? (acceptApplicationCommand = new RelayCommand<object>((obj) =>
                {
                    MessageBox.Show(obj.ToString());
                }));
            }
        }



        public AdminAcceptingViewModel(DateTime curDate, int classRoomId)
        {
            using (var db = new ReserveClassroomDBEntities())
            {
                

                //db.Applications.Add(new Model.Applications()
                //{
                //    ClassroomId = 2,
                //    UserId = 2,
                //    Date = DateTime.Now,
                //    LessonNumber = 3,
                //    StudentsCount = 13,
                //    GroupId = 3,
                //    Lesson = "sdf",
                //    Comment = "lohhhh"
                //});

                db.SaveChanges();

                var apps = db.Applications
                    .Include("Users")
                    .ToList();

                applications = apps;
            }
        }
    }
}
