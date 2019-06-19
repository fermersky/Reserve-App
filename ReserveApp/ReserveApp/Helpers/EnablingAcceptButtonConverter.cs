using ReserveApp.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ReserveApp.Helpers
{
    class EnablingAcceptButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //int ApplicationId = (int)value;

            //using (var db = new ReserveClassroomDBEntities())
            //{
            //    var appStatus = db.Applications.First(a => a.Id == ApplicationId).StatusId;

            //    return (appStatus == 3) ? true : false;
            //}

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
