// Copyright (C) 2017-2025 Antik Mozib. All rights reserved.

using Avalonia.Data.Converters;
using DupeClear.Helpers;
using System;
using System.Globalization;

namespace DupeClear.Converters;

public class LengthToStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is long length)
        {
            return length.ConvertLengthToString();
        }

        return string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
