using ReserveApp.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ReserveApp.Helpers
{
    class EnablingAcceptButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                using (var db = new ReserveClassroomDBEntities())
                {
                    var appStatus = db.Applications.Include("Status").First(a => a.Status.Type == value.ToString()).StatusId;

                    // if application has status Accepted, disable "Принять" button
                    return (appStatus == 3) ? true : false; 
                }
            }
            catch { return true; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
