using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.Entity;
using ReserveApp.Model;

namespace ReserveApp.ViewModel
{
    public class MainViewModel : ViewModelBase // класс, в котором уже реализованы RelayCommand и INotifyPropertyChanged
    {
        private RelayCommand clickCommand;

        public RelayCommand ClickCommand
        {
            get
            {
                return clickCommand ?? (clickCommand = new RelayCommand(() =>
                {
                    // тело команды

                    var db = new ReserveClassroomDBEntities();

                    MessageBox.Show(db.Lessons.FirstOrDefault(u => u.Id == 1).Lesson);
                    LabelTxt = "ok";
                }, 
                () => 
                {
                    // условие выполнения команды

                    return true;
                }));
            }
        }

        private string labelTxt;

        public string LabelTxt
        {
            get => labelTxt;
            set => Set(ref labelTxt, value);

            // метод Set устанавливает новое значение и вызывает PropertyChanged
        }

        public MainViewModel(Users user)
        {
            labelTxt = "click btn before";
            
        }
    }
}
