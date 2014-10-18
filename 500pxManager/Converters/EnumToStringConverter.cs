using _500pxManager.Api.Entities;
using _500pxManager.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace _500pxManager.Converters
{
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value.ToString().ToWords();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var r = value.ToString().Replace(" ", "");
            Privacy parsedPrivacyValue;
            Category parsedCategoryValue;
            var parsed = Enum.TryParse(r, out parsedPrivacyValue);
            if (!parsed)
            {
                parsed = Enum.TryParse(r, out parsedCategoryValue);
                if (parsed)
                {
                    return parsedCategoryValue;
                }
            }
            return parsedPrivacyValue;
        }
    }
}
