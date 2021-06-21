using ApiApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace InterWMSDesctop.Converters
{
    class UserRoleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is UserRole userRole)
            {
                switch (userRole)
                {
                    case UserRole.Admin:
                        return "Заведующий склада";
                    case UserRole.Logistics:
                        return "Специалист по логистике";
                    case UserRole.Manager:
                        return "Приемщик";
                    case UserRole.Counterparty:
                        return "";
                    default:
                        return "";
                }
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
