// Copyright (C) 2024 Antik Mozib. All rights reserved.

using Avalonia.Controls;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace DupeClear.Converters;

public class IntToGridLengthConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int length)
        {
            return new GridLength(length);
        }

        return new GridLength(0);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is GridLength gridLength)
        {
            return (int)gridLength.Value;
        }

        return 0;
    }
}
