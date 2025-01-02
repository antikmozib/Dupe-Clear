// Copyright (C) 2017-2025 Antik Mozib. All rights reserved.

using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace DupeClear.Converters;

public class DuplicateFileGroupToRowBGConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int group)
        {
            if (group % 2 == 0)
            {
                object? fgBrush = new SolidColorBrush(Colors.LimeGreen, 0.25);
                if (fgBrush != null)
                {
                    return (Brush)fgBrush;
                }

                return null;
            }
            else
            {
                return new SolidColorBrush(Colors.Transparent);
            }
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
