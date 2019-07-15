using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ReserveApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ReserveApp.ViewModel
{
    public class ReserveViewModel : ViewModelBase
    {
        public ReserveViewModel(DateTime date, int classroomNumber, int lessonNumber, Users user)
        {
            CurrentDate = date;
            LessonNumber = lessonNumber;
            ClassroomNumber = classroomNumber;
            SelectedUser = user;

            // enabling checkboxes depending on user's role
            isCheckBoxActive = (user.Role == "admin") ? true : false; 
            
 
            using (var db = new ReserveClassroomDBEntities())
            {
                // Get data for comboboxes
                Users = db.Users.ToList();
                Groups = db.Groups.ToList();
            }
        }

        private RelayCommand<object> addApplicationCommand;

        public RelayCommand<object> AddApplicationCommand
        {
            get
            {
                return addApplicationCommand ?? (addApplicationCommand = new RelayCommand<object>(async (obj) =>
                {
                    StudentsCount = Convert.ToInt32(obj);

                    var application = new Applications()
                    {
                        ClassroomId   = this.ClassroomNumber,
                        UserId        = this.SelectedUser.Id,
                        Date          = this.CurrentDate,
                        LessonNumber  = this.LessonNumber,
                        GroupId       = this.SelectedGroup.Id,
                        StatusId      = this.isSheduled ? 1 : 3, // this is "Sheduled" lesson if checkbox is active. else - "InProgress"
                        Lesson        = this.Lesson,
                        StudentsCount = this.StudentsCount, // command recievs as param Students Count
                        Comment       = this.Comment,
                    };

                    using (var db = new ReserveClassroomDBEntities())
                    {
                        db.Applications.Add(application);

                        try
                        {
                            await db.SaveChangesAsync();
                            CloseWindow?.Invoke();
                        }
                        catch { MessageBox.Show("Что-то пошло не так :("); }
                    }

                }, IsAllAreaAreField));
            }
        }

        private bool IsAllAreaAreField(object obj)
        {
            return true;
        }

        // Properties to Bind

        private DateTime _currentDate;

        public DateTime CurrentDate
        {
            get => _currentDate; 
            set => Set(ref _currentDate, value); 
        }

        //

        private int _lessonNumber;

        public int LessonNumber
        {
            get => _lessonNumber;
            set => Set(ref _lessonNumber, value);
        }

        //

        private int _classroomNumber;

        public int ClassroomNumber
        {
            get => _classroomNumber; 
            set => Set(ref _classroomNumber, value); 
        }

        //

        private string _lesson;

        public string Lesson
        {
            get => _lesson; 
            set => Set(ref _lesson, value); 
        }

        //

        private string _comment;

        public string Comment
        {
            get => _comment; 
            set => Set(ref _comment, value); 
        }

        // combobox source

        private List<Users> _users; 
        public List<Users> Users
        {
            get => _users;
            set => Set(ref _users, value);
        }

        //

        private Groups _selectedGroup;
        public Groups SelectedGroup
        {
            get => _selectedGroup;
            set => Set(ref _selectedGroup, value);
        }

        //

        private Users _selectedUser;
        public Users SelectedUser
        {
            get => _selectedUser;
            set => Set(ref _selectedUser, value);
        }

        // combobox source

        private List<Groups> _groups; 
        public List<Groups> Groups
        {
            get => _groups;
            set => Set(ref _groups, value);
        }


        // property to enabling admin's privileges

        private bool _isCheckBoxActive; 
        public bool isCheckBoxActive
        {
            get => _isCheckBoxActive;
            set => Set(ref _isCheckBoxActive, value);
        }

        // 

        private int? _studentsCount; 
        public int? StudentsCount
        {
            get => _studentsCount;
            set => Set(ref _studentsCount, value);
        }

        // property for checkbox 

        private bool _isSheduled; 
        public bool isSheduled
        {
            get => _isSheduled;
            set => Set(ref _isSheduled, value);
        }

        // This actions manipulate with ReserveWindow

        public Action CloseWindow { get; internal set; }
        public Action<string> ShowErrorMsg { get; internal set; }
    }
}
