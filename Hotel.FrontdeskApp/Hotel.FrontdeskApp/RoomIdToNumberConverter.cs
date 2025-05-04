using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using SharedModels;
using System.Collections.Generic;

namespace Hotel.FrontdeskApp
{
    public class RoomIdToNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int roomId && App.Current is App app && app.MainWindow is MainWindow main)
            {
                var room = main.GetRoomById(roomId);
                return room?.RoomNumber.ToString() ?? "Unknown";
            }

            return "Unknown";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
    

