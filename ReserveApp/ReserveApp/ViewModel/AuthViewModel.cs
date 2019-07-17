using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ReserveApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ReserveApp.ViewModel
{
    public class AuthViewModel : ViewModelBase
    {
        private string userLgn;

        public string UserLgn
        {
            get => userLgn;
            set => Set(ref userLgn, value);
        }

        private RelayCommand<object> loginCommand;
        private List<Users> usersTable;

        public RelayCommand<object> LoginCommand
        {
            get
            {
                return loginCommand ?? (loginCommand = new RelayCommand<object>(obj =>
                {
                    var userPwd = (obj as PasswordBox).Password;

                    if (!string.IsNullOrEmpty(userPwd) && !string.IsNullOrEmpty(userLgn)) // user input all the data
                    {
                        using (var db = new ReserveClassroomDBEntities())
                        {
                            var user = usersTable.FirstOrDefault(u => u.Login == userLgn); // get user by login

                            if (user == null) // user doesn't exist
                                ShowErrorMsg?.Invoke($"User with login \"{userLgn}\" not found!");
                            else
                            {
                                if (user.Password == userPwd) // success login
                                { 
                                    var mp = new MainWindow(user);
                                     //MainWindow startup Location = Center
                                    mp.Show();
                                }
                                else
                                    ShowErrorMsg?.Invoke($"Uncorrect password for login \"{userLgn}\"");
                            }
                        }
                    }
                    else
                        ShowErrorMsg?.Invoke("Fill all areas!");
                })); 
            }
        }

        public Action<string> ShowErrorMsg { get; internal set; } // this delegate recieves the method from view code behind which showes Error Message

        public AuthViewModel()
        {
            usersTable = new ReserveClassroomDBEntities().Users.ToList(); // load raw data from db
        }
    }
}
