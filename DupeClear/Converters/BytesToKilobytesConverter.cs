// Copyright (C) 2017-2025 Antik Mozib. All rights reserved.

using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace DupeClear.Converters;

public class BytesToKilobytesConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is long bytes)
        {
            return (decimal)(bytes / 1024);
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is decimal kbs)
        {
            return System.Convert.ToInt64(kbs) * 1024;
        }

        return null;
    }
}
