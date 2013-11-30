using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace HellBlaster
{
	public class DiscrepancyColorConverter : IValueConverter
	{

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if ((value as bool?).GetValueOrDefault() == true)
				return new SolidColorBrush(Colors.Red);
			else
				return new SolidColorBrush(Colors.Green);
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
